using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ.Consumer;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Configuration;
using ArquiteturaDesafio.Infrastructure.Persistence.Repositories;
using ArquiteturaDesafio.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

//Rabbit MQ
var configRabbit = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
builder.Services.AddSingleton<IConsumerMessage>(_ => new RabbitMQConsumer(configRabbit.Hostname, configRabbit.Username, configRabbit.Password));

// MongoDB
var configMongo = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Registra o cliente do MongoDB
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(configMongo.ConnectionString);
});

// Registra o banco de dados do MongoDB
builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(configMongo.DatabaseName);
});

builder.Services.AddSingleton<IDailyBalanceReportRepository, DailyBalanceReportRepository>();

var host = builder.Build();
host.Run();
