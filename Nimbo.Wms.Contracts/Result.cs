using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts;

[PublicAPI]
public enum ErrorType
{
    None = 0,
    NotFound,
    Conflict,
    Validation,
    BusinessRule,
    Unexpected,
}

[PublicAPI]
public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    public static Error BusinessRule(string code, string message) => new(code, message, ErrorType.BusinessRule);

    public static Error Unexpected(string code, string message) => new(code, message, ErrorType.Unexpected);
}

[PublicAPI]
public class Result
{
    public Result(Error error)
    {
        Error = error;
    }

    public Error Error { get; }

    public bool IsSuccess => Error.Type == ErrorType.None;

    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(Error.None);

    public static Result Failure(Error error) => error.Type == ErrorType.None
        ? throw new InvalidOperationException("A failure must carry a non-None error.")
        : new(error);

    public static Result<T> Success<T>(T value) => new(value, Error.None);

    public static Result<T> Failure<T>(Error error) => error.Type == ErrorType.None
        ? throw new InvalidOperationException("A failure must carry a non-None error.")
        : new(default!, error);

    public static implicit operator Result(Error error) => Failure(error);

    public TOut Match<TOut>(Func<TOut> onSuccess, Func<Error, TOut> onFailure) => IsSuccess ? onSuccess() : onFailure(Error);
}

[PublicAPI]
public sealed class Result<T> : Result
{
    private readonly T _value;

    internal Result(T value, Error error) : base(error) { _value = value; }

    public T Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access Value of a failed Result.");

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => Failure<T>(error);

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure) => IsSuccess ? onSuccess(_value) : onFailure(Error);
}
