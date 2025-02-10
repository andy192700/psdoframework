Add-Type -Language CSharp -TypeDefinition "public sealed class SealedClass {}";

Add-Type -TypeDefinition @"
using System.Collections.Generic;

public interface ISampleInterface
{
    string StringProperty { get; set; }
    void VoidNoParams();
    void VoidWithParams(object someObj, float someFloat);
    double ReturnsADoubleNoParams();
    double ReturnsADoubleWithParams(object someObj, float someFloat);
    List<string> ReturnsAListNoParams();
    List<string> ReturnsAListWithParams(object someObj, float someFloat);
    Dictionary<string, string> ReturnsADictionaryNoParams();
    Dictionary<string, string> ReturnsADictionaryWithParams(int someInt);
    string[] ReturnsAnArrayNoParams();
    string[] ReturnsAnArrayWithParams(int someInt);
}

public interface IExample {}

"@ -Language CSharp;

class BasicClass {
}

class MultipleConstructors {
    [string] $InjectedString = [string]::Empty;
    [int] $InjectedInt = 0;

    MultipleConstructors([string] $testString) {
        $this.InjectedString = $testString;
    }

    MultipleConstructors([string] $testString, [int] $testInt) {
        $this.InjectedInt = $testInt;
    }
}

class OverloadedMethods {
    [string] StringMethod() {
        return [string]::Empty;
    }

    [string] StringMethod([string] $input1) {
        return [string]::Empty;
    }

    [string] StringMethod([string] $input1, [int] $input2) {
        return [string]::Empty;
    }
}