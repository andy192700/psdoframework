namespace DoFramework.Testing;

/// <summary>
/// Represents the base class for proxy results.
/// </summary>
public abstract class ProxyResult
{
    /// <summary>
    /// Gets the proxy used in the operation.
    /// </summary>
    public abstract IProxy Proxy { get; }

    /// <summary>
    /// Gets or sets the instance associated with the proxy result.
    /// </summary>
    public object? Instance { get; set; }
}

/// <summary>
/// Represents the result of a proxy operation with a specific proxy type.
/// </summary>
/// <typeparam name="TProxy">The type of the proxy.</typeparam>
public class ProxyResult<TProxy> : ProxyResult, IProxyResult<TProxy> where TProxy : class
{
    /// <summary>
    /// Gets the proxy used in the operation.
    /// </summary>
    public override IProxy Proxy { get; }

    /// <summary>
    /// Gets the instance of the proxy type.
    /// </summary>
    public new virtual TProxy Instance { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyResult{TProxy}"/> class with the specified proxy object.
    /// </summary>
    /// <param name="proxy">The proxy object.</param>
    public ProxyResult(object proxy)
    {
        Proxy = (IProxy)proxy;
        Instance = (TProxy)proxy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxyResult{TProxy}"/> class with the specified proxy and instance.
    /// </summary>
    /// <param name="proxy">The proxy to use.</param>
    /// <param name="instance">The instance of the proxy type.</param>
    public ProxyResult(IProxy proxy, TProxy instance)
    {
        Proxy = proxy;
        Instance = instance;
    }
}
