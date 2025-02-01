using DoFramework.Processing;

namespace DoFrameworkTests.Validators;

public class SampleType { }

public class SampleTypeMultipleConstructors 
{
    public SampleTypeMultipleConstructors() { }

    public SampleTypeMultipleConstructors(string name) { }
}

public class SampleProces : Process
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

public class SampleComposer : IComposer
{
    public void Compose(IComposerWorkBench workBench)
    {
        throw new NotImplementedException();
    }
}

public class ProcessMultipleConstructors : Process
{
    public ProcessMultipleConstructors() { }

    public ProcessMultipleConstructors(string name) { }

    public override void Run()
    {
        throw new NotImplementedException();
    }
}

public class ComposerMultipleConstructors : IComposer
{ 
    public ComposerMultipleConstructors() { }
    public ComposerMultipleConstructors(string tsst) { }

    public void Compose(IComposerWorkBench workBench)
    {
        throw new NotImplementedException();
    }
}