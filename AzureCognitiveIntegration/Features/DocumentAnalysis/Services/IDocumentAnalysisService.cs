using Azure.AI.FormRecognizer.DocumentAnalysis;
using AzureCognitiveIntegration.Features.DocumentAnalysis.Models;

namespace AzureCognitiveIntegration.Features.DocumentAnalysis.Services;

/// <summary>
/// 
/// </summary>
public interface IDocumentAnalysisService
{
   
    
    /// <summary>
    /// GetNationalIdDetails
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GenericResponse> GetNationalIdDetails(DocumentAnalysisRequest request);
    
    
    /// <summary>
    /// AnalyzeDocument
    /// </summary>
    /// <param name="fileStream"></param>
    /// <returns></returns>
    Task<AnalyzeResult> AnalyzeDocumentAsync(Stream fileStream);
}