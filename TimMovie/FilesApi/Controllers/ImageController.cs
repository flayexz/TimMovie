using FilesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilesApi.Controllers;

[ApiController]
[Route("file-api/image")]
public class ImageController : ControllerBase
{
    private const string DefaultImgName = "default.jpg";
    private readonly ImageService _imageService;
    private readonly FileService _fileService;

    public ImageController(ImageService imageService, FileService fileService)
    {
        _imageService = imageService;
        _fileService = fileService;
    }

    [HttpGet]
    public IActionResult GetImage([FromQuery] string relativePath)
    {
        if (!_fileService.ContentFileIsExisted(relativePath))
        {
            if (!TryGetAbsolutePathDefaultImage(relativePath, out var absolutePathDefaultImg))
                return NotFound();
            return PhysicalFile(absolutePathDefaultImg, "image/jpeg");
        }

        var absolutePath = _fileService.GetAbsolutePathToFile(relativePath);

        return PhysicalFile(absolutePath, "image/jpeg");
    }

    private bool TryGetAbsolutePathDefaultImage(string relativePath, out string absolutePathDefaultImg)
    {
        absolutePathDefaultImg = null!;
        var parts = relativePath.Split("/");
        if (parts.Length != 3)
            return false;
        var newPath = $"{parts[0]}/{parts[1]}/{DefaultImgName}";
        if (!_fileService.ContentFileIsExisted(newPath))
            return false;
        absolutePathDefaultImg = _fileService.GetAbsolutePathToFile(newPath);
        return true;
    }

    [HttpGet("user-photo/default")]
    public async Task<string> GetLinkToDefaultUserPhoto()
    {
        return Url.Action(
            "GetImage",
            "Image",
            new {relativePath = _imageService.GetLinkToDefaultUserPhoto()})!;
    }


    [HttpPost("user-photo")]
    public async Task<IActionResult> SaveUserPhoto([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveUserPhotoAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("film")]
    public async Task<IActionResult> SaveFilmImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageForFilmAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("actor")]
    public async Task<IActionResult> SaveActorImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageForActorAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("banner")]
    public async Task<IActionResult> SaveBannerImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageForBannerAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("producer")]
    public async Task<IActionResult> SaveProducerImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageProducerAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    private string GetUrlToImage(string relativePath)
    {
        return Url.Action(
            "GetImage",
            "Image",
            new {relativePath = relativePath},
            Uri.UriSchemeHttps)!;
    }
}