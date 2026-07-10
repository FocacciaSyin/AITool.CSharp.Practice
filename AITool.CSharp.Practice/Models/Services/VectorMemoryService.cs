using AITool.CSharp.Practice.Models.VectorStore;
using Microsoft.Extensions.VectorData;

namespace AITool.CSharp.Practice.Models.Services;

public sealed class VectorMemoryService(
    EmbeddingService embeddingService,
    QdrantService qdrantService)
{
    public Task InitializeAsync(CancellationToken cancellationToken = default) =>
        qdrantService.EnsureCollectionExistsAsync(cancellationToken);

    public async Task SaveDocumentsAsync(
        IEnumerable<(Guid Id, string Content, string Description)> documents,
        CancellationToken cancellationToken = default)
    {
        var records = new List<QdrantDocument>();
        foreach (var document in documents)
        {
            records.Add(new QdrantDocument
            {
                Id = document.Id,
                Content = document.Content,
                Description = document.Description,
                Embedding = await embeddingService.GenerateEmbeddingAsync(document.Content, cancellationToken)
            });
        }

        await qdrantService.UpsertAsync(records, cancellationToken);
    }

    public async Task<IReadOnlyList<VectorSearchResult<QdrantDocument>>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var vector = await embeddingService.GenerateEmbeddingAsync(query, cancellationToken);
        var results = new List<VectorSearchResult<QdrantDocument>>();

        await foreach (var result in qdrantService.SearchAsync(vector, cancellationToken))
        {
            results.Add(result);
        }

        return results;
    }
}
