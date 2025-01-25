using DoFramework.Domain;
using DoFramework.Mappers;
using DoFramework.Testing;
using FluentAssertions;

namespace DoFrameworkTests.Mappers;

public class SaveProjectContentsMapperTests
{
    [Theory]
    [InlineAutoMoqData]
    public void SaveProjectContentsMapper_CanMap(
        int processCount,
        int moduleCount,
        int processTestCount,
        int moduleTestCount,
        int classTestCount,
        string psVersion,
        string version,
        string name
    )
    {
        // Arrange
        var projectContents = new ProjectContents();
        projectContents.Name = name;
        projectContents.PSVersion = psVersion;
        projectContents.Version = version;

        var processDescriptors = new List<ProcessDescriptor>();
        var moduleDescriptors = new List<ModuleDescriptor>();
        var testDescriptors = new List<TestDescriptor>();

        for (var i = 0; i < processCount; i++) projectContents.Processes.Add(new ProcessDescriptor());
        for (var i = 0; i < moduleCount; i++) projectContents.Modules.Add(new ModuleDescriptor());

        for (var i = 0; i < processTestCount; i++)
        {
            projectContents.Tests.Add(new TestDescriptor
            {
                TestType = TestType.Process
            });
        }

        for (var i = 0; i < moduleTestCount; i++)
        {
            projectContents.Tests.Add(new TestDescriptor
            {
                TestType = TestType.Module
            });
        }

        for (var i = 0; i < classTestCount; i++)
        {
            projectContents.Tests.Add(new TestDescriptor
            {
                TestType = TestType.Process
            });
        }

        var sut = new SaveProjectContentsMapper();

        // Act
        var result = sut.Map(projectContents);

        // Assert
        result.Should().NotBeNull();

        result.GetType().Should().Be(typeof(ProjectContentsStorage));

        result.Name.Should().Be(projectContents.Name);
        result.Version.Should().Be(projectContents.Version);
        result.PSVersion.Should().Be(projectContents.PSVersion);

        result.Processes.Should().HaveCount(projectContents.Processes.Count);

        result.Modules.Should().HaveCount(projectContents.Modules.Count);

        result.Tests.ProcessTests.Should().HaveCount(projectContents.Tests.Count(x => x.TestType == TestType.Process));

        result.Tests.ModuleTests.Should().HaveCount(projectContents.Tests.Count(x => x.TestType == TestType.Module));
    }
}