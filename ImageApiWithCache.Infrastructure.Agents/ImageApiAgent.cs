using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Flurl.Http;
using ImageApiWithCache.Infrastructure.Interfaces.Agents;
using Polly;

namespace ImageApiWithCache.Infrastructure.Agents;

[ExcludeFromCodeCoverage]
public class ImageApiAgent : IImageApiAgent
{
    public async Task<JsonElement> GetAlbumById(int albumId)
    {
        var result = await Policy
            .Handle<FlurlHttpException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
            .ExecuteAsync(() =>
                $"https://jsonplaceholder.typicode.com/albums/{albumId}/photos"
                    .WithTimeout(5)
                    .GetAsync()
                ).ReceiveJson<JsonElement>();

        return result;
    }

    public async Task<JsonElement> GetPhotoById(int photoId)
    {
        var result = await Policy
            .Handle<FlurlHttpException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
            .ExecuteAsync(() =>
                $"https://jsonplaceholder.typicode.com/photos/{photoId}"
                    .WithTimeout(5)
                    .GetAsync()
            ).ReceiveJson<JsonElement>();

        return result;
    }
}