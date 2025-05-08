using AzureCognitiveIntegration.Features.Vision.Models;
using AzureCognitiveIntegration.Features.Vision.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AzureCognitiveIntegration.Tests.VisionTests;

[TestClass]
public class DetectFaceTest
{
    [TestMethod]
    public async Task DetectFaceAsync_ReturnsConfidenceLevel()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Vision:Endpoint", "https://fake-endpoint" },
            { "Vision:Key", "fake-key" },
            { "Vision:UseMock", "true" } 
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        var loggerMock = new Mock<ILogger<VisionService>>();

        var serviceMock = new Mock<IVisionService>();

        var fakeRequestData = new FacialComparisonRequest()
        {
            Image1 = new FormFile(new MemoryStream(new byte[100]), 0, 100, "Data", "id.jpg"),
            Image2 = new FormFile(new MemoryStream(new byte[100]), 0, 100, "Data", "id.jpg")

        };
        
        
    }


    [TestMethod]
    public void DetectFaceAsync_ReturnsSuccess()
    {
        
        
    }


    [TestMethod]
    public void DetectFaceAsync_ReturnsIdentical()
    {
        
    }
}