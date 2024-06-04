using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public class Result
    {
        protected internal Result(bool isSuccess, List<string>? errors)
        {
            if (isSuccess && errors is not null && errors.Count > 0)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && (errors is null || errors.Count == 0))
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Errors = errors ?? [];
        }

        public static Result Success() => new(true, null);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);

        public static Result Failure(List<string> errors) => new(false, errors);
        public static Result Failure(string error) => new(false, [error]);

        public static Result<TValue> Failure<TValue>(List<string>? errors) => new(default, false, errors);
        public static Result<TValue> Failure<TValue>(string error) => new(default, false, [error]);

        public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(new List<string>());

        public bool IsSuccess { get; }
        public List<string> Errors { get; } = [];
    }
}
