# 🏗️ APPLICATION ARCHITECTURE - QUICK REFERENCE

## 📁 Project Structure

```
NetChapterAspire.Server/
│
├── Controllers/                    # HTTP Layer
│   ├── BlobStorageController.cs   # File upload/download handling
│   ├── DatabaseController.cs      # Book CRUD operations
│   ├── MailingController.cs       # Email sending
│   └── ServiceBusController.cs    # Messaging
│
├── Services/                       # Business Logic Layer
│   ├── Interfaces/
│   │   ├── IBlobStorageService.cs
│   │   ├── IEmailService.cs
│   │   └── IServiceBusService.cs
│   │
│   ├── BlobStorageService.cs      # Azure Blob Storage logic
│   ├── EmailService.cs            # Email sending logic
│   └── ServiceBusService.cs       # Azure Service Bus logic
│
├── Models/
│   ├── Common/
│   │   └── ApiResponse.cs         # Common response model
│   ├── DTOs/
│   │   ├── AddBookRequest.cs
│   │   ├── SendMailRequest.cs
│   │   └── SendMessageRequest.cs
│   └── Book.cs                    # Entity
│
├── Data/
│   └── ApplicationDbContext.cs    # EF Core DbContext
│
├── Middleware/
│   └── GlobalExceptionHandler.cs  # Centralized error handling
│
└── Program.cs                      # Configuration and startup
```

---

## 🔄 REQUEST FLOW

### Example: File Upload to Blob Storage

```
1. HTTP Request
   POST /api/BlobStorage/upload
   ↓
2. BlobStorageController
   - Validation
   - Service invocation
   ↓
3. IBlobStorageService (interface)
   ↓
4. BlobStorageService (implementation)
   - Business logic
   - Azure SDK invocation
   ↓
5. Azure Blob Storage (infrastructure)
   ↓
6. Return through layers
   - Service → Controller
   - ApiResponse<T>
   ↓
7. HTTP Response (JSON)
```

---

## 🎯 LAYER RESPONSIBILITIES

### Controllers (HTTP Layer)
**Responsibilities:**
- Receiving HTTP requests
- Input validation
- Invoking appropriate service
- Returning HTTP responses

**SHOULD NOT:**
- ❌ Contain business logic
- ❌ Directly invoke Azure SDK
- ❌ Handle exceptions (GlobalExceptionHandler does this)
- ❌ Create complex objects

**Example:**
```csharp
[HttpPost("upload")]
public async Task<IActionResult> UploadFile(IFormFile? file)
{
    if (file is not { Length: not 0 })
        return BadRequest(ApiResponse.ErrorResponse("No file provided"));

    await using var stream = file.OpenReadStream();
    var fileName = await _blobStorageService.UploadFileAsync(stream, file.FileName);

    return Ok(ApiResponse<object>.SuccessResponse(
        new { fileName, message = "File uploaded successfully" }
    ));
}
```

---

### Services (Business Logic Layer)
**Responsibilities:**
- Business logic
- Operation orchestration
- Calling infrastructure (Azure SDK, DB)
- Operation logging

**SHOULD NOT:**
- ❌ Know about HTTP (Request, Response)
- ❌ Create ActionResults
- ❌ Handle routing

**Example:**
```csharp
public class BlobStorageService : IBlobStorageService
{
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);

        _logger.LogInformation("File uploaded: {FileName}", fileName);
        return fileName;
    }
}
```

---

### Middleware (Cross-cutting concerns)
**Responsibilities:**
- Global error handling
- Request logging
- Authentication/Authorization (future)

**Example:**
```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception");
        
        var response = ApiResponse.ErrorResponse("An error occurred");
        await httpContext.Response.WriteAsJsonAsync(response);
        
        return true;
    }
}
```

---

## 🧪 HOW TO TEST

### 1. **Service Unit Tests**
```csharp
public class BlobStorageServiceTests
{
    [Fact]
    public async Task UploadFileAsync_ValidStream_ReturnsFileName()
    {
        // Arrange
        var mockBlobServiceClient = new Mock<BlobServiceClient>();
        var mockLogger = new Mock<ILogger<BlobStorageService>>();
        var service = new BlobStorageService(mockBlobServiceClient.Object, mockLogger.Object);
        
        // Act
        var result = await service.UploadFileAsync(stream, "test.txt");
        
        // Assert
        Assert.Equal("test.txt", result);
    }
}
```

### 2. **Controller Integration Tests**
```csharp
public class BlobStorageControllerTests
{
    [Fact]
    public async Task UploadFile_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var mockService = new Mock<IBlobStorageService>();
        mockService.Setup(x => x.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                  .ReturnsAsync("test.txt");
        
        var controller = new BlobStorageController(mockService.Object);
        
        // Act
        var result = await controller.UploadFile(CreateMockFile());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);
    }
}
```

---

## 📝 HOW TO ADD A NEW ENDPOINT

### Step 1: Create service interface
```csharp
public interface IMyNewService
{
    Task<MyResult> DoSomethingAsync(MyInput input);
}
```

### Step 2: Service implementation
```csharp
public class MyNewService : IMyNewService
{
    private readonly ILogger<MyNewService> _logger;
    
    public MyNewService(ILogger<MyNewService> logger)
    {
        _logger = logger;
    }
    
    public async Task<MyResult> DoSomethingAsync(MyInput input)
    {
        // Your business logic
        _logger.LogInformation("Doing something");
        return new MyResult();
    }
}
```

### Step 3: Register in DI
```csharp
// Program.cs
builder.Services.AddScoped<IMyNewService, MyNewService>();
```

### Step 4: Create controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class MyNewController : ControllerBase
{
    private readonly IMyNewService _service;
    
    public MyNewController(IMyNewService service)
    {
        _service = service;
    }
    
    [HttpPost("dosomething")]
    public async Task<IActionResult> DoSomething([FromBody] MyInput input)
    {
        var result = await _service.DoSomethingAsync(input);
        return Ok(ApiResponse<MyResult>.SuccessResponse(result));
    }
}
```

---

## 🔧 DEPENDENCY INJECTION

### Lifecycle Scopes:

```csharp
// Scoped - new instance per HTTP request (recommended for most services)
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Transient - new instance every time
builder.Services.AddTransient<IEmailService, EmailService>();

// Singleton - one instance for entire application (cache, configuration)
builder.Services.AddSingleton<IMyCache, MyCache>();
```

### When to use which?

| Scope | Usage | Example |
|-------|--------|----------|
| **Scoped** | Services with DB access, Azure SDK | BlobStorageService, DatabaseService |
| **Transient** | Lightweight stateless services | EmailService, ValidationService |
| **Singleton** | Cache, configuration, heavy objects | MemoryCache, Configuration |

---

## 🚨 BEST PRACTICES

### ✅ DO:
- Use interfaces for all services
- Return `ApiResponse<T>` from controllers
- Log important operations
- Use async/await consistently
- Validate input at controller level

### ❌ DON'T:
- Don't use try-catch in controllers (GlobalExceptionHandler handles it)
- Don't mix business logic with HTTP
- Don't create dependencies on concrete implementations
- Don't log in controllers (only in services)

---

## 📊 MONITORING AND LOGGING

### Logging levels:

```csharp
// Information - normal operations
_logger.LogInformation("File uploaded: {FileName}", fileName);

// Warning - unexpected situations, but not errors
_logger.LogWarning("Large file detected: {Size}", size);

// Error - errors with exceptions
_logger.LogError(ex, "Failed to upload file");

// Debug - detailed info for development
_logger.LogDebug("Processing file: {FileName}", fileName);
```

---

## 🔄 MIGRATING OLD CODE

### Old pattern (❌):
```csharp
[HttpPost]
public async Task<IActionResult> DoSomething()
{
    try
    {
        // business logic directly in controller
        var client = new AzureClient();
        var result = await client.DoSomethingAsync();
        return Ok(new { success = true, data = result });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message });
    }
}
```

### New pattern (✅):
```csharp
[HttpPost]
public async Task<IActionResult> DoSomething()
{
    var result = await _service.DoSomethingAsync();
    return Ok(ApiResponse<MyResult>.SuccessResponse(result));
}
```

---
