using System.Globalization;
using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents PowerShell method information, extending the <see cref="MethodInfo"/> class.
/// </summary>
public class PowershellMethodInfo : MethodInfo
{
    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    public override string Name => _givenName;

    /// <summary>
    /// Gets the parameters of the method.
    /// </summary>
    /// <returns>An array of <see cref="ParameterInfo"/> representing the method's parameters.</returns>
    public override ParameterInfo[] GetParameters() => _parameters;

    /// <summary>
    /// Gets the return type of the method.
    /// </summary>
    public override Type ReturnType => _returnType;

    private readonly string _givenName;

    private readonly ParameterInfo[] _parameters = [];

    private readonly Type _returnType;

    /// <summary>
    /// Initializes a new instance of the <see cref="PowershellMethodInfo"/> class with the specified name, parameters, and return type.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameters">The parameters of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    public PowershellMethodInfo(
        string name,
        ParameterInfo[] parameters,
        Type returnType)
    {
        _givenName = name;
        _parameters = parameters;
        _returnType = returnType;
    }

    /// <summary>
    /// Gets the custom attributes associated with the return type of the method.
    /// </summary>
    public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

    /// <summary>
    /// Gets the attributes associated with the method.
    /// </summary>
    public override MethodAttributes Attributes => throw new NotImplementedException();

    /// <summary>
    /// Gets the method handle of the method.
    /// </summary>
    public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

    /// <summary>
    /// Gets the declaring type of the method.
    /// </summary>
    public override Type? DeclaringType => throw new NotImplementedException();

    /// <summary>
    /// Gets the reflected type of the method.
    /// </summary>
    public override Type? ReflectedType => throw new NotImplementedException();

    /// <summary>
    /// Gets the base definition of the method.
    /// </summary>
    /// <returns>The <see cref="MethodInfo"/> representing the base definition of the method.</returns>
    public override MethodInfo GetBaseDefinition()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the custom attributes of the method.
    /// </summary>
    /// <param name="inherit">Whether to search the method's inheritance chain to find the attributes.</param>
    /// <returns>An array of custom attributes.</returns>
    public override object[] GetCustomAttributes(bool inherit)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the custom attributes of the specified type applied to the method.
    /// </summary>
    /// <param name="attributeType">The type of the attributes to search for.</param>
    /// <param name="inherit">Whether to search the method's inheritance chain to find the attributes.</param>
    /// <returns>An array of custom attributes.</returns>
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the method implementation flags.
    /// </summary>
    /// <returns>The <see cref="MethodImplAttributes"/> representing the method implementation flags.</returns>
    public override MethodImplAttributes GetMethodImplementationFlags()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Invokes the method using the specified parameters.
    /// </summary>
    /// <param name="obj">The object on which to invoke the method.</param>
    /// <param name="invokeAttr">The binding flags.</param>
    /// <param name="binder">The binder.</param>
    /// <param name="parameters">The parameters to pass to the method.</param>
    /// <param name="culture">The culture information.</param>
    /// <returns>The result of the method invocation.</returns>
    public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether the method has the specified custom attribute.
    /// </summary>
    /// <param name="attributeType">The type of the attribute to search for.</param>
    /// <param name="inherit">Whether to search the method's inheritance chain to find the attribute.</param>
    /// <returns><c>true</c> if the method has the specified attribute; otherwise, <c>false</c>.</returns>
    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }
}
