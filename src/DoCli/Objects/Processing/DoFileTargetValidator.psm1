using namespace DoFramework.Validators;
using namespace System.Collections.Generic;

class DoFileTargetValidator : IValidator[string] {
    [IValidationResult] Validate([string] $item) {
        [List[string]] $errors = [List[string]]::new();

        if (!$Global:targets.ContainsKey($item)) {
            $errors.Add("Could not locate target '$item'");
        }

        return [ValidationResult]::new($errors);
    }
}