using System;

namespace At.luki0606.DartZone.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string error)
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

    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        private readonly T _value;
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException($"No value present. Error: {Error}");

        private Result(T value)
        {
            IsSuccess = true;
            _value = value;
            Error = string.Empty;
        }

        private Result(string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static Result<T> Success(T value)
        {
            return new(value);
        }

        public static Result<T> Failure(string error)
        {
            return new(error);
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }
    }
}
