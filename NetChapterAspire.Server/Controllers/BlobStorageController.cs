using Microsoft.AspNetCore.Mvc;
using NetChapterAspire.Server.Models.Common;
using NetChapterAspire.Server.Services.Interfaces;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlobStorageController(IBlobStorageService blobStorageService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file is not { Length: not 0 })
        {
            return BadRequest(ApiResponse.ErrorResponse("No file provided"));
        }

        await using var stream = file.OpenReadStream();
        var fileName = await blobStorageService.UploadFileAsync(stream, file.FileName);

        return Ok(ApiResponse<object>.SuccessResponse(
            new { fileName, message = "File uploaded successfully" }
        ));
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListFiles()
    {
        var files = await blobStorageService.ListFilesAsync();
        return Ok(ApiResponse<object>.SuccessResponse(files));
    }
}