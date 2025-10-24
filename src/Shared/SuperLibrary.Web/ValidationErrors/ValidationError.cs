using SuperLibrary.Web.Results;

namespace SuperLibrary.Web.ValidationErrors;

public class ValidationError
{
    public ValidationError(string code, string? message, string? propertyName, Exception? exception, ResultStatus resultStatus)
    {
        Code = code;
        Message = message;
        Exception = exception;
        ResultStatus = resultStatus;
        PropertyName = propertyName;
    }

    public string Code { get; }

    public string? Message { get; }

    public Exception? Exception { get; }

    public ResultStatus ResultStatus { get; }

    public string? PropertyName { get; }

    public static ValidationError CreateInvalidArgument(string code, ResultStatus resultStatus = ResultStatus.InvalidArgument)
        => new(code, null, null, null, resultStatus);

    public static ValidationError Create(string code, Exception? exception = null, ResultStatus resultStatus = ResultStatus.InvalidArgument)
        => new(code, null, null, exception, resultStatus);

    public static ValidationError CreatePropertyValidation(string code, string? message, string propertyName)
        => new(code, message, propertyName, null, ResultStatus.InvalidArgument);
}
