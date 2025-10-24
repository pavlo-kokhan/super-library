namespace SuperLibrary.Web.Results;

public enum ResultStatus
{
    Ok,
    InvalidArgument,
    Forbidden,
    Unauthenticated,
    NotFound,
    InternalError,
    ApiError
}