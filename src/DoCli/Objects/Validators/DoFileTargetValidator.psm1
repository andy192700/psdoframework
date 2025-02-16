using namespace DoFramework.Validators;
using namespace System.Collections.Generic;

# Class to validate a target string by checking if it exists in the global targets dictionary.
# Implements the IValidator interface for strings, with a method to validate a target item.
class DoFileTargetValidator : IValidator[string] {

    # Validate method to check if the target item exists in the global targets.
    # Takes a string $item as input and returns a validation result.
    #
    # Parameters:
    #   $item (string): The target item to validate.
    #
    # Returns:
    #   IValidationResult: Contains a list of error messages if validation fails.
    [IValidationResult] Validate([string] $item) {
        [List[string]] $errors = [List[string]]::new();

        if (!$Global:targets.ContainsKey($item)) {
            $errors.Add("Could not locate target '$item'");
        }

        return [ValidationResult]::new($errors);
    }
}
