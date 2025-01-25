namespace DoFramework.Testing;

/// <summary>
/// Represents the result of a proxy operation with a specific proxy type.
/// </summary>
/// <typeparam name="TProxy">The type of the proxy.</typeparam>
public interface IProxyResult<TProxy> where TProxy : class
{
    /// <summary>
    /// Gets the proxy used in the operation.
    /// </summary>
    IProxy Proxy { get; }

    /// <summary>
    /// Gets the instance of the proxy type.
    /// </summary>
    TProxy Instance { get; }
}
