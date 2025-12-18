using Azure.Storage.Blobs.Models;

namespace NetChapterAspire.Server.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task<IEnumerable<BlobFileInfo>> ListFilesAsync();
}

public record BlobFileInfo(string Name, long? Size, DateTimeOffset? LastModified, string? ContentType);
