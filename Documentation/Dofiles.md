# Dofiles

A **dofile** is a PowerShell-based alternative to traditional `Makefile`s, designed to work seamlessly with the `doing` CLI command from the DoFramework.

This feature allows developers to execute project-related tasks without requiring the terminal to be in the project’s root directory, thanks to its satellite execution model.

Please see the associated functions to [create](./CLIFunctions.md#new-dofile) and [run](./CLIFunctions.md#exec) dofiles.

See [here](../dofile.ps1) for a working example.

## Key Features

- **PowerShell Syntax**: Write targets using familiar PowerShell constructs.
- **Named Targets**: Use `Target` blocks to define build/run/test logic.
- **CLI Flexibility**: Supports PowerShell-style parameters and switches.
- **Variable Overrides**: Define default variables and override them via CLI, the framework will create them if they are not defined.
- **Ease of use**: like a makefile a dofile.ps1 can live in the base of a repository meaning it can orchestrate/house automation tasks as soon as a terminal is opened there (like in VSCode or similar).

## 📄 Example Dofile
Below is some example content of a dofile, note that some of the variables have implicit types - up to a developer to choose whether this is important to them or not.

```powershell
$myVar = "hello world!!!";
[bool] $theBool = $false;
$homeDir = Get-Location;
[string] $processName = "SimpleProcess";
$composerName = "AdvancedComposer";
$testFilter = ".*";

Target A {
    if ($theBool) {
        Write-Host boolset;
    }

    Write-Host $myVar;
}

Target B -inherits A {
    Write-Host "the end";
}

