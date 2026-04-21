using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/files")]
[Authorize(Roles = "SuperAdmin,Expert")]
public class FileController : ControllerBase
{
    private readonly string _uploadPath;

    public FileController(IConfiguration configuration)
    {
        _uploadPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            configuration["FileStorage:Path"]!);

        if (!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath);
    }

    [HttpPost("audio")]
    public async Task<IActionResult> UploadAudio(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл пустой");

        var allowed = new[] { ".mp3", ".wav", ".ogg" };
        var ext = Path.GetExtension(file.FileName).ToLower();

        if (!allowed.Contains(ext))
            return BadRequest("Только mp3, wav, ogg");

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { url = $"/uploads/{fileName}" });
    }
}