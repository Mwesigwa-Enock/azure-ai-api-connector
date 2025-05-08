namespace AzureCognitiveIntegration.Config;

/// <summary>
/// ConfigExtensions
/// </summary>
public static class ConfigExtensions
{
    /// <summary>
    /// GetSwaggerSettings
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static SwaggerSettings GetSwaggerSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("Swagger").Get<SwaggerSettings>();
    }

    /// <summary>
    /// GetCognitiveSettings
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static CognitiveSettings GetCognitiveSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("Cognitive").Get<CognitiveSettings>();
    }
    
    
    /// <summary>
    /// GetVisionSettings
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static VisionSettings GetVisionSettings(this IConfiguration configuration)
    {
        return configuration.GetSection("Vision").Get<VisionSettings>();
    }
}