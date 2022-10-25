using System.Text.Json;
using ImageApiWithCache.Domain.Interfaces;
using ImageApiWithCache.Infrastructure.Interfaces.Agents;

namespace ImageApiWithCache.Models.Service;

public class ImageApiService : IImageApiService
{
    private readonly IImageApiAgent _imageApiAgent;

    public ImageApiService(IImageApiAgent imageApiAgent)
    {
        _imageApiAgent = imageApiAgent;
    }

    public async Task<JsonElement> GetAlbumById(int albumId)
    {
        return await _imageApiAgent.GetAlbumById(albumId);
    }

    public async Task<JsonElement> GetPhotoById(int photoId)
    {
        return await _imageApiAgent.GetPhotoById(photoId);
    }
}