using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuperLibrary.Web.Pipelines;
using SuperLibrary.Web.Results;

namespace SuperLibrary.Web.Extensions;

public static class PipelineBehavioursExtensions
{
    public static IServiceCollection AddValidationPipelines(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var requestTypes = assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IRequest<Result>)))
            .ToList();

        foreach (var requestType in requestTypes)
        {
            var genericResultRequestType = requestType
                .GetInterfaces()
                .FirstOrDefault(i => i.GetGenericArguments().Any(a => a.GenericTypeArguments.Length == 1));

            if (genericResultRequestType is not null)
            {
                var resultArgument = genericResultRequestType.GetGenericArguments().First();
                var interfaceValidationPipeline = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, resultArgument);
                var validationPipelineType = typeof(ValidationPipelineGeneric<,>).MakeGenericType(requestType, resultArgument.GetGenericArguments().First());

                serviceCollection.Add(new ServiceDescriptor(interfaceValidationPipeline, validationPipelineType, ServiceLifetime.Singleton));
            }
            else
            {
                var resultRequestType = requestType
                    .GetInterfaces()
                    .FirstOrDefault(i => i.GetGenericArguments().Any(a => a == typeof(Result)));

                if (resultRequestType is null)
                {
                    continue;
                }

                var resultArgument = resultRequestType.GetGenericArguments().First();
                var interfaceValidationPipeline = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, resultArgument);
                var validationPipelineType = typeof(ValidationPipeline<>).MakeGenericType(requestType);

                serviceCollection.Add(new ServiceDescriptor(interfaceValidationPipeline, validationPipelineType, ServiceLifetime.Singleton));
            }
        }

        return serviceCollection;
    }
}
