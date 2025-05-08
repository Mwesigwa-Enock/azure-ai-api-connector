namespace AzureCognitiveIntegration.Features.Vision.Models;

/// <summary>
/// VisionRequest
/// </summary> 
public class FacialComparisonRequest
{
    /// <summary>
    /// Image1
    /// </summary>
    public IFormFile Image1 { get; set; } = default!;
    
    /// <summary>
    /// Image2
    /// </summary>
    public IFormFile Image2 { set; get; } = default!;
}