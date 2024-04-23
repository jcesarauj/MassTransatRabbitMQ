using MassTransit;
using Publisher.EventBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(bus =>
    {
        bus.UsingRabbitMq((context, config) =>
        {
            var hostNames = builder.Configuration.GetSection("MessageBroker:HostNames").Get<string[]>();

            config.Host(hostNames.First(), builder.Configuration["MessageBroker:VirtualHost"], h =>
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

            config.ConfigureEndpoints(context);
            config.UseMessageRetry(retry => { retry.Interval(int.Parse(builder.Configuration["MessageBroker:NumbersOfRetry"]), TimeSpan.FromSeconds(int.Parse(builder.Configuration["MessageBroker:RetryIntervalInSeconds"]))); });
        });
    });

builder.Services.AddTransient<IEventBus, EventBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
