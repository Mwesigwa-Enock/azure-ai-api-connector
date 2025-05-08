using System.Net.Mime;
using AzureCognitiveIntegration.Core.Controllers;
using AzureCognitiveIntegration.Features.Vision.Models;
using AzureCognitiveIntegration.Features.Vision.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureCognitiveIntegration.Features.Vision.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/v1/vision")]
public class VisionController(IVisionService visionService) : BaseController
{
    
    /// <summary>
    /// FaceComparison
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("facial-comparison")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> FaceComparison([FromForm] FacialComparisonRequest request)
    {
        if (request.Image1.Length == 0 || request.Image2.Length == 0)
        {
            return BadRequest("Invalid request data, Image1 or Image2 is missing");
        }
        var result = await visionService.FacialComparisonAsync(request);
        return Ok(result);
    }
    
    
}