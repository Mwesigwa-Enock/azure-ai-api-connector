namespace AzureCognitiveIntegration.Models;

/// <summary>
/// GenericResponse
/// </summary>
public class GenericResponse
{
    
    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    public object Data { get; set; } = default!;
}