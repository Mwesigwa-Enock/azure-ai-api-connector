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
    private async Task<NationalIdFrontDetails> ExtractNationalIdFrontDetails(IFormFile file)
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

        var idFrontData = GetNationalIdFrontDetails(results);
        logger.LogInformation("Found {Pages} Pages from the document", pages);
        
        var serializedData = JsonConvert.SerializeObject(idFrontData);
        logger.LogInformation("Found data : {SerializedData}", serializedData);
        return idFrontData;
    }


    /// <summary>
    /// ExtractNationalIdBackDetails
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private async Task<NationalIdBackDetails> ExtractNationalIdBackDetails(IFormFile file)
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

        var idBackDetails = GetNationalIdBackDetails(results);
        logger.LogInformation("Found {Pages} Pages from the document", pages);
        
        var serializedData = JsonConvert.SerializeObject(idBackDetails);
        logger.LogInformation("Found data : {SerializedData}", serializedData);
        return idBackDetails;
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


    private NationalIdFrontDetails GetNationalIdFrontDetails(AnalyzeResult result)
    {
        var idFrontData = new NationalIdFrontDetails();
        foreach (var page in result.Pages)
        {
            logger.LogInformation(
                "Document Page {page} has {lines} line(s), {Words} word(s) and {SelectionMarks} selection mark(s)",
                page.PageNumber, page.Lines.Count, page.Words.Count, page.SelectionMarks.Count);
            for (var i = 0; i < page.Lines.Count; i++)
            {
                var line = page.Lines[i];
                logger.LogInformation("Line {Line} has content : {Content}", i, line.Content);
                switch (line.Content)
                {
                    case "SURNAME":
                        idFrontData.Surname = page.Lines[i + 1].Content;
                        break;
                    case "GIVEN NAME":
                        idFrontData.GivenName = page.Lines[i + 1].Content;
                        break;
                    case "NATIONALITY":
                        idFrontData.Nationality = page.Lines[i + 3].Content;
                        break;
                    case "SEX":
                        idFrontData.Sex = page.Lines[i + 3].Content;
                        break;
                    case "DATE OF BIRTH":
                        idFrontData.DateOfBirth = page.Lines[i + 3].Content;
                        break;
                    
                    case "NIN":
                        idFrontData.Nin = page.Lines[i + 2].Content;
                        break;
                   
                    case "CARD NO" or "CARD NO.":
                        idFrontData.CardNo = page.Lines[i + 2].Content;
                        break;
                    
                    case "DATE OF EXPIRY":
                        idFrontData.DateOfExpiry = page.Lines[i + 1].Content;
                        break;
                }
            }
        }
        logger.LogInformation("Found data : {SerializedData}", JsonConvert.SerializeObject(idFrontData));
        return idFrontData;
    }

    private NationalIdBackDetails GetNationalIdBackDetails(AnalyzeResult result)
    {
        var idBackData = new NationalIdBackDetails();
        foreach (var page in result.Pages)
        {
            logger.LogInformation(
                "Document Page {page} has {lines} line(s), {Words} word(s) and {SelectionMarks} selection mark(s)",
                page.PageNumber, page.Lines.Count, page.Words.Count, page.SelectionMarks.Count);
            
            for (var i = 0; i < page.Lines.Count; i++)
            {
                var line = page.Lines[i];
                logger.LogInformation("Line {Line} has content : {Content}", i, line.Content);

                switch (line.Content)
                {
                    case "VILLAGE:" or "VILLAGE":
                        idBackData.Village = page.Lines[i + 1].Content;
                        break;
                    case "PARISH:" or "PARISH":
                        idBackData.Parish = page.Lines[i + 1].Content;
                        break;
                    case "S.COUNTY:" or "S.COUNTY":
                        idBackData.SubCounty = page.Lines[i + 1].Content;
                        break;
                    case "COUNTY:" or "COUNTY":
                        idBackData.County = page.Lines[i + 1].Content;
                        break;
                    case "DISTRICT:" or "DISTRICT":
                        idBackData.District = page.Lines[i + 1].Content;
                        break;
                }
            }
        }
        return idBackData;
    }
}