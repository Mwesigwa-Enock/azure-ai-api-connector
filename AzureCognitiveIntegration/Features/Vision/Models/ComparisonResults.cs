namespace AzureCognitiveIntegration.Features.Vision.Models;

/// <summary>
/// ComparisonResults
/// </summary>
public class ComparisonResults
{
    /// <summary>
    /// Identical
    /// </summary>
    public bool Identical { get; set; }

    /// <summary>
    /// ConfidenceLevel
    /// </summary>
    public double ConfidenceLevel { get; set; }
}