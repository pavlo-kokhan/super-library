using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace SuperLibrary.Web.Pipelines;

public class ValidationPipeline<TRequest> : IPipelineBehavior<TRequest, Result>
    where TRequest : IRequest<Result>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationPipeline(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator is null)
            return next(cancellationToken);

        var validationResult = validator.Validate(request);

        return !validationResult.IsValid
            ? Task.FromResult(
                Result.Failure(
                    validationResult
                        .Errors
                        .DistinctBy(e => e.ErrorCode)
                        .ToDictionary(e => e.ErrorCode, e => ValidationError.CreatePropertyValidation(e.ErrorCode, e.ErrorMessage, e.PropertyName)),
                    null,
                    ResultStatus.InvalidArgument))
            : next(cancellationToken);
    }
}
