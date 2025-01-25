using System.Collections;
using System.Management.Automation;
using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents a proxy that extends <see cref="DispatchProxy"/> and implements <see cref="IProxy"/>.
/// </summary>
public class Proxy : DispatchProxy, IProxy
{
    /// <summary>
    /// Gets or sets the list of mocked methods.
    /// </summary>
    protected List<(string, IScriptBlockWrapper)> MockMethods { get; set; } = [];

    private readonly List<MethodCall> _methodCalls = [];

    /// <inheritdoc/>
    public void MockProperty(string propertyName, ScriptBlock scriptBlock)
    {
        MockMethod($"get_{propertyName}", scriptBlock);
    }

    /// <inheritdoc/>
    public void MockMethod(string methodName, ScriptBlock scriptBlock)
    {
        MockMethod(methodName, new ScriptBlockWrapper(new ScriptBlockExecutor(scriptBlock)));
    }

    /// <inheritdoc/>
    public void MockMethod(string methodName, IScriptBlockWrapper scriptBlockWrapper)
    {
        if (CheckMethodMocked(methodName, scriptBlockWrapper))
        {
            throw new Exception($"Method '{methodName}' has already been mocked with these parameters.");
        }

        MockMethods.Add((methodName, scriptBlockWrapper));
    }

    /// <inheritdoc/>
    public int CountCalls(string methodName, Dictionary<string, object> args = default!)
    {
        if (args is null)
        {
            return _methodCalls.Where(x => x.Method.Name.Equals(methodName)).Count();
        }

        args ??= [];

        return _methodCalls.Where(x =>
        {
            var parameterMatchCount = 0;

            foreach (var key in args.Keys)
            {
                if (x.Args.TryGetValue(key, out object? value)
                && value.GetType().Equals(args[key].GetType())
                && value.Equals(args[key]))
                {
                    parameterMatchCount++;
                }
            }

            return x.Method.Name.Equals(methodName)
                && parameterMatchCount == args.Keys.Count
                && x.Args.Count == args.Keys.Count;
        }).Count();
    }

    /// <inheritdoc/>
    public int CountPropertyCalls(string propertyName)
    {
        return _methodCalls.Where(x => x.Method.Name.Equals($"get_{propertyName}")).Count();
    }

    /// <summary>
    /// Invokes the specified method with the given arguments.
    /// </summary>
    /// <param name="targetMethod">The method information to use for invocation.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    /// <returns>The result of the method invocation.</returns>
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var parameters = new Dictionary<string, object>();

        for (var i = 0; i < args!.Length; i++)
        {
            parameters[targetMethod!.GetParameters()[i].Name!] = args[i]!;
        }

        var call = new MethodCall(targetMethod!, parameters);

        _methodCalls.Add(call);

        foreach (var mockMethod in MockMethods)
        {
            if (mockMethod.Item1.Equals(targetMethod!.Name))
            {
                var scriptBlockWrapper = mockMethod.Item2;

                var expectedParameters = scriptBlockWrapper.ReadParameters();

                var i = 0;

                foreach (var param in expectedParameters)
                {
                    var type = expectedParameters[param.Key];

                    if (parameters.TryGetValue(param.Key, out var value)
                    && (value.GetType().IsAssignableFrom(type)
                    || value.GetType().IsSubclassOf(type)))
                    {
                        i++;
                    }
                }

                if (i == expectedParameters.Count && i == parameters.Count)
                {
                    var result = scriptBlockWrapper.Invoke(targetMethod, args!);

                    if (result is not null)
                    {
                        if (IsPSListShowingAsItem(result, targetMethod))
                        {
                            var instance = Activator.CreateInstance(targetMethod.ReturnType);

                            var addMethod = targetMethod.ReturnType.GetMethod("Add");
                            addMethod!.Invoke(instance, [ result ]);

                            call.Result = instance;
                        }
                        else if (IsPSArrayShowingAsItem(result, targetMethod))
                        {
                            var elementType = targetMethod.ReturnType.GetElementType();

                            var arrayInstance = Array.CreateInstance(elementType!, 1);
                            arrayInstance.SetValue(result, 0);

                            call.Result = arrayInstance;
                        }
                        else
                        {
                            call.Result = result;
                        }
                    }
                    else
                    {
                        call.Result = result;
                    }


                    return call.Result;
                }
            }
        }

        if (!targetMethod!.ReturnType.Equals(typeof(void)))
        {
            call.Result = Activator.CreateInstance(targetMethod.ReturnType);

            return call.Result;
        }

        return null;
    }

    /// <summary>
    /// Checks if the specified method has been mocked with the given script block wrapper.
    /// </summary>
    /// <param name="methodName">The name of the method to check.</param>
    /// <param name="suppliedScriptBlock">The script block wrapper to check.</param>
    /// <returns><c>true</c> if the method has been mocked; otherwise, <c>false</c>.</returns>
    protected bool CheckMethodMocked(string methodName, IScriptBlockWrapper suppliedScriptBlock)
    {
        var givenParameters = suppliedScriptBlock.ReadParameters();

        var parameters = new List<ParameterInfo>();

        foreach (var param in givenParameters)
        {
            parameters.Add(new PowershellParameterInfo(param.Key, param.Value));
        }

        var returnType = suppliedScriptBlock.HasReturnType() ? typeof(object) : typeof(void);

        return CheckMethodMocked(new PowershellMethodInfo(methodName, [.. parameters], returnType));
    }

    /// <summary>
    /// Checks if the specified method has been mocked.
    /// </summary>
    /// <param name="method">The method information to check.</param>
    /// <returns><c>true</c> if the method has been mocked; otherwise, <c>false</c>.</returns>
    protected bool CheckMethodMocked(MethodInfo method)
    {
        var dictionary = new Dictionary<string, Type>();

        foreach (var param in method.GetParameters())
        {
            dictionary[param.Name!] = param.ParameterType;
        }

        foreach (var mockMethod in MockMethods)
        {
            if (mockMethod.Item1.Equals(method.Name))
            {
                var scriptBlockWrapper = mockMethod.Item2;

                var expectedParameters = scriptBlockWrapper.ReadParameters();

                var i = 0;

                foreach (var param in expectedParameters)
                {
                    var type = expectedParameters[param.Key];

                    if (dictionary.TryGetValue(param.Key, out var value) && value.Equals(param.Value))
                    {
                        i++;
                    }
                }

                if (i == dictionary.Count && i == expectedParameters.Count)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Resets the state of the proxy, clearing all mocked methods and method calls.
    /// </summary>
    public void Reset()
    {
        MockMethods.Clear();

        _methodCalls.Clear();
    }

    /// <summary>
    /// Determines whether the return type of the specified target method is a generic collection
    /// containing elements of the same type as the provided result object.
    /// </summary>
    /// <param name="result">The object to check against the generic type argument of the collection.</param>
    /// <param name="targetMethod">The method whose return type is being checked.</param>
    /// <returns>
    /// <c>true</c> if the return type of the target method is a generic collection with 
    /// a single generic type argument that matches the type of the result object; otherwise, <c>false</c>.
    /// </returns>
#pragma warning disable CA1822 // Mark members as static
    private bool IsPSListShowingAsItem(object result, MethodInfo targetMethod)
#pragma warning restore CA1822 // Mark members as static
    {
        return typeof(ICollection).IsAssignableFrom(targetMethod.ReturnType)
                && targetMethod.ReturnType.GenericTypeArguments.Length == 1
                && targetMethod.ReturnType.GenericTypeArguments[0] == result.GetType();
    }

    /// <summary>
    /// Determines whether the return type of the specified target method is an array
    /// containing elements of the same type as the provided result object.
    /// </summary>
    /// <param name="result">The object to check against the generic type argument of the array.</param>
    /// <param name="targetMethod">The method whose return type is being checked.</param>
    /// <returns>
    /// <c>true</c> if the return type of the target method is an array with 
    /// an element type that matches the type of the result object; otherwise, <c>false</c>.
    /// </returns>
#pragma warning disable CA1822 // Mark members as static
    private bool IsPSArrayShowingAsItem(object result, MethodInfo targetMethod)
#pragma warning restore CA1822 // Mark members as static
    { 
        return targetMethod.ReturnType.IsArray 
                && targetMethod.ReturnType.GetElementType() == result.GetType(); 
    }
}
