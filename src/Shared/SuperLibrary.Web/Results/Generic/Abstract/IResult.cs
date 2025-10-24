using SuperLibrary.Web.Results.Abstract;

namespace SuperLibrary.Web.Results.Generic.Abstract;

public interface IResult<out T> : IResult
{
    T Data { get; }
}