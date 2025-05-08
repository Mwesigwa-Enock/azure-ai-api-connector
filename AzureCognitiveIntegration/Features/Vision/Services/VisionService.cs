using System.Security.Cryptography;
using AzureCognitiveIntegration.Config;
using AzureCognitiveIntegration.Features.Vision.Models;
using AzureCognitiveIntegration.Helpers;
using AzureCognitiveIntegration.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AzureCognitiveIntegration.Features.Vision.Services;

/// <summary>
/// VisionService
/// </summary>
/// <param name="logger"></param>
public class VisionService(ILogger<VisionService> logger, IAzureServiceWorker serviceWorker, IConfiguration configuration) : IVisionService
{
    /// <summary>
    /// FacialComparisonAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<GenericResponse> FacialComparisonAsync(FacialComparisonRequest request)
    {
        const string recognitionModel4 = RecognitionModel.Recognition04;

        var stream1 = await StringHelper.GetStreamFromFormFile(request.Image1);
        var stream2 = await StringHelper.GetStreamFromFormFile(request.Image2);

        var configs = configuration.GetVisionSettings();
        if (configs.Mock)
        {
            logger.LogInformation("Mocking the response");
            var random = new Random();
            var confidence = Math.Round(random.NextDouble(), 2);
            var identical = random.Next(2) == 0;
            return new GenericResponse
            {
                Data = new ComparisonResults
                {
                    Identical = identical,
                    ConfidenceLevel = confidence
                },
                Success = identical
            };
        }
        var faceClient = serviceWorker.AuthenticatedClient();
        logger.LogInformation("Detection client is created- {FaceClient}", faceClient);
        var detectionFromImage1 = await serviceWorker.DetectedFaceRecognize(faceClient, stream1, recognitionModel4);
        var sourceId1 = detectionFromImage1[0].FaceId!.Value;

        var detectionFromImage2 = await serviceWorker.DetectedFaceRecognize(faceClient, stream2, recognitionModel4);
        var sourceId2 = detectionFromImage2[0].FaceId!.Value;

        var verifyResult = await faceClient.Face.VerifyFaceToFaceAsync(sourceId1, sourceId2);
        logger.LogInformation("Image 1 and Image 2 are: {result} with confidence level of {Confidence}", 
            verifyResult.IsIdentical ? "Identical" : "Different", verifyResult.Confidence);
        var result = new ComparisonResults
        {
            Identical = verifyResult.IsIdentical,
            ConfidenceLevel = verifyResult.Confidence
        };
        return new GenericResponse
        {
            Data = result
        };
    }
}