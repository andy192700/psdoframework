using namespace DoFramework.Processing;
using module "..\..\Modules\ConfigurableObject.psm1";

class ConfigurationSample : Process {
    [IContext] $Context;
    [MyConfig] $Config1;
    [MySecondConfig] $Config2;
    [MyThirdConfig] $Config3;

    ConfigurationSample(
        [IContext] $context,
        [MyConfig] $config1,
        [MySecondConfig] $config2,
        [MyThirdConfig] $config3) {
        $this.Context = $context;
        $this.Config1 = $config1;
        $this.Config2 = $config2;
        $this.Config3 = $config3;
    }
    
    [bool] Validate() {
        return $this.Context.Requires().
            ComposedBy("ConfigurationComposer").
            Verify();
    }

    [void] Run() {
        Write-Host "1: $($this.Config1.MyInt) $($this.Config1.MyString)";
        Write-Host "2: $($this.Config2.MyDouble) $($this.Config2.MyBool)";
        Write-Host "3: $($this.Config3.MyShort) $($this.Config3.MyFloat)";
    }
}
