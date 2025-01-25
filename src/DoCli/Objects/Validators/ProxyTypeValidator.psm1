using namespace DoFramework.Validators;
using namespace System.Reflection;
using namespace System.Collections.Generic;

<#
.SYNOPSIS
Class for validating proxy types within the DoFramework environment.

.DESCRIPTION
The ProxyTypeValidator class is designed to validate whether a given type can be 
proxied within the DoFramework environment. It checks if the type is an interface 
or an unsealed class, and returns validation results accordingly.
#>
class ProxyTypeValidator : IValidator[Type] {
    <#
    .SYNOPSIS
    Validates the provided type to determine if it can be proxied.

    .DESCRIPTION
    The Validate method checks if the type is an interface or an unsealed class. 
    It returns a validation result with any errors found during the validation process.
    #>
    [IValidationResult] Validate([Type] $type) {
        [List[string]] $errors = [List[string]]::new();

        if ($type.IsInterface) {
            return [ValidationResult]::new($errors);
        }

        if ($type.IsSealed) {
            $errors.Add("Cannot create a proxy for type $type - it must either be an interface or an unsealed type.");
        }

        return [ValidationResult]::new($errors);
    }
}
