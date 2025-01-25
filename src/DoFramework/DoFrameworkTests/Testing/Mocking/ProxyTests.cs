using AutoFixture.Xunit2;
using DoFramework.Testing;
using FluentAssertions;
using Moq;
using System.Reflection;

namespace DoFrameworkTests.Testing;

public class ProxyTests
{
    [Theory]
    [InlineAutoMoqData]
    public void MockMethod_AddsToMockMethodsList(
        TestableProxy sut,
        string methodName,
        [Frozen] Mock<IScriptBlockWrapper> scriptBlockWrapperMock,
        Dictionary<string, Type> givenParameters)
    {
        // Arrange
        scriptBlockWrapperMock.Setup(x => x.ReadParameters()).Returns(givenParameters);

        // Act
        sut.MockMethod(methodName, scriptBlockWrapperMock.Object);

        // Assert
        sut.MockedMethods.Should().ContainSingle().Which.Item1.Should().Be(methodName);
        sut.MockedMethods.Should().ContainSingle().Which.Item2.Should().Be(scriptBlockWrapperMock.Object);
    }

    [Theory]
    [InlineAutoMoqData]
    public void MockMethod_ThrowsExceptionIfMethodAlreadyMocked(
        TestableProxy sut,
        string methodName,
        [Frozen] Mock<IScriptBlockWrapper> scriptBlockWrapperMock,
        Dictionary<string, Type> givenParameters)
    {
        // Arrange
        scriptBlockWrapperMock.Setup(x => x.ReadParameters()).Returns(givenParameters);

        sut.MockedMethods.Add((methodName, scriptBlockWrapperMock.Object));

        // Act & Assert
        Action action = () => sut.MockMethod(methodName, scriptBlockWrapperMock.Object);

        action.Should().Throw<Exception>().WithMessage($"Method '{methodName}' has already been mocked with these parameters.");

        sut.MockedMethods.Should().ContainSingle().Which.Item1.Should().Be(methodName);
        sut.MockedMethods.Should().ContainSingle().Which.Item2.Should().Be(scriptBlockWrapperMock.Object);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodMocked(
        TestableProxy sut,
        [Frozen] Mock<IScriptBlockWrapper> scriptBlockWrapperMock,
        [Frozen] Mock<MethodInfo> targetMethod,
        object?[] args,
        Type type)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(type);

        var mockParams = new List<ParameterInfo>();

        var expectedArgs = new Dictionary<string, object>();
        var scriptBlockExpectedArgs = new Dictionary<string, Type>();

        for (int i = 0; i < args.Length; i++)
        {
            var parameterMock = new Mock<ParameterInfo>();

            parameterMock.Setup(x => x.Name).Returns($"MockParam_{i}");

            mockParams.Add(parameterMock.Object);

            expectedArgs.Add(parameterMock.Object.Name!, args[i]!);
            scriptBlockExpectedArgs.Add(parameterMock.Object.Name!, args[i]!.GetType());
        }

        targetMethod.Setup(x => x.GetParameters()).Returns([.. mockParams]);

        scriptBlockWrapperMock.Setup(x => x.ReadParameters()).Returns(scriptBlockExpectedArgs);

        scriptBlockWrapperMock.Setup(x => x.Invoke(It.IsAny<MethodInfo>(), It.IsAny<object[]>())).Returns(Activator.CreateInstance(type)!);

        sut.MockMethod(methodName, scriptBlockWrapperMock.Object);

        // Act
        var result = sut.InvokeMock(targetMethod.Object, args);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(type);

        scriptBlockWrapperMock.Verify(x => x.Invoke(targetMethod.Object, args!), Times.Once);

        sut.CountCalls(methodName, expectedArgs).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodMockedEmptyParams(
        TestableProxy sut,
        [Frozen] Mock<IScriptBlockWrapper> scriptBlockWrapperMock,
        [Frozen] Mock<MethodInfo> targetMethod,
        Type type)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(type);

        targetMethod.Setup(x => x.GetParameters()).Returns([]);

        scriptBlockWrapperMock.Setup(x => x.ReadParameters()).Returns([]);

        scriptBlockWrapperMock.Setup(x => x.Invoke(It.IsAny<MethodInfo>(), It.IsAny<object[]>())).Returns(new object());

        sut.MockMethod(methodName, scriptBlockWrapperMock.Object);

        var args = Array.Empty<object>();

        // Act
        var result = sut.InvokeMock(targetMethod.Object, args);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(type);

        scriptBlockWrapperMock.Verify(x => x.Invoke(targetMethod.Object, args!), Times.Once);

        sut.CountCalls(methodName).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodMockedIsVoid(
        TestableProxy sut,
        [Frozen] Mock<IScriptBlockWrapper> scriptBlockWrapperMock,
        [Frozen] Mock<MethodInfo> targetMethod,
        object?[] args)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(typeof(void));

        var mockParams = new List<ParameterInfo>();

        var expectedArgs = new Dictionary<string, object>();
        var scriptBlockExpectedArgs = new Dictionary<string, Type>();

        for (int i = 0; i < args.Length; i++)
        {
            var parameterMock = new Mock<ParameterInfo>();

            parameterMock.Setup(x => x.Name).Returns($"MockParam_{i}");

            mockParams.Add(parameterMock.Object);

            expectedArgs.Add(parameterMock.Object.Name!, args[i]!);
            scriptBlockExpectedArgs.Add(parameterMock.Object.Name!, args[i]!.GetType());
        }

        targetMethod.Setup(x => x.GetParameters()).Returns([.. mockParams]);

        scriptBlockWrapperMock.Setup(x => x.ReadParameters()).Returns(scriptBlockExpectedArgs);

        sut.MockMethod(methodName, scriptBlockWrapperMock.Object);

        // Act
        var result = sut.InvokeMock(targetMethod.Object, args);

        // Assert
        result.Should().BeNull();

        scriptBlockWrapperMock.Verify(x => x.Invoke(targetMethod.Object, args!), Times.Once);

        sut.CountCalls(methodName, expectedArgs).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodMockedIsVoidEmptyParams(
        TestableProxy sut,
        [Frozen] Mock<IScriptBlockWrapper> scriptBlockWrapperMock,
        [Frozen] Mock<MethodInfo> targetMethod)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(typeof(void));

        targetMethod.Setup(x => x.GetParameters()).Returns([]);

        scriptBlockWrapperMock.Setup(x => x.ReadParameters()).Returns([]);

        sut.MockMethod(methodName, scriptBlockWrapperMock.Object);

        var args = Array.Empty<object>();

        // Act
        var result = sut.InvokeMock(targetMethod.Object, args);

        // Assert
        result.Should().BeNull();

        scriptBlockWrapperMock.Verify(x => x.Invoke(targetMethod.Object, args!), Times.Once);

        sut.CountCalls(methodName).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodNotMocked(
        TestableProxy sut,
        [Frozen] Mock<MethodInfo> targetMethod,
        object?[] args,
        Type type)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(type);

        var mockParams = new List<ParameterInfo>();

        var expectedArgs = new Dictionary<string, object>();

        for (int i = 0; i < args.Length; i++)
        {
            var parameterMock = new Mock<ParameterInfo>();

            parameterMock.Setup(x => x.Name).Returns($"MockParam_{i}");

            mockParams.Add(parameterMock.Object);

            expectedArgs.Add(parameterMock.Object.Name!, args[i]!);
        }

        targetMethod.Setup(x => x.GetParameters()).Returns([.. mockParams]);

        // Act
        var result = sut.InvokeMock(targetMethod.Object, args);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(type);

        sut.CountCalls(methodName, expectedArgs).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodNotMockedEmptyParams(
        TestableProxy sut,
        [Frozen] Mock<MethodInfo> targetMethod,
        Type type)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(type);

        targetMethod.Setup(x => x.GetParameters()).Returns([]);

        // Act
        var result = sut.InvokeMock(targetMethod.Object, []);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(type);

        sut.CountCalls(methodName).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodNotMockedIsVoid(
        TestableProxy sut,
        [Frozen] Mock<MethodInfo> targetMethod,
        object?[] args)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(typeof(void));

        var mockParams = new List<ParameterInfo>();

        var expectedArgs = new Dictionary<string, object>();

        for (int i = 0; i < args.Length; i++)
        {
            var parameterMock = new Mock<ParameterInfo>();

            parameterMock.Setup(x => x.Name).Returns($"MockParam_{i}");

            mockParams.Add(parameterMock.Object);

            expectedArgs.Add(parameterMock.Object.Name!, args[i]!);
        }

        targetMethod.Setup(x => x.GetParameters()).Returns([.. mockParams]);

        // Act
        var result = sut.InvokeMock(targetMethod.Object, args);

        // Assert
        result.Should().BeNull();

        sut.CountCalls(methodName, expectedArgs).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void Invoke_CallsRegisteredIfMethodNotMockedIsVoidEmptyParams(
        TestableProxy sut,
        [Frozen] Mock<MethodInfo> targetMethod)
    {
        // Arrange
        string methodName = targetMethod.Name;

        targetMethod.Setup(x => x.Name).Returns(methodName);

        targetMethod.Setup(x => x.ReturnType).Returns(typeof(void));

        targetMethod.Setup(x => x.GetParameters()).Returns([]);

        // Act
        var result = sut.InvokeMock(targetMethod.Object, []);

        // Assert
        result.Should().BeNull();

        sut.CountCalls(methodName).Should().Be(1);

        sut.CountCalls(methodName).Should().Be(1);
    }
}

public class TestableProxy : Proxy
{
    public List<(string, IScriptBlockWrapper)> MockedMethods => MockMethods;

    public object? InvokeMock(MethodInfo? targetMethod, object?[]? args)
    {
        return base.Invoke(targetMethod, args);
    }
}