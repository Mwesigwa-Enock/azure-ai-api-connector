using AzureCognitiveIntegration.Config;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AzureCognitiveIntegration.Features.Vision.Services;

/// <summary>
/// IAzureServiceWorker
/// </summary>
public interface IAzureServiceWorker
{
    /// <summary>
    /// AuthenticatedClient
    /// </summary>
    /// <returns></returns>
    IFaceClient AuthenticatedClient();


    /// <summary>
    /// DetectedFaceRecognize
    /// </summary>
    /// <param name="faceClient"></param>
    /// <param name="fileStream"></param>
    /// <param name="recognitionModel"></param>
    /// <returns></returns>
    Task<List<DetectedFace>> DetectedFaceRecognize(IFaceClient faceClient, Stream fileStream, string recognitionModel);
}

/// <summary>
/// AzureServiceWorker
/// </summary>
public class AzureServiceWorker(ILogger<AzureServiceWorker> logger,  IConfiguration configuration) : IAzureServiceWorker
{
    
    /// <summary>
    /// AuthenticatedClient
    /// </summary>
    /// <returns></returns>
    public IFaceClient AuthenticatedClient()
    {
        logger.LogInformation("Authenticating Client....");
        var visionSettings = configuration.GetVisionSettings();
        var endpoint = visionSettings.Endpoint;
        var key = visionSettings.ApiKey;
        logger.LogInformation("Endpoint: {Endpoint} key {Key}", endpoint, key);
        var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(key))
        {
            Endpoint = endpoint
        };
        return faceClient;
    }


    /// <summary>
    /// DetectedFaceRecognize
    /// </summary>
    /// <param name="faceClient"></param>
    /// <param name="fileStream"></param>
    /// <param name="recognitionModel"></param>
    /// <returns></returns>
    public async Task<List<DetectedFace>> DetectedFaceRecognize(IFaceClient faceClient, Stream fileStream,
        string recognitionModel)
    {
        logger.LogInformation("Detect Face from Image....");
        var detectedFaces = await  faceClient.Face.DetectWithStreamAsync(fileStream,
            detectionModel: DetectionModel.Detection03,
            returnFaceAttributes: new List<FaceAttributeType>
            {
                FaceAttributeType.QualityForRecognition,
                FaceAttributeType.Age,
                FaceAttributeType.Gender,
                FaceAttributeType.Smile
            });

        var sufficientFaces = new List<DetectedFace>();
        foreach (var face in detectedFaces)
        {
            var faceQualityForRecognition = face.FaceAttributes.QualityForRecognition;
            if (faceQualityForRecognition is >= QualityForRecognition.Medium)
            {
                sufficientFaces.Add(face);
            }
        }
        logger.LogInformation(
            "Detected {DetectedFaces} Face from Image with {sufficientQuality} having sufficient quality",
            detectedFaces.Count, sufficientFaces.Count);
        return sufficientFaces;
    }
}