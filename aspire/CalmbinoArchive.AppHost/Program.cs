using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("db")
                .WithPgAdmin();
var cache = builder.AddRedis("cache")
                   .WithImageRegistry("ghcr.io")
                   .WithImage("microsoft/garnet")
                   .WithImageTag("latest")
                   .WithRedisInsight();

var api = builder.AddProject<CalmbinoArchive_Api>("backend")
                 .WithReference(cache)
                 .WithReference(db)
                 .WaitFor(cache)
                 .WaitFor(db)
                 .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
                 .WithEndpoint("https", endpoint => endpoint.IsProxied = false);

builder.AddProject<CalmbinoArchive_Web>("frontend")
       .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
       .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
       .WithReference(api)
       .WithExternalHttpEndpoints();


builder.Build()
       .Run();