using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace AzureCognitiveIntegration.Tests.CognitiveTests;

[TestClass]
public class DocumentAnalysisIntegrationTests
{
    private DocumentAnalysisClient _client;

    [TestInitialize]
    public void Init ()
    {
        //TODO - Read this from the appsettings file
        const string key = "api-key";
        const string endpoint = "document-analysis-endpoint";
        
        var credentials = new AzureKeyCredential(key);
        _client = new DocumentAnalysisClient(new Uri(endpoint), credentials);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task AnalyzeFrontIdDocument_ReturnsResultAsync()
    {
        var testImage = "TestImages/Mubiru_ID.jpg";
        await using var stream = File.OpenRead(testImage);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);
        var result = operation.Value;
        
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Pages.Count > 0);
    }
    


    [TestMethod]
    public async Task AnalyzeBackIdDocument_ReturnsResultAsync()
    {
        var testImage = "TestImages/national-idback.jpg";
        await using var stream = File.OpenRead(testImage);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);
        var result = operation.Value;
        
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Pages.Count > 0);
    }

    
    [TestMethod]
    public async Task AnalyzeFrontIdContentDetailsAsync()
    {
        var testImage = "TestImages/Mubiru_ID.jpg";
        await using var stream = File.OpenRead(testImage);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);
        var result = operation.Value;
        var allText = string.Join(" ", result.Pages.SelectMany(p => p.Lines).Select(l => l.Content));
        Assert.IsTrue(allText.Contains("SURNAME"));
        Assert.IsTrue(allText.Contains("GIVEN NAME"));
        Assert.IsTrue(allText.Contains("DATE OF BIRTH"));
    }
    
    [TestMethod]
    public async Task AnalyzeBackIdContentDetailsAsync()
    {
        var testImage = "TestImages/national-idback.jpg";
        await using var stream = File.OpenRead(testImage);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);
        var result = operation.Value;
        var allText = string.Join(" ", result.Pages.SelectMany(p => p.Lines).Select(l => l.Content));
        Assert.IsTrue(allText.Contains("DISTRICT"));
        Assert.IsTrue(allText.Contains("S.COUNTY"));
        Assert.IsTrue(allText.Contains("VILLAGE"));
    }
    
    [TestMethod]
    public async Task AnalyzeBackIdContent_DetailsAsync()
    {
        var testImage = "TestImages/national-idback.jpg";
        await using var stream = File.OpenRead(testImage);
        var operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-idDocument", stream);
        var result = operation.Value;
        var allText = string.Join(" ", result.Pages.SelectMany(p => p.Lines).Select(l => l.Content));
        Assert.IsTrue(allText.Contains("PARISH:"));
        Assert.IsTrue(allText.Contains("COUNTY:"));
    }
}