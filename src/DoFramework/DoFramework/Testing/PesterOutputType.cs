namespace DoFramework.Testing;

/// <summary>
/// Specifies the output type for Pester test results.
/// </summary>
public enum PesterOutputType
{
    /// <summary>
    /// No output.
    /// </summary>
    None,

    /// <summary>
    /// Output in NUnit XML format.
    /// </summary>
    NUnitXml,

    /// <summary>
    /// Output in JUnit XML format.
    /// </summary>
    JUnitXml
}
