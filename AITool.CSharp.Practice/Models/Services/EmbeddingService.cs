using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Models.VectorStore;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.Google;

namespace AITool.CSharp.Practice.Models.Services;

public sealed class EmbeddingService : IDisposable
{
    private readonly GoogleAIEmbeddingGenerator _generator;
    private readonly int _dimensions;

    public EmbeddingService(GeminiSettings settings, int dimensions)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (string.IsNullOrWhiteSpace(settings.ApiKey) || settings.ApiKey == "透過 Secret Manager 設定")
        {
            throw new ArgumentException("Gemini API key 未透過 User Secrets 設定。");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(settings.EmbeddingModelId);

        if (dimensions != QdrantDocument.VectorDimensions)
        {
            throw new ArgumentException($"Embedding 維度必須為 {QdrantDocument.VectorDimensions}。");
        }

        _dimensions = dimensions;
        _generator = new GoogleAIEmbeddingGenerator(
            settings.EmbeddingModelId,
            settings.ApiKey,
            GoogleAIVersion.V1_Beta,
            httpClient: null,
            loggerFactory: null,
            dimensions: dimensions);
    }

    public async Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        var vector = await _generator.GenerateVectorAsync(text, cancellationToken: cancellationToken);

        if (vector.Length != _dimensions)
        {
            throw new InvalidOperationException($"Embedding 維度應為 {_dimensions}，實際為 {vector.Length}。");
        }

        return vector;
    }

    public void Dispose() => _generator.Dispose();
}
