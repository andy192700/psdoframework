# Dotnet Tool Offering

The [PSDoFramework.Tool](https://www.nuget.org/packages/PSDoFramework.Tool/) is a .NET global tool called by `psdoing` that wraps the PowerShell-based `DoFramework` CLI function `doing`, enabling seamless usage across any terminal environment — not just PowerShell. Ideal for cross-platform work, CI/CD pipelines, allowing simple calls to the PowerShell module without the need to set up a complicated command.

## Installation

Requires LTS versions of the Dotnet SDK (currently 8) and PowerShell (currently 7.4), please ensure these are in place before proceeding.

Install globally from NuGet using the .NET CLI:

```
dotnet tool install --global PSDoFramework.Tool
```

## Usage

As previously mentioned the tool simply wraps the PowerShell function offered by the module such that `psdoing` is used for invocation as opposed to `doing`, see the [CLI Functions](./CLIFunctions.md) document for all provided features.

An example call using which creates a new project using the dotnet tool would look like:
```
psdoing new-project -name MyProject
```

As opposed to native PowerShell usage:
```
doing new-project -name MyProject
```

Above, the only difference to use the DoFramework from any terminal in Windows/MacOS/Linux is to prefix the `doing` function with `ps`.