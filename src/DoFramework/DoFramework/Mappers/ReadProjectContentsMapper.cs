using DoFramework.Domain;
using DoFramework.FileSystem;
using DoFramework.Testing;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping project contents storage to project contents.
/// </summary>
/// <param name="processDescriptorMapper">The mapper for process descriptors.</param>
/// <param name="moduleDescriptorMapper">The mapper for module descriptors.</param>
/// <param name="testDescriptorMapper">The mapper for test descriptors.</param>
public class ReadProjectContentsMapper : IMapper<ProjectContentsStorage, ProjectContents>
{
    private readonly IMapper<string, ProcessDescriptor> _processDescriptorMapper;
    private readonly IMapper<string, ModuleDescriptor> _moduleDescriptorMapper;
    private readonly IMapper<string, TestDescriptor> _testDescriptorMapper;
    private readonly IMapper<string, ComposerDescriptor> _composerDescriptorMapper;

    private readonly IOSSanitise _osSanitise;

    public ReadProjectContentsMapper(
        IMapper<string, ProcessDescriptor> processDescriptorMapper,
        IMapper<string, ModuleDescriptor> moduleDescriptorMapper,
        IMapper<string, TestDescriptor> testDescriptorMapper,
        IMapper<string, ComposerDescriptor> composerDescriptorMapper,
        IOSSanitise osSanitise)
    {
        _processDescriptorMapper = processDescriptorMapper;
        _moduleDescriptorMapper = moduleDescriptorMapper;
        _testDescriptorMapper = testDescriptorMapper;
        _composerDescriptorMapper = composerDescriptorMapper;
        _osSanitise = osSanitise;
    }

    /// <summary>
    /// Maps the project contents storage to project contents.
    /// </summary>
    /// <param name="source">The project contents storage.</param>
    /// <returns>The mapped project contents.</returns>
    public ProjectContents Map(ProjectContentsStorage source)
    {
        var contents = new ProjectContents
        {
            Name = source.Name,
            Version = source.Version,
            PSVersion = source.PSVersion   
        };

        foreach (var process in source!.Processes)
        {
            contents.Processes.Add(_processDescriptorMapper.Map(_osSanitise.Sanitise(process)));
        }

        foreach (var test in source.Tests.ProcessTests)
        {
            var descriptor = _testDescriptorMapper.Map(_osSanitise.Sanitise(test));

            descriptor.TestType = TestType.Process;

            contents.Tests.Add(descriptor);
        }

        foreach (var test in source.Tests.ModuleTests)
        {
            var descriptor = _testDescriptorMapper.Map(_osSanitise.Sanitise(test));

            descriptor.TestType = TestType.Module;

            contents.Tests.Add(descriptor);
        }

        foreach (var module in source.Modules)
        {
            contents.Modules.Add(_moduleDescriptorMapper.Map(_osSanitise.Sanitise(module)));
        }

        foreach (var composer in source.Composers)
        {
            contents.Composers.Add(_composerDescriptorMapper.Map(_osSanitise.Sanitise(composer)));
        }

        return contents;
    }
}
