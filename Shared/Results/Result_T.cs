using System;

namespace At.luki0606.DartZone.Shared.Results;

public class Result<T> : Result
{
    public T Value => IsSuccess
        ? field!
        : throw new InvalidOperationException($"No value present. Error: {Error}");

    private Result(T value) : base(true, string.Empty)
    {
        Value = value;
    }

    private Result(string error) : base(false, error)
    {
        Value = default!;
    }

#pragma warning disable CA1000 // Do not declare static members on generic types
    public static Result<T> Success(T value)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return new(value);
    }

#pragma warning disable CA1000 // Do not declare static members on generic types
    public static new Result<T> Failure(string error)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        return new(error);
    }

    public static implicit operator T(Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return result.Value;
    }

    public T ToT()
    {
        return Value;
    }
}
