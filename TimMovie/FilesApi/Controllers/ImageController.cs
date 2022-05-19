using FilesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilesApi.Controllers;

[ApiController]
[Route("file-api/image")]
public class ImageController : ControllerBase
{
    private readonly ImageService _imageService;
    private readonly FileService _fileService;

    public ImageController(ImageService imageService, FileService fileService)
    {
        _imageService = imageService;
        _fileService = fileService;
    }

    [HttpGet]
    public IActionResult GetImage([FromQuery]string relativePath)
    {
        if (!_fileService.ContentFileIsExisted(relativePath))
        {
            return NotFound();
        }

        var absolutePath = _fileService.GetAbsolutePathToFile(relativePath);

        return PhysicalFile(absolutePath, "image/jpeg");
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