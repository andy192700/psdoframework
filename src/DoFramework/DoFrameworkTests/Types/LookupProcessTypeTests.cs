using AutoFixture.Xunit2;
using DoFramework.Processing;
using DoFramework.Types;
using DoFramework.Validators;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Types;

public class LookupComposerTypeTests
{
    [Theory]
    [InlineAutoMoqData]
    public void TypeDoesNotExist(
        [Frozen] Mock<TypeValidator<IComposer>> validator, 
        [Frozen] Mock<IValidationErrorWriter> validationErrorWriter,
        string typeName)
    {
        // Arrange
        var sut = new LookupComposerType(validator.Object, validationErrorWriter.Object);

        // Act
        var func = () => sut.Lookup(typeName);

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Could not find the {typeof(IComposer).Name} class {typeName}");
    }

    [Theory]
    [InlineAutoMoqData]
    public void TypeDoesExistButNotValid(
        [Frozen] Mock<TypeValidator<IComposer>> validator,
        [Frozen] Mock<IValidationErrorWriter> validationErrorWriter,
        List<string> errors)
    {
        // Arrange
        var validationResult = new ValidationResult(errors);

        validator.Setup(x => x.Validate(It.IsAny<Type>())).Returns(validationResult);

        var sut = new LookupComposerType(validator.Object, validationErrorWriter.Object);

        // Act
        var func = () => sut.Lookup(typeof(ExampleComposer).FullName!);

        // Assert
        
        func.Should().Throw<Exception>().WithMessage($"{typeof(IComposer).Name} Type is invalid {typeof(ExampleComposer).FullName!}");

        validationErrorWriter.Verify(x => x.Write(validationResult), Times.Once());
    }

    [Theory]
    [InlineAutoMoqData]
    public void TypeIsValid(
        [Frozen] Mock<TypeValidator<IComposer>> validator,
        [Frozen] Mock<IValidationErrorWriter> validationErrorWriter)
    {
        // Arrange
        validator.Setup(x => x.Validate(It.IsAny<Type>())).Returns(new ValidationResult([]));

        var sut = new LookupComposerType(validator.Object, validationErrorWriter.Object);

        // Act
        var result = sut.Lookup(typeof(ExampleComposer).FullName!);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(typeof(ExampleComposer));
    }
}

public class ExampleComposer : IComposer
{
    public void Compose(IComposerWorkBench workBench)
    {
        throw new NotImplementedException();
    }
}