using AITool.CSharp.Practice.Models.Settings;
using AITool.CSharp.Practice.Models.VectorStore;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;

namespace AITool.CSharp.Practice.Models.Services;

public sealed class QdrantService : IDisposable
{
    private readonly QdrantClient _client;
    private readonly QdrantCollection<Guid, QdrantDocument> _collection;
    private readonly int _searchLimit;

    public QdrantService(QdrantSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Endpoint);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.CollectionName);

        if (settings.VectorSize != QdrantDocument.VectorDimensions)
        {
            throw new ArgumentException($"Qdrant VectorSize 必須為 {QdrantDocument.VectorDimensions}。");
        }

        if (settings.SearchLimit <= 0 || settings.TimeoutSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(settings));
        }

        _searchLimit = settings.SearchLimit;
        _client = new QdrantClient(
            new Uri(settings.Endpoint),
            settings.ApiKey ?? string.Empty,
            TimeSpan.FromSeconds(settings.TimeoutSeconds),
            loggerFactory: null);
        _collection = new QdrantCollection<Guid, QdrantDocument>(
            _client,
            settings.CollectionName,
            ownsClient: false,
            options: null);
    }

    public async Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default)
    {
        await _client.HealthAsync(cancellationToken);
        await _collection.EnsureCollectionExistsAsync(cancellationToken);
    }

    public Task UpsertAsync(
        IEnumerable<QdrantDocument> documents,
        CancellationToken cancellationToken = default) =>
        _collection.UpsertAsync(documents, cancellationToken);

    public IAsyncEnumerable<VectorSearchResult<QdrantDocument>> SearchAsync(
        ReadOnlyMemory<float> vector,
        CancellationToken cancellationToken = default) =>
        _collection.SearchAsync(vector, _searchLimit, options: null, cancellationToken);

    public void Dispose()
    {
        _collection.Dispose();
        _client.Dispose();
    }
}
