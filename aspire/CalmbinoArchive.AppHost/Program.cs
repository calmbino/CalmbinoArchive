using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("db", port: 5432)
                .WithDataBindMount("../../docker/postgresql/data")
                .AddDatabase("CalmbinoArchive");

var cache = builder.AddGarnet("cache", port: 6379)
                   .WithDataBindMount("../../docker/garnet/data")
                   .WithPersistence(TimeSpan.FromSeconds(10));

var api = builder.AddProject<CalmbinoArchive_Api>("backend")
                 .WithReference(cache)
                 .WithReference(db)
                 .WaitFor(cache)
                 .WaitFor(db)
                 .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
                 .WithEndpoint("https", endpoint => endpoint.IsProxied = false);


builder.AddProject<CalmbinoArchive_MigrationService>("migrations")
       .WithReference(db);

// builder.AddProject<CalmbinoArchive_Web>("frontend")
//        .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
//        .WithEndpoint("https", endpoint => endpoint.IsProxied = false)
//        .WithReference(api)
//        .WaitFor(api)
//        .WithExternalHttpEndpoints();

 builder.Build()
       .Run();