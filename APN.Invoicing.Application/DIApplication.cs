using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace APN.Invoicing.Application;

public static class DIApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DIApplication).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<IOperationValidationService, OperationValidationService>();
        services.AddScoped<IInvoiceService, InvoiceService>();

        services.AddAutoMapper(assembly);

        return services;
    }
}
