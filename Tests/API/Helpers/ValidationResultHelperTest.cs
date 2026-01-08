using At.luki0606.DartZone.API.Helpers;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;
using FluentValidation.Results;

namespace At.luki0606.DartZone.Tests.API.Helpers;

[TestFixture]
internal sealed class ValidationResultHelperTest
{
    [Test]
    public void GetFirstErrorMessage_ValidationResultIsNull_SHouldReturnGenericErrorMessage()
    {
        ValidationResult validationResult = null;
        MessageResponseDto errorMessage = ValidationResultHelper.GetFirstErrorMessage(validationResult);
        errorMessage.Message.Should().Be("Validation failed unintentionally.");
    }

    [Test]
    public void GetFirstErrorMessage_ValidationResultHasNoErros_SHouldReturnGenericErrorMessage()
    {
        ValidationResult validationResult = new();
        MessageResponseDto errorMessage = ValidationResultHelper.GetFirstErrorMessage(validationResult);
        errorMessage.Message.Should().Be("Validation failed unintentionally.");
    }

    [Test]
    public void GetFirstErrorMessage_ValidationResultHasErrors_SHouldReturnFirstErrorMessage()
    {
        ValidationResult validationResult = new()
        {
            Errors =
            [
                new ValidationFailure("Field1", "Error message 1"),
                new ValidationFailure("Field2", "Error message 2")
            ]
        };
        MessageResponseDto errorMessage = ValidationResultHelper.GetFirstErrorMessage(validationResult);
        errorMessage.Message.Should().Be("Error message 1");
    }
}
