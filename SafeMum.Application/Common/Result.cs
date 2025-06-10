using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Common
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string? Error { get; }
    }

    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T> : Result, IResult
    {
        public T? Value { get; }

        private Result(T? value, bool isSuccess, string? error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(value, true, null);
        public static new Result<T> Failure(string error) => new(default, false, error);
    }


}
