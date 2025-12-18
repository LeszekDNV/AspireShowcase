using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlobStorageController(BlobServiceClient blobServiceClient) : ControllerBase
{
    private const string ContainerName = "test-container-1";

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file is not { Length: not 0 })
        {
            return BadRequest("No file provided");
        }

        try
        {
            BlobContainerClient? containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
            await containerClient.CreateIfNotExistsAsync();

            BlobClient? blobClient = containerClient.GetBlobClient(file.FileName);

            await using Stream stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return Ok(new { fileName = file.FileName, message = "File uploaded successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error uploading file: {ex.Message}");
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {
        try
        {
            BlobContainerClient? containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            if (!await containerClient.ExistsAsync())
            {
                return Ok(new List<object>());
            }

            List<object> blobs = [];

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(new
                {
                    name = blobItem.Name,
                    size = blobItem.Properties.ContentLength,
                    lastModified = blobItem.Properties.LastModified,
                    contentType = blobItem.Properties.ContentType
                });
            }

            return Ok(blobs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error listing files: {ex.Message}");
        }
    }
}