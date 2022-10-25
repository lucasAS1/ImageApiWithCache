using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using ImageApiWithCache.Infrastructure.Interfaces.Agents;
using ImageApiWithCache.Models.Service;
using Moq;
using Xunit;

namespace ImageApiWithCache.UnitTests;

public class ImageApiServiceTests
{
    private Mock<IImageApiAgent> _imageApiAgentMock;
    private readonly IFixture _fixture;

    public ImageApiServiceTests()
    {
        _imageApiAgentMock = new Mock<IImageApiAgent>();
        _fixture = new Fixture();

        _fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
    }

    [Fact]
    public async Task ShouldGetAlbumByIdCorrectly()
    {
        var uat = new ImageApiService(_imageApiAgentMock.Object);

        var result = await uat.GetAlbumById(1);

        Assert.IsType<JsonElement>(result);
    }
    
    [Fact]
    public async Task ShouldGetPhotoByIdCorrectly()
    {
        var uat = new ImageApiService(_imageApiAgentMock.Object);

        var result = await uat.GetPhotoById(1);

        Assert.IsType<JsonElement>(result);
    }
}
