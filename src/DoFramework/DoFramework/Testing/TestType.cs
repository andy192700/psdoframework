namespace DoFramework.Testing;

/// <summary>
/// Specifies the type of test.
/// </summary>
public enum TestType
{
    /// <summary>
    /// Test that involves a process.
    /// </summary>
    Process,

    /// <summary>
    /// Test that involves a module.
    /// </summary>
    Module,

    /// <summary>
    /// Test that involves a composer.
    /// </summary>
    Composer
}
