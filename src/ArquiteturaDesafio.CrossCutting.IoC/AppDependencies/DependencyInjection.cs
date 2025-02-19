using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Configuration;
using ArquiteturaDesafio.Infrastructure.Persistence.Repositories;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Service;
using ArquiteturaDesafio.Core.Application.UseCases.Mapper;
using ArquiteturaDesafio.Core.Application.Shared.Behavior;
using ArquiteturaDesafio.Infrastructure.Security;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ;
namespace ArquiteturaDesafio.Infrastructure.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Postgres
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // MongoDB
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

            // Registra o cliente do MongoDB
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Registra o banco de dados do MongoDB
            services.AddScoped<IMongoDatabase>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });

            // Adicionar a configuração RabbitMQ

            services.AddScoped<IProducerMessage>(_ => new RabbitMQProducer(configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>().Hostname));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<JwtTokenService>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddScoped<CounterService>();
            services.AddAutoMapper(typeof(CommonMapper));

            var myhandlers = AppDomain.CurrentDomain.Load("ArquiteturaDesafio.Core.Application");
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(myhandlers);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.Load("ArquiteturaDesafio.Core.Application"));

            return services;


        }
    }
}
