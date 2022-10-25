using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ImageApiWithCache.Domain.Interfaces;
using ImageApiWithCache.Infrastructure.Agents;
using Microsoft.AspNetCore.Mvc;

namespace ImageApiWithCache.Host.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("[controller]")]
public class ImageApiController : ControllerBase
{
    private readonly IImageApiService _imageService;

    public ImageApiController(IImageApiService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet]
    [Route("albums/{albumId}/photos")]
    [Cached(600)]
    public async Task<IActionResult> GetAlbumByIdAsync(int albumId)
    {
        return Ok(await _imageService.GetAlbumById(albumId));
    }
    
    [HttpGet]
    [Route("photos/{photoId}")]
    [Cached(300)]
    public async Task<IActionResult> GetPhotoByIdAsync(int photoId)
    {
        return Ok(await _imageService.GetPhotoById(photoId));
    }
}