using System.Text.Json;

namespace ImageApiWithCache.Infrastructure.Interfaces.Agents;

public interface IImageApiAgent
{
    Task<JsonElement> GetAlbumById(int albumId);
    Task<JsonElement> GetPhotoById(int photoId);
}