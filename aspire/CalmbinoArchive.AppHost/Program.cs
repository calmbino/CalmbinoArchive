using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<CalmbinoArchive_Api>("backend")
                 .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
                 .WithEndpoint("https", endpoint => endpoint.IsProxied = false);

builder.AddProject<CalmbinoArchive_Web>("frontend")
       .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
       .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
       .WithReference(api)
       .WithExternalHttpEndpoints();

builder.Build()
       .Run();