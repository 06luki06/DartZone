using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using FluentAssertions;
using FluentValidation.Results;

namespace At.luki0606.DartZone.Tests.API.Validators.Concrete;

[TestFixture]
internal sealed class UserRequestDtoValidatorTest
{
    private UserRequestDtoValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UserRequestDtoValidator();
    }

    [Test]
    public void Validate_ValidUserRequestDto_ReturnsSuccess()
    {
        UserRequestDto userRequestDto = new()
        {
            Username = "validUser",
            Password = "VeryStrongPassword123!",
        };

        ValidationResult result = _validator.Validate(userRequestDto);
        result.IsValid.Should().BeTrue();
    }

    [TestCase("", "Username is required.")]
    [TestCase("us", "Username must be between 3 and 20 characters long.")]
    [TestCase("ThisIsAVeryLongUsername", "Username must be between 3 and 20 characters long.")]
    [TestCase("user@name", "Username can only contain letters, numbers, and underscores.")]
    public void Validate_InvalidUsername_ReturnsFailure(string username, string errormessage)
    {
        UserRequestDto userRequestDto = new()
        {
            Username = username,
            Password = "VeryStrongPassword123!",
        };

        ValidationResult result = _validator.Validate(userRequestDto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(errormessage, System.StringComparison.Ordinal));
    }

    [TestCase("", "Password is required.")]
    [TestCase("weakpass", "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
    public void Validate_InvalidPassword_ReturnsFailure(string password, string errorMessage)
    {
        UserRequestDto userRequestDto = new()
        {
            Username = "validUser",
            Password = password,
        };

        ValidationResult result = _validator.Validate(userRequestDto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(errorMessage, System.StringComparison.Ordinal));
    }
}
