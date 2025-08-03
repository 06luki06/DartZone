using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Enums;
using FluentAssertions;
using FluentValidation.Results;

namespace At.luki0606.DartZone.Tests.API.Validators.Concrete
{
    [TestFixture]
    public class ThrowRequestDtoValidatorTest
    {
        private ThrowRequestDtoValidator _validator;
        private DartRequestDto _dart1;
        private DartRequestDto _dart2;
        private DartRequestDto _dart3;

        [SetUp]
        public void Setup()
        {
            _validator = new ThrowRequestDtoValidator();
            _dart1 = new()
            {
                Multiplier = Multiplier.Single,
                Field = 20
            };

            _dart2 = new()
            {
                Multiplier = Multiplier.Double,
                Field = 5
            };

            _dart3 = new()
            {
                Multiplier = Multiplier.Triple,
                Field = 15
            };
        }

        [Test]
        public void Validate_ValidThrowRequestDto_ReturnsTrue()
        {
            ThrowRequestDto throwRequestDto = new()
            {
                Dart1 = _dart1,
                Dart2 = _dart2,
                Dart3 = _dart3
            };

            ValidationResult result = _validator.Validate(throwRequestDto);
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_NullDart1_ReturnsFalse()
        {
            ThrowRequestDto throwRequestDto = new()
            {
                Dart1 = null,
                Dart2 = _dart2,
                Dart3 = _dart3
            };
            ValidationResult result = _validator.Validate(throwRequestDto);
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void Validate_NullDart2_ReturnsFalse()
        {
            ThrowRequestDto throwRequestDto = new()
            {
                Dart1 = _dart1,
                Dart2 = null,
                Dart3 = _dart3
            };
            ValidationResult result = _validator.Validate(throwRequestDto);
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void Validate_NullDart3_ReturnsFalse()
        {
            ThrowRequestDto throwRequestDto = new()
            {
                Dart1 = _dart1,
                Dart2 = _dart2,
                Dart3 = null
            };
            ValidationResult result = _validator.Validate(throwRequestDto);
            result.IsValid.Should().BeFalse();
        }

        [Test]
        public void Validate_InvalidDart1_ReturnsFalse()
        {
            DartRequestDto invalidDart1 = new()
            {
                Multiplier = Multiplier.Single,
                Field = -1
            };
            ThrowRequestDto throwRequestDto = new()
            {
                Dart1 = invalidDart1,
                Dart2 = _dart2,
                Dart3 = _dart3
            };
            ValidationResult result = _validator.Validate(throwRequestDto);
            result.IsValid.Should().BeFalse();
        }
    }
}
