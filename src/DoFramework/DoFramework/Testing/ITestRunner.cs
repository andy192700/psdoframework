using DoFramework.Domain;

namespace DoFramework.Testing;

/// <summary>
/// Defines a runner for executing tests with a specified filter.
/// </summary>
public interface ITestRunner
{
    /// <summary>
    /// Runs tests that match the specified filter.
    /// </summary>
    /// <param name="filter">The filter to apply when running tests.</param>
    void Test(string filter);
}

/// <summary>
/// Defines a runner for executing tests with a specified descriptor and filter.
/// </summary>
/// <typeparam name="TDescriptor">The type of the descriptor.</typeparam>
public interface ITestRunner<TDescriptor> : ITestRunner where TDescriptor : IDescriptor { }
