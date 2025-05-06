using System.Net.Mime;
using AzureCognitiveIntegration.Core.Controllers;
using AzureCognitiveIntegration.Features.DocumentAnalysis.Models;
using AzureCognitiveIntegration.Features.DocumentAnalysis.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureCognitiveIntegration.Features.DocumentAnalysis.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/v1/documents")]
public class DocumentAnalysisController(IDocumentAnalysisService documentAnalysisService) : BaseController
{
    /// <summary>
    /// submitDocument
    /// </summary>
    /// <returns></returns>
    [HttpPost("analyze")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> SubmitDocument([FromForm] DocumentAnalysisRequest request)
    {
        if (request.DocumentFile.Length == 0)
        {
            return BadRequest("Invalid request data");
        }

        var results = await documentAnalysisService.GetNationalIdDetails(request);
        results.Success = true;
        return Ok(results);
    }
}