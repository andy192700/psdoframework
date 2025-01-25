using System.Linq.Expressions;
using System.Reflection;

namespace DoFramework.Testing;

/// <summary>
/// Represents a factory for creating proxy instances.
/// </summary>
public class ProxyFactory
{
    /// <summary>
    /// Creates a proxy for the specified type.
    /// </summary>
    /// <param name="type">The type for which to create a proxy.</param>
    /// <returns>An object representing the created proxy.</returns>
    public static object CreateProxy(Type type)
    {
        var factory = new ProxyFactory();

        return factory.CreateProxyForType(type);
    }

    /// <summary>
    /// Creates a class proxy with the specified proxy and instance.
    /// </summary>
    /// <param name="proxy">The proxy to use.</param>
    /// <param name="instance">The instance to proxy.</param>
    /// <returns>An object representing the created class proxy.</returns>
    public static object CreateClassProxy(IProxy proxy, object instance)
    {
        return new ProxyResult<object>(proxy, instance);
    }

    /// <summary>
    /// Creates a proxy for the specified type parameter.
    /// </summary>
    /// <typeparam name="TProxy">The type of the proxy.</typeparam>
    /// <returns>An instance of <see cref="IProxyResult{TProxy}"/> representing the created proxy.</returns>
    public static IProxyResult<TProxy> CreateProxy<TProxy>() where TProxy : class
    {
        var factory = new ProxyFactory();

        return factory.CreateProxyResult<TProxy>();
    }

    /// <summary>
    /// Creates a class proxy for the specified type parameter with the given proxy and instance.
    /// </summary>
    /// <typeparam name="TProxy">The type of the proxy.</typeparam>
    /// <param name="proxy">The proxy to use.</param>
    /// <param name="instance">The instance to proxy.</param>
    /// <returns>An instance of <see cref="IProxyResult{TProxy}"/> representing the created class proxy.</returns>
    public static IProxyResult<TProxy> CreateClassProxy<TProxy>(IProxy proxy, TProxy instance) where TProxy : class
    {
        return new ProxyResult<TProxy>(proxy, instance);
    }

    /// <summary>
    /// Creates a proxy for the specified type.
    /// </summary>
    /// <param name="type">The type for which to create a proxy.</param>
    /// <returns>An object representing the created proxy.</returns>
    private object CreateProxyForType(Type type)
    {
        var method = typeof(ProxyFactory).GetMethod("CreateProxyResult", BindingFlags.Instance | BindingFlags.NonPublic);

        var genericMethod = method?.MakeGenericMethod(type);

        var lambda = Expression.Lambda(
            Expression.Call(Expression.Constant(this), genericMethod!),
            Expression.Parameter(typeof(object), "instance"));

        var compiledLambda = lambda.Compile();

        return compiledLambda.DynamicInvoke(type)!;
    }

    /// <summary>
    /// Creates a proxy result for the specified type parameter.
    /// </summary>
    /// <typeparam name="TProxy">The type of the proxy.</typeparam>
    /// <returns>An instance of <see cref="IProxyResult{TProxy}"/> representing the created proxy result.</returns>
    private IProxyResult<TProxy> CreateProxyResult<TProxy>() where TProxy : class
    {
        var instance = DispatchProxy.Create<TProxy, Proxy>();

        return new ProxyResult<TProxy>(instance);
    }
}
