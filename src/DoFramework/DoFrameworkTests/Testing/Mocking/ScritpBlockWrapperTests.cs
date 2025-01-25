using AutoFixture.Xunit2;
using DoFramework.Testing;
using FluentAssertions;
using Moq;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Reflection;
using System.Text;

namespace DoFrameworkTests.Testing;

public class ScritpBlockWrapperTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ReadsParametersSuccessfully(Dictionary<string, Type> parameters)
    {
        // Arrange
        var sb = new StringBuilder();

        var i = 0;

        foreach (var key in parameters.Keys)
        {
            sb.AppendLine($"[{parameters[key].FullName}] ${key.Replace("-", string.Empty)}{(i < parameters.Count - 1 ? "," : string.Empty)}");
            i++;
        }

        var block = ScriptBlock.Create($@"
            param (
                {sb}
            )
        ");

        var sut = new ScriptBlockWrapper(new ScriptBlockExecutor(block));

        // Act
        var result = sut.ReadParameters();

        // Assert
        result.Keys.Count.Should().Be(parameters.Count);

        foreach (var key in parameters.Keys)
        {
            result[key.Replace("-", string.Empty)].Should().Be(parameters[key]);
        }
    }

    [Fact]
    public void ReadsParametersSuccessfullyWhenEmpty()
    {
        // Arrange
        var block = ScriptBlock.Create(string.Empty);

        var sut = new ScriptBlockWrapper(new ScriptBlockExecutor(block));

        // Act
        var result = sut.ReadParameters();

        // Assert
        result.Keys.Count.Should().Be(0);
    }

    [Theory]
    [InlineAutoMoqData]
    public void InvokeVoidReturnsNull(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        [Frozen] Mock<MethodInfo> targetMethod)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(null!);

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineAutoMoqData]
    public void InvokeVoidReturnsNullWithParams(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        Dictionary<string, Type> parameters,
        [Frozen] Mock<MethodInfo> targetMethod)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(null!);

        var sb = new StringBuilder();

        var i = 0;

        foreach (var key in parameters.Keys)
        {
            sb.AppendLine($"[{parameters[key].FullName}] ${key.Replace("-", string.Empty)}{(i < parameters.Count - 1 ? "," : string.Empty)}");
            i++;
        }

        var block = ScriptBlock.Create($@"
            param (
                {sb}
            )
        ");

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().BeNull();
    }


    [Theory]
    [InlineAutoMoqData]
    public void InvokeVoidReturnsObj(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        object returnObject,
        [Frozen] Mock<MethodInfo> targetMethod)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(returnObject);

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(returnObject);
    }

    [Theory]
    [InlineAutoMoqData]
    public void InvokeVoidReturnsObjWithParams(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        Dictionary<string, Type> parameters,
        object returnObject,
        [Frozen] Mock<MethodInfo> targetMethod)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(returnObject);

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        var sb = new StringBuilder();

        var i = 0;

        foreach (var key in parameters.Keys)
        {
            sb.AppendLine($"[{parameters[key].FullName}] ${key.Replace("-", string.Empty)}{(i < parameters.Count - 1 ? "," : string.Empty)}");
            i++;
        }

        var block = ScriptBlock.Create($@"
            param (
                {sb}
            )
        ");

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(returnObject);
    }


    [Theory]
    [InlineAutoMoqData]
    public void InvokeReturnsArray(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        [Frozen] Mock<MethodInfo> targetMethod,
        object[] returnObject)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(returnObject);
        targetMethod.Setup(x => x.ReturnType).Returns(returnObject.GetType());

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(returnObject.GetType());
        result.Should().BeEquivalentTo(returnObject);
    }

    [Theory]
    [InlineAutoMoqData]
    public void InvokeReturnsList(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        [Frozen] Mock<MethodInfo> targetMethod,
        List<object> returnObject)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(returnObject);
        targetMethod.Setup(x => x.ReturnType).Returns(returnObject.GetType());

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(returnObject.GetType());
        result.Should().BeEquivalentTo(returnObject);
    }

    [Theory]
    [InlineAutoMoqData]
    public void InvokeReturnsDictionary(
        [Frozen] Mock<IScriptBlockExecutor> scriptBlockExecMock,
        [Frozen] Mock<MethodInfo> targetMethod,
        Dictionary<object, object> returnObject)
    {
        // Arrange
        scriptBlockExecMock.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(returnObject);

        var sut = new ScriptBlockWrapper(scriptBlockExecMock.Object);

        // Act
        var result = sut.Invoke(targetMethod.Object);

        // Assert
        result.Should().NotBeNull();
        result!.GetType().Should().Be(returnObject.GetType());
        result.Should().Be(returnObject);
    }

    [Fact]
    public void HasReturnTypeIsFalse()
    {
        // Arrange
        var block = ScriptBlock.Create(string.Empty);

        var sut = new ScriptBlockWrapper(new ScriptBlockExecutor(block));

        // Act
        var result = sut.HasReturnType();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasReturnTypeIsTrue()
    {
        // Arrange
        var block = ScriptBlock.Create("return 0;");

        var sut = new ScriptBlockWrapper(new ScriptBlockExecutor(block));

        // Act
        var result = sut.HasReturnType();

        // Assert
        result.Should().BeTrue();
    }
}