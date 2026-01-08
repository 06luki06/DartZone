using At.luki0606.DartZone.Shared.Results;
using FluentAssertions;

namespace At.luki0606.DartZone.Tests.Shared.Results;

[TestFixture]
internal sealed class ResultTests
{
    [Test]
    public void Success_ShouldSetIsSuccessTrue()
    {
        Result result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeEmpty();
    }

    [Test]
    public void Failure_ShouldSetIsSuccessFalse_AndContainError()
    {
        Result result = Result.Failure("Oops");

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Oops");
    }
}
