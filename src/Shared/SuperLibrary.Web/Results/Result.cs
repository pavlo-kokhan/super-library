using SuperLibrary.Web.Results.Abstract;
using SuperLibrary.Web.ValidationErrors;

namespace SuperLibrary.Web.Results;

public class Result : IResult
{
    protected Result()
    {
        IsSuccess = true;
        DetailedErrors = new Dictionary<string, ValidationError>();
        Errors = GetSimpleErrors(DetailedErrors);
        ResultStatus = ResultStatus.Ok;
    }

    protected Result(IDictionary<string, ValidationError> errors, ResultStatus resultStatus, Exception? exception)
    {
        DetailedErrors = errors;
        Errors = GetSimpleErrors(errors);
        IsSuccess = false;
        ResultStatus = resultStatus;
        Exception = exception;
    }

    protected Result(ValidationError error)
        : this(
            new Dictionary<string, ValidationError>
            {
                [error.Code] = error
            },
            error.ResultStatus,
            error.Exception)
    { }

    protected Result(IDictionary<string, string?> errors, Exception? exception, ResultStatus resultStatus)
        : this(
            errors
                .ToDictionary(er => er.Key, er => new ValidationError(er.Key, er.Value, null, exception, resultStatus)),
            resultStatus,
            exception)
    { }

    protected Result(string errorCode, Exception? exception, ResultStatus resultStatus)
        : this(
            new Dictionary<string, ValidationError>
            {
                [errorCode] = ValidationError.Create(errorCode, exception, resultStatus)
            },
            resultStatus,
            exception)
    { }

    public bool IsSuccess { get; }

    public bool IsFailure
        => !IsSuccess;

    public IDictionary<string, string?> Errors { get; }

    public IDictionary<string, ValidationError> DetailedErrors { get; }

    public ResultStatus ResultStatus { get; }

    public Exception? Exception { get; }

    public static Result Success()
        => new();

    public static Result Failure(IDictionary<string, string?> errors, Exception? exception, ResultStatus resultStatus)
        => new(errors, exception, resultStatus);

    public static Result Failure(string errorCode, Exception? exception, ResultStatus resultStatus)
        => new(errorCode, exception, resultStatus);

    public static Result Failure(IDictionary<string, ValidationError> errors, Exception? exception, ResultStatus resultStatus)
        => new(errors, resultStatus, exception);

    public static implicit operator Result(ValidationError validationError)
        => new(validationError);

    private IDictionary<string, string?> GetSimpleErrors(IDictionary<string, ValidationError> errors)
        => errors.ToDictionary(e => e.Key, e => e.Value.Message);
}