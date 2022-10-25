using System.Text.Json;

namespace ImageApiWithCache.Domain.Interfaces;

public interface IImageApiService
{
    Task<JsonElement> GetAlbumById(int albumId);
    Task<JsonElement> GetPhotoById(int photoId);
}