using CommenRabbitMQHelper;
using System;

using System.Text;

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using UserService.Managers;



using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserService.Controllers; // Erstat med den faktiske namespace for din controller
using Microsoft.Extensions.DependencyInjection; // Tilføj denne for DI support
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
namespace UserService.Services;



public class UserProcessingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserProcessingService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory; // Dependency injection til at skabe scopes
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "UserIdQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var userIdString = Encoding.UTF8.GetString(body);
                if (int.TryParse(userIdString, out int userId)) // Konverter string til int
                {
                    using (var scope = _scopeFactory.CreateScope()) // Opret et scope for hver anmodning
                    {
                        var userController = scope.ServiceProvider.GetRequiredService<CustomerController>();
                        var result = await userController.GetbyID(userId); // Asynkron kald til controller
                        if (result.Result is OkObjectResult okResult)
                        {
                            var userEmail = ((Customer)okResult.Value).Email; // Antager at Email er et felt i Customer
                            if (!string.IsNullOrEmpty(userEmail))
                            {
                                RabbitMqHelper.Send("UserEmailQueue", userEmail);
                            }
                        }
                    }
                }
            };
            channel.BasicConsume(queue: "UserIdQueue", autoAck: true, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);  // Reducer CPU brug
            }
        }
    }
}

