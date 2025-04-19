using namespace DoFramework.Processing;

class TestProcess2 : Process {
    [IContext] $Context;
    [string] $Mode;

    TestProcess2([IContext] $context) {
        $this.Context = $context;
    }

    [bool] Validate() {
        $this.Mode = [string]::Empty;

        if ($this.Context.KeyExists("Mode")) {
            $this.Mode = $this.Context.Get("Mode");
        }

        return $this.Mode -ne "Invalid";
    }

    [void] Run() {
        if ($this.Mode -eq "Fail") {
            throw "Process Failed.";
        }
    }
}
