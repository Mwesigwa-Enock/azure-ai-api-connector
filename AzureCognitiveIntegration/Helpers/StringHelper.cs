namespace AzureCognitiveIntegration.Helpers;

/// <summary>
/// StringHelper
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// GetStreamFromFormFile
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static async Task<Stream> GetStreamFromFormFile(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
}