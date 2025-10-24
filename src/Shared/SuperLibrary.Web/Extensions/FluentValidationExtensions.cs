using System.Linq.Expressions;
using FluentValidation;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.Results.Generic;
using SuperLibrary.Web.ValidationErrors;

namespace SuperLibrary.Web.Extensions;

public static class FluentValidationExtensions
{
    public static Result<TEntity> ToResult<TEntity>(this IValidator<TEntity> entityValidator, TEntity entity, params Expression<Func<TEntity, object?>>[] propertiesSelector)
    {
        var validationResult = propertiesSelector.Length > 0
            ? entityValidator.Validate(entity, strategy => strategy.IncludeProperties(propertiesSelector))
            : entityValidator.Validate(entity);

        if (!validationResult.IsValid)
            return Result<TEntity>.Failure(
                validationResult
                    .Errors
                    .DistinctBy(e => e.ErrorCode)
                    .ToDictionary(e => e.ErrorCode, e => ValidationError.CreatePropertyValidation(e.ErrorCode, e.ErrorMessage, e.PropertyName)),
                null,
                ResultStatus.InvalidArgument);

        return entity;
    }
}