using AutoFixture.Xunit2;
using DoFramework.Processing;
using DoFramework.Types;
using DoFramework.Validators;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Types;

public class LookupProcessTypeTests
{
    [Theory]
    [InlineAutoMoqData]
    public void TypeDoesNotExist(
        [Frozen] Mock<TypeValidator<IProcess>> validator, 
        [Frozen] Mock<IValidationErrorWriter> validationErrorWriter,
        string typeName)
    {
        // Arrange
        var sut = new LookupProcessType(validator.Object, validationErrorWriter.Object);

        // Act
        var func = () => sut.Lookup(typeName);

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Could not find the process class {typeName}");
    }

    [Theory]
    [InlineAutoMoqData]
    public void TypeDoesExistButNotValid(
        [Frozen] Mock<TypeValidator<IProcess>> validator,
        [Frozen] Mock<IValidationErrorWriter> validationErrorWriter,
        List<string> errors)
    {
        // Arrange
        var validationResult = new ValidationResult(errors);

        validator.Setup(x => x.Validate(It.IsAny<Type>())).Returns(validationResult);

        var sut = new LookupProcessType(validator.Object, validationErrorWriter.Object);

        // Act
        var func = () => sut.Lookup(typeof(ExampleProcess).FullName!);

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Process Type is invalid {typeof(ExampleProcess).FullName!}");

        validationErrorWriter.Verify(x => x.Write(validationResult), Times.Once());
    }

    [Theory]
    [InlineAutoMoqData]
    public void TypeIsValid(
        [Frozen] Mock<TypeValidator<IProcess>> validator,
        [Frozen] Mock<IValidationErrorWriter> validationErrorWriter)
    {
        // Arrange
        validator.Setup(x => x.Validate(It.IsAny<Type>())).Returns(new ValidationResult([]));

        var sut = new LookupProcessType(validator.Object, validationErrorWriter.Object);

        // Act
        var result = sut.Lookup(typeof(ExampleProcess).FullName!);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(typeof(ExampleProcess));
    }
}

public class ExampleProcess : IProcess
{
    public void Run()
    {
        throw new NotImplementedException();
    }

    public bool Validate()
    {
        throw new NotImplementedException();
    }
}