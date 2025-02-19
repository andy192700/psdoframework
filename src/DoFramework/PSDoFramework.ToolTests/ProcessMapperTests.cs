using AutoFixture.Xunit2;
using DoFramework.Processing;
using FluentAssertions;
using Microsoft.Extensions.Options;
using PSDoFramework.Tool;
using System.Diagnostics;
using System.Runtime;
using System.Text;

namespace PSDoFramework.ToolTests;
public class ProcessMapperTests
{
    [Theory]
    [InlineAutoData]
    public void ProcessMapper_NoParams(PowerShellSettings powerShellSettings)
    {
        // Arrange
        var options = new PowerShellSettingsOptions(powerShellSettings);
        var mapper = new ProcessMapper(options);

        // Act
        var result = mapper.Map([]);

        // Assert
        result.Should().NotBeNull();

        result.StartInfo.FileName.Should().Be("pwsh");
        result.StartInfo.Arguments.Should().Be($"-ExecutionPolicy Bypass -Command \"{PredictCommand([], options)}\"");
        result.StartInfo.RedirectStandardError.Should().BeTrue();
        result.StartInfo.UseShellExecute.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData]
    public void ProcessMapper_SingleParam(
        PowerShellSettings powerShellSettings,
        string param)
    {
        // Arrange
        var options = new PowerShellSettingsOptions(powerShellSettings);
        var mapper = new ProcessMapper(options);

        // Act
        var result = mapper.Map([ param ]);

        // Assert
        result.Should().NotBeNull();

        result.StartInfo.FileName.Should().Be("pwsh");
        result.StartInfo.Arguments.Should().Be($"-ExecutionPolicy Bypass -Command \"{PredictCommand([param], options)}\"");
        result.StartInfo.RedirectStandardError.Should().BeTrue();
        result.StartInfo.UseShellExecute.Should().BeFalse();
    }

    [Theory]
    [InlineAutoData]
    public void ProcessMapper_MultiParam(
        PowerShellSettings powerShellSettings,
        string[] parameters)
    {
        // Arrange
        var options = new PowerShellSettingsOptions(powerShellSettings);
        var mapper = new ProcessMapper(options);

        // Act
        var result = mapper.Map(parameters);

        // Assert
        result.Should().NotBeNull();

        result.StartInfo.FileName.Should().Be("pwsh");
        result.StartInfo.Arguments.Should().Be($"-ExecutionPolicy Bypass -Command \"{PredictCommand(parameters, options)}\"");
        result.StartInfo.RedirectStandardError.Should().BeTrue();
        result.StartInfo.UseShellExecute.Should().BeFalse();
    }

    private string PredictCommand(string[] args, IOptions<PowerShellSettings> settings)
    {
        var doingArgs = new StringBuilder();

        for (int i = 0; i < args.Length; i++)
        {
            doingArgs.Append($" {args[i]}");
        }

        var version = settings.Value.FrameworkVersion;
        var repository = settings.Value.Repository;

        return @$"$ErrorActionPreferene = 'Stop';
        $module = Get-InstalledModule -Name PSDoFramework;
        if ($null -eq $module -or $module.Version -ne ""{version}"") {{
            Write-Host ""Installing the PowerShell Module 'PSdoFramework' Version: {version} Repository: {repository}"" -ForegroundColor Yellow;
            Install-Module -Name PSDoFramework -Repository {repository} -RequiredVersion {version} -Force;
        }}

        doing {doingArgs.ToString()};";
    }
}

public class PowerShellSettingsOptions : IOptions<PowerShellSettings>
{
    public PowerShellSettings Value { get; }

    public PowerShellSettingsOptions(PowerShellSettings settings)
    {
        Value = settings;
    }
}