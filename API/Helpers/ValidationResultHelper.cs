using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentValidation.Results;

namespace At.luki0606.DartZone.API.Helpers;

internal static class ValidationResultHelper
{
    public static MessageResponseDto GetFirstErrorMessage(ValidationResult validationResult)
    {
        if (validationResult != null && validationResult.Errors.Count > 0)
        {
            ValidationFailure firstError = validationResult.Errors[0];
            return new MessageResponseDto
            {
                Message = firstError.ErrorMessage
            };
        }
        else
        {
            return new MessageResponseDto
            {
                Message = "Validation failed unintentionally."
            };
        }
    }
}
