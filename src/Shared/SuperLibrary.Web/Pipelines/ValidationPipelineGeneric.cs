using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.Results.Generic;
using SuperLibrary.Web.ValidationErrors;

namespace SuperLibrary.Web.Pipelines;

public class ValidationPipelineGeneric<TRequest, TData> : IPipelineBehavior<TRequest, Result<TData>>
    where TRequest : IRequest<Result<TData>>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationPipelineGeneric(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public Task<Result<TData>> Handle(TRequest request, RequestHandlerDelegate<Result<TData>> next, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator is null)
            return next(cancellationToken);

        var validationResult = validator.Validate(request);

        return !validationResult.IsValid
            ? Task.FromResult(
                Result<TData>.Failure(
                    validationResult
                        .Errors
                        .DistinctBy(e => e.ErrorCode)
                        .ToDictionary(e => e.ErrorCode, e => ValidationError.CreatePropertyValidation(e.ErrorCode, e.ErrorMessage, e.PropertyName)),
                    null,
                    ResultStatus.InvalidArgument))
            : next(cancellationToken);
    }
}
