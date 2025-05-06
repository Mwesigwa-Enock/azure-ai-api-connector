using AzureCognitiveIntegration.Helpers;

namespace AzureCognitiveIntegration.Features.DocumentAnalysis.Models;

/// <summary>
/// DocumentAnalysisRequest
/// </summary>
public class DocumentAnalysisRequest
{
    /// <summary>
    /// Type
    /// </summary>
    public IdTypes Type { get; set; }
    
    /// <summary>
    /// DocumentFile
    /// </summary>
    public IFormFile DocumentFile { set; get; } = default!;
}