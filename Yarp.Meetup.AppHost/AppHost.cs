var builder = DistributedApplication.CreateBuilder(args);

var onpremise = builder.AddProject<Projects.WebApi_OnPremise>("webapi-onpremise");

var azure = builder.AddProject<Projects.WebApi_Azure>("webapi-azure");

builder.AddProject<Projects.Yarp_Meetup>("yarp-meetup")
    .WithReference(onpremise)
    .WithReference(azure);

builder.Build().Run();
