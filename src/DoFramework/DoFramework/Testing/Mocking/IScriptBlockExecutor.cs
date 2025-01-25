using System.Management.Automation.Language;

namespace DoFramework.Testing;

/// <summary>
/// Defines an executor for script blocks, providing access to the script block's AST and invocation capabilities.
/// </summary>
public interface IScriptBlockExecutor
{
    /// <summary>
    /// Gets the abstract syntax tree (AST) of the script block.
    /// </summary>
    ScriptBlockAst Ast { get; }

    /// <summary>
    /// Invokes the script block with the specified arguments.
    /// </summary>
    /// <param name="args">The arguments to pass to the script block.</param>
    /// <returns>The result of the script block invocation.</returns>
    object Invoke(params object[] args);
}
