using namespace DoFramework.Processing;
using module "..\..\Modules\ModuleWithFunctions.psm1";

# Simple example injecting a IContext allowing access to shared values set by either
#    .env* files
#    supplied directly via the command line (caller can override by appending -InputInteger SomeValue to the run call)
#    a previous process
# This example also parses a switch - true if is supplied, false if it is not supplied
class DoublesANumber : Process {
    [IContext] $Context;

    DoublesANumber(
        [IContext] $context) {
        $this.Context = $context;
    }

    [void] Run() {
        [int] $inputValue = $this.Context.Get("InputInteger");

        Write-Host "Input: $inputValue Output: $(DoubleANumber -number $inputValue)";

        Write-Host "The switch 'MySwitch' was supplied: $($this.Context.ParseSwitch("MySwitch"))";
    }
}
