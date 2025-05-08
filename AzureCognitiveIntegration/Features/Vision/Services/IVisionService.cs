using AzureCognitiveIntegration.Features.Vision.Models;
using AzureCognitiveIntegration.Models;

namespace AzureCognitiveIntegration.Features.Vision.Services;

/// <summary>
/// IVisionService
/// </summary>
public interface IVisionService
{
    /// <summary>
    /// FacialComparisonAsync
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GenericResponse> FacialComparisonAsync(FacialComparisonRequest request);
}