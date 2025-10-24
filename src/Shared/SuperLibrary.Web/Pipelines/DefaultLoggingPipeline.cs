using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using SuperLibrary.Web.Results;

namespace SuperLibrary.Web.Pipelines;

public class DefaultLoggingPipeline<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    private readonly ILogger<DefaultLoggingPipeline<TRequest, TResult>> _logger;

    public DefaultLoggingPipeline(ILogger<DefaultLoggingPipeline<TRequest, TResult>> logger)
        => _logger = logger;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var sw = Stopwatch.StartNew();

        _logger.LogInformation("Starting process request {RequestName}", requestType.Name);

        var result = await next(cancellationToken);

        sw.Stop();

        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "Request {RequestName} was processed successfully. Process time: {ProcessTime}ms",
                requestType.Name,
                sw.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogWarning(
                result.Exception,
                "Request {RequestName} was not processed successfully {@Errors}. Result status: {ResultStatus}. Request: {@Request}. Process time: {ProcessTime}ms",
                requestType.Name,
                result.DetailedErrors,
                result.ResultStatus,
                request,
                sw.ElapsedMilliseconds);
        }

        return result;
    }
}
