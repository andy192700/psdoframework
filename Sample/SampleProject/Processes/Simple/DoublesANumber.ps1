using namespace DoFramework.Processing;
using module "..\..\Modules\ModuleWithFunctions.psm1";

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
