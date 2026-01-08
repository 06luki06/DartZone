namespace At.luki0606.DartZone.Shared.Results;

public class Result
{
    public bool IsSuccess
    {
        get;
    }
    public string Error
    {
        get;
    }
    public bool IsFailure => !IsSuccess;

    internal Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new(true, string.Empty);
    }

    public static Result Failure(string error)
    {
        return new(false, error);
    }
}
