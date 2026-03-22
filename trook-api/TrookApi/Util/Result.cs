namespace TrookApi.Util;

public class Result
{
    protected const string DefaultSuccessMessage = "Completed successfully";
    protected const string DefaultFailureMessage = "Unexpected error occurred";

    public bool IsSuccess { get; init; }
    
    public required string Message { get; init; }

    public List<Exception> Errors { get; init; } = [];
    
    protected Result() {}

    public static Result Success() => Success(DefaultSuccessMessage);

    public static Result Success(string message) => new() { IsSuccess = true, Message = message };

    public static Result Failure() => Failure(DefaultFailureMessage);

    public static Result Failure(string message) => new() { IsSuccess = false, Message = message };

    public static Result Failure(params Exception[] errors) => Failure(DefaultFailureMessage, errors);

    public static Result Failure(string message, params Exception[] errors) =>
        new() { IsSuccess = false, Message = message, Errors = errors.ToList() };
}

public sealed class Result<T> : Result
{
    public T? Data { get; init; }
    
    private Result() {}

    public static new Result<T> Success() => Success(DefaultSuccessMessage);

    public static new Result<T> Success(string message) => Success(default, message);
    
    public static Result<T> Success(T data) => Success(data, DefaultSuccessMessage);

    public static Result<T> Success(T? data, string message) =>
        new() { IsSuccess = true, Message = message, Data = data };

    public static new Result<T> Failure() => Failure(DefaultFailureMessage);

    public static new Result<T> Failure(string message) => Failure(default, message);

    public static Result<T> Failure(T data) => Failure(data, DefaultFailureMessage);

    public static new Result<T> Failure(params Exception[] errors) => Failure(DefaultFailureMessage, errors);

    public static new Result<T> Failure(string message, params Exception[] errors) => Failure(default, message, errors);
    
    public static Result<T> Failure(T data, params Exception[] errors) => Failure(data, DefaultFailureMessage, errors);

    public static Result<T> Failure(T? data, string message, params Exception[] errors) =>
        new() { IsSuccess = false, Message = message, Data = data, Errors = errors.ToList() };
}