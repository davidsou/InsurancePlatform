namespace Insurance.SharedKernel;

public sealed record Error(string Code, string Message);

public class OperationResult
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected OperationResult(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static OperationResult Ok() => new(true, null);

    public static OperationResult Fail(string code, string message)
        => new(false, new Error(code, message));
}

public sealed class OperationResult<T> : OperationResult
{
    public T? Value { get; }

    private OperationResult(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
        => Value = value;

    public static OperationResult<T> Ok(T value) => new(true, value, null);

    public static new OperationResult<T> Fail(string code, string message)
        => new(false, default, new Error(code, message));
}

public static class Errors
{
    public static OperationResult Validation(string message)
        => OperationResult.Fail("validation", message);

    public static OperationResult BusinessRule(string message)
        => OperationResult.Fail("business_rule", message);

    public static OperationResult Conflict(string message)
        => OperationResult.Fail("conflict", message);

    public static OperationResult NotFound(string entity, string id)
        => OperationResult.Fail("not_found", $"{entity} '{id}' was not found.");

    public static OperationResult<T> NotFound<T>(string entity, string id)
        => OperationResult<T>.Fail("not_found", $"{entity} '{id}' was not found.");
}
