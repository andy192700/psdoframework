namespace DoFramework.Processing;

/// <summary>
/// Represents an interface for verifying context.
/// </summary>
public interface IContextVerifier
{
    /// <summary>
    /// Verifies the context.
    /// </summary>
    /// <returns><c>true</c> if the context is valid; otherwise, <c>false</c>.</returns>
    bool Verify();

    /// <summary>
    /// Confirms that a specific composer invoked processing.
    /// </summary>
    /// <param name="composerName">The name of the composer.</param>
    /// <returns>The context verifier.</returns>
    IContextVerifier ComposedBy(string composerName);

    /// <summary>
    /// Confirms the presence of a specific key in the context.
    /// </summary>
    /// <param name="key">The key to confirm.</param>
    /// <returns>The context verifier.</returns>
    IContextVerifier ConfirmKey(string key);

    /// <summary>
    /// Confirms that a process succeeded in the context.
    /// </summary>
    /// <param name="processName">The name of the process.</param>
    /// <returns>The context verifier.</returns>
    IContextVerifier ProcessSucceeded(string processName);
}
