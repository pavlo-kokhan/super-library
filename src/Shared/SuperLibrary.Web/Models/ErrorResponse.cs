using SuperLibrary.Web.Results;

namespace SuperLibrary.Web.Models;

public record ErrorResponse(IDictionary<string, string?> Errors, ResultStatus ResultStatus);
