using SuperLibrary.Web.ValidationErrors;

namespace SuperLibrary.Web.Results.Abstract;

public interface IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure { get; }

    public IDictionary<string, string?> Errors { get; }

    public IDictionary<string, ValidationError> DetailedErrors { get; }

    public ResultStatus ResultStatus { get; }

    public Exception? Exception { get; }
}