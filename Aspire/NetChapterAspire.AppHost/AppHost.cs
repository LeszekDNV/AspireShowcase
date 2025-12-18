using Aspire.Hosting.Azure;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<SqlServerDatabaseResource> sqlDatabase = builder.AddSqlServer("sqlserver", port: 2137)
    .WithImage("mssql/server:2025-latest")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("CDB");

IResourceBuilder<MailPitContainerResource> mailSmtp = builder.AddMailPit("smtp", 1080, 1025);

IResourceBuilder<AzureStorageResource> storage = builder.AddAzureStorage("Storage")
    // Use the Azurite storage emulator for local development
    .RunAsEmulator(emulator =>
    {
        emulator
            .WithBlobPort(10000)
            .WithQueuePort(10001)
            .WithTablePort(10002)
            .WithDataVolume("AspireTest");
    });

IResourceBuilder<AzureBlobStorageResource> blobs = storage.AddBlobs("Blobs");
storage.AddBlobContainer("mycontainer1", blobContainerName: "test-container-1");

IResourceBuilder<AzureQueueStorageResource> queues = storage.AddQueues("Queues");

IResourceBuilder<AzureServiceBusResource> serviceBus = builder.AddAzureServiceBus("ServiceBus")
    .RunAsEmulator();
serviceBus.AddServiceBusQueue("demo-queue");


IResourceBuilder<ProjectResource> api = builder.AddProject<Projects.NetChapterAspire_Server>("netchapteraspire-server")
.WithExternalHttpEndpoints()
.WithReference(sqlDatabase, "DefaultConnection")
.WithReference(blobs)
.WithReference(queues)
.WithReference(mailSmtp)
.WithReference(serviceBus)
.WaitFor(sqlDatabase)
.WaitFor(storage);

builder.AddViteApp("vite-frontend", "../../netchapteraspire.client")
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api)
    .WithNpm();

builder.Build().Run();
