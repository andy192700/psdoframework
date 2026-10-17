using namespace DoFramework.Processing;

class ProcessWithExternalCheck : Process {    
    [IContext] $Context;

    ProcessWithExternalCheck(
        [IContext] $context) {
        $this.Context = $context;
    }
    
    [bool] Validate() {
        return $this.Context.Requires().
            Satisfies({
                return 1 -eq 1;
            }).
            ContextSatisfies({
                param([IContext] $c)

                return $c.Get("sample") -eq "123";
            }).
            Verify();
    }
    [void] Run() {
        # TODO: implement process
    }
}
