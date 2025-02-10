using namespace DoFramework.Processing;

class TestProcess3 : Process {
    [IContext] $Context;
    [string] $Mode;

    TestProcess3([IContext] $context) {
        $this.Context = $context;
    }

    [void] Run() {
        $this.Context.AddOrUpdate("ContextEntry", "some_value");
    }
}
