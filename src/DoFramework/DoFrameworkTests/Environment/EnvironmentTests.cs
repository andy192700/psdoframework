﻿using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.FileSystem;
using DoFramework.Mappers;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace DoFrameworkTests.Environment;

public class EnvironmentTests
{
    [Theory]
    [InlineAutoMoqData]
    public void Environment_DoesPopulateProperties(
        [Frozen] Mock<IReadProcessLocation> readProcessLocation,
        [Frozen] Mock<IFileManager> fileManager,
        ProjectContentsStorage projectContentsStorage,
        string path)
    {
        // Arrange/Act
        var jsonConverter = new DoFramework.Data.JsonConverter();
        var osSanitise = new Mock<IOSSanitise>();

        osSanitise.Setup(x => x.Sanitise(It.IsAny<string>())).Returns(path);

        var processDescriptorMapper = new ProcessDescriptorMapper(osSanitise.Object);
        var moduleDescriptorMapper = new ModuleDescriptorMapper(osSanitise.Object);
        var testDescriptorMapper = new TestDescriptorMapper(osSanitise.Object);
        var composerDescriptorMapper = new ComposerDescriptorMapper(osSanitise.Object);
        var contentsMapper = new ReadProjectContentsMapper(processDescriptorMapper, moduleDescriptorMapper, testDescriptorMapper, composerDescriptorMapper, osSanitise.Object);
        var contentsProvider = new ReadProjectContents(contentsMapper, readProcessLocation.Object, fileManager.Object, jsonConverter);

        string jsonString = JsonConvert.SerializeObject(projectContentsStorage, Formatting.Indented);

        fileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        fileManager.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonString);
        readProcessLocation.Setup(x => x.Read()).Returns(System.Environment.CurrentDirectory);

        var sut = new DoFramework.Environment.Environment(
            readProcessLocation.Object, 
            contentsProvider);

        var homeDir = readProcessLocation.Object.Read();
        var baseDir = $"{homeDir}{DoFramework.Environment.Environment.Separator}{projectContentsStorage.Name}{DoFramework.Environment.Environment.Separator}";

        // Assert
        sut.HomeDir.Should().NotBeNull();
        sut.HomeDir.Should().Be(homeDir);
        sut.ProcessesDir.Should().NotBeNull();
        sut.ProcessesDir.Should().Be($"{baseDir}Processes");
        sut.TestsDir.Should().NotBeNull();
        sut.TestsDir.Should().Be($"{baseDir}Tests");
        sut.ModuleDir.Should().NotBeNull();
        sut.ModuleDir.Should().Be($"{baseDir}Modules");
    }
}