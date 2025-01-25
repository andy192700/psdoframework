using DoFramework.Testing;
using FluentAssertions;

namespace DoFrameworkTests.Testing;

public class ProxyFactoryTests
{
    [Fact]
    public void CanCreateInterfaceProxy()
    {
        // Arrange / Act
        var result = ProxyFactory.CreateProxy(typeof(ITestInterface));

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().BeAssignableTo(typeof(IProxyResult<ITestInterface>));
    }

    [Fact]
    public void CanCreateInterfaceProxyWithGenerics()
    {
        // Arrange / Act
        var result = ProxyFactory.CreateProxy<ITestInterface>();

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().BeAssignableTo(typeof(IProxyResult<ITestInterface>));
    }

    [Theory]
    [InlineAutoMoqData]
    public void CanCreateClassProxy(Proxy proxy, TestClass instance)
    {
        // Arrange / Act
        var result = ProxyFactory.CreateClassProxy(proxy, (object)instance);

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().BeAssignableTo(typeof(IProxyResult<object>));
    }

    [Theory]
    [InlineAutoMoqData]
    public void CanCreateClassProxyWithGenerics(Proxy proxy, TestClass testClass)
    {
        // Arrange / Act
        var result = ProxyFactory.CreateClassProxy(proxy, testClass);

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().BeAssignableTo(typeof(IProxyResult<TestClass>));
    }
}

public interface ITestInterface { }

public class TestClass { }