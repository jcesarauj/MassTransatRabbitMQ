using MassTransit;
using Subscriber;
using System;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(bus =>
{
    int numberOfConsumers = int.Parse(builder.Configuration["MessageBroker:NumberOfConsumers"]);
    int prefetchCount = int.Parse(builder.Configuration["MessageBroker:PrefetchCount"]);
    ushort port = ushort.Parse(builder.Configuration["MessageBroker:Port"]);

    bus.AddConsumer<InvoiceRequestEventConsumer>();

    bus.UsingRabbitMq((context, config) =>
    {
        var hostNames = builder.Configuration.GetSection("MessageBroker:HostNames").Get<string[]>();

        config.Host(hostNames.First(), port, builder.Configuration["MessageBroker:VirtualHost"], h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
            h.UseCluster(cluster =>
            {
                foreach (var host in hostNames)
                {
                    cluster.Node(host);
                }
            });
        });

        config.ReceiveEndpoint("invoice-request-endpoint-teste", ep =>
        {
            ep.PrefetchCount = prefetchCount;
            ep.ExclusiveConsumer = false;

            for (int i = 0; i < numberOfConsumers; i++)
            {
                ep.ConfigureConsumer<InvoiceRequestEventConsumer>(context);
            }
        });

        config.ConfigureEndpoints(context);
        config.UseMessageRetry(retry => { retry.Interval(int.Parse(builder.Configuration["MessageBroker:NumbersOfRetry"]), TimeSpan.FromSeconds(int.Parse(builder.Configuration["MessageBroker:RetryIntervalInSeconds"]))); });
    });
});

builder.Services.AddMassTransitHostedService();

var host = builder.Build();
host.Run();
