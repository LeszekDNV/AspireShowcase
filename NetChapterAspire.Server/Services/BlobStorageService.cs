using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NetChapterAspire.Server.Services.Interfaces;

namespace NetChapterAspire.Server.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;
    private const string ContainerName = "test-container-1";

    public BlobStorageService(BlobServiceClient blobServiceClient, ILogger<BlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);

        _logger.LogInformation("File uploaded successfully: {FileName}", fileName);

        return fileName;
    }

    public async Task<IEnumerable<BlobFileInfo>> ListFilesAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        if (!await containerClient.ExistsAsync())
        {
            return Enumerable.Empty<BlobFileInfo>();
        }

        var blobs = new List<BlobFileInfo>();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            blobs.Add(new BlobFileInfo(
                blobItem.Name,
                blobItem.Properties.ContentLength,
                blobItem.Properties.LastModified,
                blobItem.Properties.ContentType
            ));
        }

        _logger.LogInformation("Retrieved {Count} files from blob storage", blobs.Count);

        return blobs;
    }
}
