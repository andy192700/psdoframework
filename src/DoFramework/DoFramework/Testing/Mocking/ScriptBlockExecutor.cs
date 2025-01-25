using System.Management.Automation.Language;
using System.Management.Automation;

namespace DoFramework.Testing;

/// <summary>
/// Represents an executor for a PowerShell script block, providing access to its AST and invocation capabilities.
/// </summary>
public class ScriptBlockExecutor : IScriptBlockExecutor
{
    /// <summary>
    /// Gets the abstract syntax tree (AST) of the script block.
    /// </summary>
    public ScriptBlockAst Ast => (ScriptBlockAst)_scriptBlock.Ast;

    private readonly ScriptBlock _scriptBlock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptBlockExecutor"/> class with the specified script block.
    /// </summary>
    /// <param name="scriptBlock">The script block to be executed.</param>
    public ScriptBlockExecutor(ScriptBlock scriptBlock)
    {
        _scriptBlock = scriptBlock;
    }

    /// <summary>
    /// Invokes the script block with the specified arguments.
    /// </summary>
    /// <param name="args">The arguments to pass to the script block.</param>
    /// <returns>The result of the script block invocation.</returns>
    public object Invoke(params object[] args)
    {
        var obj = _scriptBlock.InvokeReturnAsIs(args);

        if (obj is PSObject psobj)
        {
            return psobj.BaseObject;
        }

        return obj;
    }
}
