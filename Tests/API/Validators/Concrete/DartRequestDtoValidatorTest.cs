using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;
using FluentValidation.Results;

namespace At.luki0606.DartZone.Tests.API.Validators.Concrete
{
    [TestFixture]
    public class DartRequestDtoValidatorTest
    {
        private DartRequestDtoValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new DartRequestDtoValidator();
        }

        [Test]
        public void Validate_ValidRequest_ReturnsTrue()
        {
            DartRequestDto dartDto = new()
            {
                Multiplier = (Multiplier)1,
                Field = 10,
            };

            ValidationResult result = _validator.Validate(dartDto);
            result.IsValid.Should().BeTrue();
        }

        [TestCase(0)]
        [TestCase(4)]
        public void Validate_InvalidMultiplier_ReturnsFalse(int multiplier)
        {
            DartRequestDto dartDto = new()
            {
                Multiplier = (Multiplier)multiplier,
                Field = 10,
            };
            ValidationResult result = _validator.Validate(dartDto);
            result.IsValid.Should().BeFalse();
        }

        [TestCase(21)]
        [TestCase(24)]
        [TestCase(26)]
        [TestCase(-1)]
        public void Validate_InvalidField_ReturnsFalse(int field)
        {
            DartRequestDto dartDto = new()
            {
                Multiplier = (Multiplier)1,
                Field = field,
            };
            ValidationResult result = _validator.Validate(dartDto);
            result.IsValid.Should().BeFalse();
        }
    }
}
