using System;
using At.luki0606.DartZone.Shared.Results;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Results
{
    [TestFixture]
    public class Result_T_Test
    {
        [Test]
        public void Success_ShouldSetIsSuccessTrue_AndStoreValue()
        {
            int value = 42;
            Result<int> result = Result<int>.Success(value);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(42);
            result.Error.Should().BeEmpty();
        }

        [Test]
        public void Failure_ShouldSetIsSuccessFalse_AndStoreError()
        {
            string error = "Something went wrong";
            Result<int> result = Result<int>.Failure(error);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        [Test]
        public void Failure_ShouldNotAllowAccessingValue()
        {
            Result<string> result = Result<string>.Failure("Error");

            Action act = () =>
            {
                var _ = result.Value;
            };

            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ImplicitConversion_ToValue_ShouldReturnValue_WhenSuccess()
        {
            Result<int> result = Result<int>.Success(123);

            int value = result;
            value.Should().Be(123);
        }

        [Test]
        public void ImplicitConversion_ToValue_ShouldThrow_WhenFailure()
        {
            Result<int> result = Result<int>.Failure("fail");

            Action act = () =>
            {
                int _ = result;
            };

            act.Should().Throw<InvalidOperationException>();
        }
    }
}
