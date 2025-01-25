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

public class ProcessMultipleConstructors : Process
{
    public ProcessMultipleConstructors() { }

    public ProcessMultipleConstructors(string name) { }

    public override void Run()
    {
        throw new NotImplementedException();
    }
}