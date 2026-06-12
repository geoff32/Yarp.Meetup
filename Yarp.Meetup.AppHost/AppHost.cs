var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithRedisCommander()
    .WithDataVolume()
    .WithPersistence();

var onpremise = builder.AddProject<Projects.WebApi_OnPremise>("webapi-onpremise");

var azure = builder.AddProject<Projects.WebApi_Azure>("webapi-azure");

builder.AddProject<Projects.Yarp_Meetup>("yarp-meetup")
    .WithReference(onpremise)
    .WithReference(azure)
    .WithReference(redis);

builder.Build().Run();
