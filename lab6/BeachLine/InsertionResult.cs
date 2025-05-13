using lab6.BeachLine.Nodes;

namespace lab6.BeachLine;

public abstract record InsertionResult(LeafBeachNode? NewLeaf)
{
    public sealed record NewLeafOnly(LeafBeachNode? NewLeaf) : InsertionResult(NewLeaf);

    public sealed record SplitOccurred(LeafBeachNode SplitLeaf, LeafBeachNode? NewLeaf)
        : InsertionResult(NewLeaf);
}