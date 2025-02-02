using DoFramework.Domain;
using DoFramework.FileSystem;
using DoFramework.Mappers;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Mappers;

public class ReadProjectContentsMapperTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ReadProjectContentsMapper_CanMap(ProjectContentsStorage projectContentsStorage)
    {
        // Arrange
        var processDescriptor = new ProcessDescriptor();
        var moduleDescriptor = new ModuleDescriptor();
        var composerDescriptor = new ComposerDescriptor();
        var testDescriptor = new TestDescriptor();

        var mockProcessDescriptorMapper = new Mock<IMapper<string, ProcessDescriptor>>();
        var mockModuleDescriptorMapper = new Mock<IMapper<string, ModuleDescriptor>>();
        var mockTestDescriptorMapper = new Mock<IMapper<string, TestDescriptor>>();
        var mockComposerDescriptorMapper = new Mock<IMapper<string, ComposerDescriptor>>();

        var osSanitise = new Mock<IOSSanitise>();

        mockProcessDescriptorMapper.Setup(x => x.Map(It.IsAny<string>())).Returns(processDescriptor);
        mockModuleDescriptorMapper.Setup(x => x.Map(It.IsAny<string>())).Returns(moduleDescriptor);
        mockTestDescriptorMapper.Setup(x => x.Map(It.IsAny<string>())).Returns(testDescriptor);
        mockComposerDescriptorMapper.Setup(x => x.Map(It.IsAny<string>())).Returns(composerDescriptor);

        var sut = new ReadProjectContentsMapper(
            mockProcessDescriptorMapper.Object,
            mockModuleDescriptorMapper.Object,
            mockTestDescriptorMapper.Object,
            mockComposerDescriptorMapper.Object,
            osSanitise.Object);

        // Act
        var result = sut.Map(projectContentsStorage);

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().Be(typeof(ProjectContents));

        result.Name.Should().Be(projectContentsStorage.Name);
        result.Version.Should().Be(projectContentsStorage.Version);
        result.PSVersion.Should().Be(projectContentsStorage.PSVersion);

        result.Processes.Should().HaveCount(projectContentsStorage.Processes.Count);

        result.Modules.Should().HaveCount(projectContentsStorage.Modules.Count);

        result.Composers.Should().HaveCount(projectContentsStorage.Composers.Count);

        result.Tests.Should().HaveCount(
               projectContentsStorage.Tests.ModuleTests.Count
             + projectContentsStorage.Tests.ProcessTests.Count
             + projectContentsStorage.Tests.ComposerTests.Count);
    }
}