using namespace DoFramework.Processing;
using module "..\..\Modules\ConfigurableObject.psm1";

class ConfigurationSample : Process {
    [IContext] $Context;
    [MyConfigurationService] $config;

    ConfigurationSample(
        [IContext] $context,
        [MyConfigurationService] $config) {
        $this.Context = $context;
        $this.Config = $config;
    }
    
    [bool] Validate() {
        return $this.Context.Requires().
            ComposedBy("ConfigurationComposer").
            Verify();
    }

    [void] Run() {
        Write-Host "1: $($this.Config.Config1.MyInt) $($this.Config.Config1.MyString)";
        Write-Host "2: $($this.Config.Config2.MyDouble) $($this.Config.Config2.MyBool)";
        Write-Host "3: $($this.Config.Config3.MyShort) $($this.Config.Config3.MyFloat)";
    }
}
