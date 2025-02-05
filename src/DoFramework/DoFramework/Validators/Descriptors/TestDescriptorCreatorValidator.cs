using DoFramework.CLI;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using System.Text;

namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for test descriptor creation, extending the <see cref="DescriptorCreatorValidator"/> class and providing additional validation for test-specific naming conventions.
/// </summary>
public class TestDescriptorCreatorValidator(
    IEnvironment environment,
    IFileManager fileManager,
    ISimpleDataProvider<ProjectContents> readProjectContents,
    CLIFunctionParameters cliFunctionParameters)
    : DescriptorCreatorValidator(environment, fileManager, readProjectContents), IValidator<TestDescriptor>
{
    private readonly CLIFunctionParameters _cliFunctionParameters = cliFunctionParameters;
    /// <summary>
    /// Validates the specified test descriptor.
    /// </summary>
    /// <param name="item">The test descriptor to validate.</param>
    /// <returns>The result of the validation.</returns>
    public IValidationResult Validate(TestDescriptor item)
    {
        var result = base.Validate(item);

        if (item.TestType is null)
        {
            result.Errors.Add("TestType not set.");
        }

        if (!item.Name!.EndsWith("Tests"))
        {
            result.Errors.Add("Tests names must be suffixed with the string 'Tests'.");
        }

        var switchCount = HasOneTestSwitch();

        if (switchCount == 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Please specify a type of test to define.");
            sb.AppendLine("-forProcess for Process tests.");
            sb.AppendLine("-forModule for Module tests.");
            sb.AppendLine("-forComposer for Composer tests.");
            result.Errors.Add(sb.ToString());
        }

        if (switchCount > 1)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Multiple test types requested, only ONE can be selected.");
            sb.AppendLine("-forProcess for Process tests.");
            sb.AppendLine("-forModule for Module tests.");
            sb.AppendLine("-forComposer for Composer tests.");
            result.Errors.Add(sb.ToString());
        }

        return result;
    }

    private int HasOneTestSwitch()
    {
        var switches = new List<string>
        {
            "forProcess",
            "forModule",
            "forComposer"
        };

        var switchCount = 0;

        foreach (var switchesEntry in switches)
        {
            switchCount += _cliFunctionParameters.ParseSwitch(switchesEntry) ? 1 : 0;
        }

        return switchCount;
    }
}
