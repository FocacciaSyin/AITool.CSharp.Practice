using Microsoft.Extensions.VectorData;

namespace AITool.CSharp.Practice.Models.VectorStore;

public sealed class QdrantDocument
{
    public const int VectorDimensions = 768;

    [VectorStoreKey]
    public Guid Id { get; set; }

    [VectorStoreData]
    public string Content { get; set; } = string.Empty;

    [VectorStoreData]
    public string Description { get; set; } = string.Empty;

    [VectorStoreVector(VectorDimensions, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
