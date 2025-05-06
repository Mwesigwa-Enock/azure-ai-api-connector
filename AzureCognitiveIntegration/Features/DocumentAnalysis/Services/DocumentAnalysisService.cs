using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using AzureCognitiveIntegration.Config;
using AzureCognitiveIntegration.Features.DocumentAnalysis.Models;
using AzureCognitiveIntegration.Helpers;
using Newtonsoft.Json;

namespace AzureCognitiveIntegration.Features.DocumentAnalysis.Services;

/// <summary>
/// DocumentAnalysisService
/// </summary>
public class DocumentAnalysisService(ILogger<DocumentAnalysisService> logger, IConfiguration configuration)
    : IDocumentAnalysisService
{
    
    
    /// <summary>
    /// GetNationalIdDetails
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<GenericResponse> GetNationalIdDetails(DocumentAnalysisRequest request)
    {
        var genericRes = new GenericResponse();
        if (request.Type == IdTypes.NationalIdFront)
        {
            var frontResults = await ExtractNationalIdFrontDetails(request.DocumentFile);
            genericRes.Data = frontResults;
        }
        else
        {
            var backResults = await ExtractNationalIdBackDetails(request.DocumentFile);
            genericRes.Data = backResults;
        }

        return genericRes;
    }
    
    
    /// <summary>
    /// ExtractNationalIdFrontDetails
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<NationalIdFrontDetails> ExtractNationalIdFrontDetails(IFormFile file)
    {
        var stream = await GetStreamFromFormFile(file);
        var results = await AnalyzeDocumentAsync(stream);

        foreach (var kvp in results.KeyValuePairs)
        {
            logger.LogInformation("Found Key:  {Key} with value:  {Value}", kvp.Key, kvp.Value.Content);
        }

        var pages = results.Pages;
        if (pages.Count <= 0)
        {
            logger.LogWarning("No Pages Found");
            throw new NotImplementedException();
        }

        var idFrontData = new NationalIdFrontDetails();
        logger.LogInformation("Found {Pages} Pages from the document", pages);
        foreach (var page in pages)
        {
            logger.LogInformation(
                "Document Page {page} has {lines} line(s), {Words} word(s) and {SelectionMarks} selection mark(s)",
                page, page.Lines.Count, page.Words.Count, page.SelectionMarks.Count);

            var line1 = page.Lines[3];
            var line2 = page.Lines[5];
            var line3 = page.Lines[8];
            var line4 = page.Lines[9];
            var line5 = page.Lines[11];
            var line6 = page.Lines[13];
            var line7 = page.Lines[17];

            idFrontData.Surname = line1.Content;
            idFrontData.GivenName = line2.Content;
            idFrontData.Nationality = line4.Content;
            idFrontData.Sex = line6.Content;
            idFrontData.Nin = line3.Content;
            idFrontData.DateOfBirth = line5.Content;
        }

        var serializedData = JsonConvert.SerializeObject(idFrontData);
        logger.LogInformation("Found data : {SerializedData}", serializedData);
        return idFrontData;
    }

 
    /// <summary>
    /// ExtractNationalIdBackDetails
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<NationalIdBackDetails> ExtractNationalIdBackDetails(IFormFile file)
    {
        var stream = await GetStreamFromFormFile(file);
        var results = await AnalyzeDocumentAsync(stream);

        return null;
    }
    
    /// <summary>
    /// AnalyzeDocument
    /// </summary>
    /// <param name="fileStream"></param>
    /// <returns></returns>
    public async Task<AnalyzeResult> AnalyzeDocumentAsync(Stream fileStream)
    {
        logger.LogInformation("Starting AnalyzeDocument with DocumentAnalysis Client ");
        var cognitiveSettings = configuration.GetCognitiveSettings();
        var endpoint = cognitiveSettings.Endpoint;
        var key = cognitiveSettings.ApiKey;
        logger.LogInformation("Cognitive endpoint {Endpoint} ", endpoint);
        var documentAnalysisClient = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));
        var operation = await documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed,
            "prebuilt-idDocument", fileStream);
        var result = operation.Value;
        logger.LogInformation("AnalyzeDocument with DocumentAnalysis Client completed");
        return result;
    }


    private static async Task<Stream> GetStreamFromFormFile(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
}