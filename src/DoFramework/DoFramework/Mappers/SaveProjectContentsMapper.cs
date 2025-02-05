using DoFramework.Domain;
using DoFramework.Testing;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping project contents to project contents storage.
/// </summary>
public class SaveProjectContentsMapper : IMapper<ProjectContents, ProjectContentsStorage>
{
    /// <summary>
    /// Maps the project contents to project contents storage.
    /// </summary>
    /// <param name="source">The project contents.</param>
    /// <returns>The mapped project contents storage.</returns>
    public ProjectContentsStorage Map(ProjectContents source)
    {
        var contentsToSave = new ProjectContentsStorage
        {
            Name = source.Name,
            Version = source.Version,
            PSVersion = source.PSVersion
        };

        foreach (var process in source.Processes)
        {
            contentsToSave.Processes.Add(process.Path!);
        }

        foreach (var test in source.Tests)
        {
            switch (test.TestType)
            {
                case (TestType.Process):
                    {
                        contentsToSave.Tests.ProcessTests.Add(test.Path!);
                        break;
                    }
                case (TestType.Module):
                    {
                        contentsToSave.Tests.ModuleTests.Add(test.Path!);
                        break;
                    }
                case (TestType.Composer):
                    {
                        contentsToSave.Tests.ComposerTests.Add(test.Path!);
                        break;
                    }
            }
        }

        foreach (var module in source.Modules)
        {
            contentsToSave.Modules.Add(module.Path!);
        }

        foreach (var composer in source.Composers)
        {
            contentsToSave.Composers.Add(composer.Path!);
        }

        return contentsToSave;
    }
}
