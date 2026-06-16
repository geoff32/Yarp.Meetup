using StackExchange.Redis;
using Yarp.Meetup;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddHttpForwarderWithServiceDiscovery();

builder.Services
    .AddSingleton<ICloudProvider, CloudProvider>()
    .AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnectionString = builder.Configuration.GetConnectionString("redis");

            ArgumentException.ThrowIfNullOrEmpty(redisConnectionString);
            var redisConfig = ConfigurationOptions.Parse(redisConnectionString);

            redisConfig.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(redisConfig);
        });

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapReverseProxy(pipeline => pipeline.UseMiddleware<AzureSwitchMiddleware>());

app.MapControllers();

app.Run();
