using System;
using FluentValidation;                                                                                          
using Microsoft.Extensions.DependencyInjection; 

namespace MyTravel.Application.DI;

public  static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);                                
        return services; 
    }
}
