using System;

namespace At.luki0606.DartZone.Shared.Results
{
    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException($"No value present. Error: {Error}");

        private Result(T value) : base(true, string.Empty)
        {
            _value = value;
        }

        private Result(string error) : base(false, error)
        {
            _value = default!;
        }

        public static Result<T> Success(T value)
        {
            return new(value);
        }

        public static new Result<T> Failure(string error)
        {
            return new(error);
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }
    }
}
