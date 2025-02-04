using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Testing;

namespace DoFramework.Validators;

/// <summary>
/// Represents a validator for descriptor creation, providing validation for file extensions and uniqueness of descriptors.
/// </summary>
public class DescriptorCreatorValidator(
    IEnvironment environment,
    IFileManager fileManager,
    ISimpleDataProvider<ProjectContents> readProjectContents) : IValidator<IDescriptor>
{
    private readonly IEnvironment _environment = environment;
    private readonly IFileManager _fileManager = fileManager;
    private readonly ISimpleDataProvider<ProjectContents> _readProjectContents = readProjectContents;

    /// <summary>
    /// Validates the specified descriptor.
    /// </summary>
    /// <param name="item">The descriptor to validate.</param>
    /// <returns>The result of the validation.</returns>
    public IValidationResult Validate(IDescriptor item)
    {
        var errors = new List<string>();

        var realPath = $"{item.GetDirectory(_environment)}{Environment.Environment.Separator}{item.Path}";

        var fileInfo = _fileManager.GetFileInfo(realPath);

        if (fileInfo.Extension != item.Extension)
        {
            errors.Add($"A {item.TypeName} file must have the extension '{item.Extension}'");
        }

        var contents = _readProjectContents.Provide();

        switch (item.TypeName)
        {
            case "Process":
                {
                    if (contents.Processes.Exists(t => t.Name!.Equals(item.Name)))
                    {
                        errors.Add($"A Process named {item.Name} already exists");
                    }

                    break;
                }
            case "Module":
                {
                    if (contents.Modules.Exists(t => t.Name!.Equals(item.Name)))
                    {
                        errors.Add($"A Module named {item.Name} already exists");
                    }

                    break;
                }
            case "Composer":
                {
                    if (contents.Composers.Exists(t => t.Name!.Equals(item.Name)))
                    {
                        errors.Add($"A Composer named {item.Name} already exists");
                    }

                    break;
                }
            case "Test":
                {
                    if (contents.Tests.Exists(t => t.Name!.Equals(item.Name)))
                    {
                        errors.Add($"A Test named {item.Name} already exists");
                    }

                    var testDescriptor = item as TestDescriptor;

                    var targetDescriptorName = testDescriptor!.Name!.Substring(0, testDescriptor.Name.Length - 5);

                    if (testDescriptor!.TestType == TestType.Module
                     && !contents.Modules.Any(t => t.Name!.Equals(targetDescriptorName)))
                    {
                        errors.Add($"Cannot create tests for the Module {targetDescriptorName} because it does not exist");
                    }

                    if (testDescriptor!.TestType == TestType.Process
                     && !contents.Processes.Any(t => t.Name!.Equals(targetDescriptorName)))
                    {
                        errors.Add($"Cannot create tests for the Process {targetDescriptorName} because it does not exist");
                    }

                    break;
                }
        }

        return new ValidationResult(errors);
    }
}
