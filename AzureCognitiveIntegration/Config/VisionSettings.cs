namespace AzureCognitiveIntegration.Config;

/// <summary>
/// VisionSettings
/// </summary>
public class VisionSettings
{
    /// <summary>
    /// Endpoint
    /// </summary>
    public string Endpoint { get; set; }
    
    /// <summary>
    /// ApiKey
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Mock
    /// </summary>
    public bool Mock { get; set; }
}