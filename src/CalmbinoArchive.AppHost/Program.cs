using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<CalmbinoArchive_Api>("api");

builder.AddProject<CalmbinoArchive_Web>("web").WithReference(api).WithExternalHttpEndpoints();

builder.Build().Run();