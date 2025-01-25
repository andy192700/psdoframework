using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents PowerShell parameter information, extending the <see cref="ParameterInfo"/> class.
/// </summary>
public class PowershellParameterInfo(string name, Type type) : ParameterInfo
{
    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    public override string? Name => name;

    /// <summary>
    /// Gets the type of the parameter.
    /// </summary>
    public override Type ParameterType => type;
}
