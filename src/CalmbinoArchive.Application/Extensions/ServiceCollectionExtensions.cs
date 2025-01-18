using CalmbinoArchive.Application.DTOs.Validators;
using CalmbinoArchive.Application.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CalmbinoArchive.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfig));

        services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>(includeInternalTypes: true);

        return services;
    }
}