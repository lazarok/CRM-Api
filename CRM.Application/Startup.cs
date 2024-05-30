using System.Reflection;
using AutoMapper;
using CRM.Application.Behaviours;
using CRM.Application.Helpers.Mapping;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); }).CreateMapper());
        var provider = services.BuildServiceProvider();
        MappingHelper.Configure(provider.GetService<IMapper>()!);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}