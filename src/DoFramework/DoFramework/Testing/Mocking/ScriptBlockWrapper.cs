using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents a wrapper for a PowerShell script block, providing methods to invoke the script block, read parameters, and check for a return type.
/// </summary>
public class ScriptBlockWrapper(IScriptBlockExecutor scriptBlockExecutor) : IScriptBlockWrapper
{
    private readonly IScriptBlockExecutor _scriptBlockExecutor = scriptBlockExecutor;

    /// <summary>
    /// Invokes the script block with the specified method information and arguments.
    /// </summary>
    /// <param name="targetMethod">The method information to use for invocation.</param>
    /// <param name="args">The arguments to pass to the script block.</param>
    /// <returns>The result of the script block invocation.</returns>
    public object? Invoke(MethodInfo targetMethod, params object[] args)
    {
        object? result = null;

        var scriptResult = _scriptBlockExecutor.Invoke(args);

        var type = targetMethod.ReturnType.GetElementType();

        if (scriptResult is not null)
        {
            if (scriptResult is PSCustomObject
             && typeof(IList).IsAssignableFrom(targetMethod.ReturnType))
            {
                if (targetMethod.ReturnType.IsArray)
                {
                    result = Array.CreateInstance(type!, 0);
                }
                else
                {
                    result = Activator.CreateInstance(targetMethod.ReturnType);
                }
            }
            else if (scriptResult is IList arr)
            {
                if (targetMethod.ReturnType.IsArray)
                {
                    var array = Array.CreateInstance(type!, arr.Count);

                    arr.CopyTo(array, 0);

                    result = array;
                }
                else
                {
                    result = Activator.CreateInstance(targetMethod.ReturnType);

                    foreach (var item in arr)
                    {
                        var addMethod = result!.GetType().GetMethod("Add");

                        addMethod?.Invoke(result, [ item ]);
                    }
                }
            }
            else
            {
                result = scriptResult;
            }
        }

        return result;
    }

    /// <summary>
    /// Reads the parameters of the script block.
    /// </summary>
    /// <returns>A dictionary containing the parameter names and their types.</returns>
    public Dictionary<string, Type> ReadParameters()
    {
        var parameters = new Dictionary<string, Type>();

        var ast = _scriptBlockExecutor.Ast;

        if (ast is not null && ast.ParamBlock is not null)
        {
            foreach (var param in ast.ParamBlock.Parameters)
            {
                parameters[param.Name.VariablePath.UserPath] = param.StaticType;
            }
        }

        return parameters;
    }

    /// <summary>
    /// Determines whether the script block has a return type.
    /// </summary>
    /// <returns><c>true</c> if the script block has a return type; otherwise, <c>false</c>.</returns>
    public bool HasReturnType()
    {
        var ast = _scriptBlockExecutor.Ast;

        if (ast is not null && ast.EndBlock is not null)
        {
            foreach (var statement in ast.EndBlock.Statements)
            {
                if (statement is ReturnStatementAst returnStatement && returnStatement.Pipeline != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
