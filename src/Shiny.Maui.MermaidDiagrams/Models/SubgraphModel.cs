namespace Shiny.Maui.MermaidDiagrams.Models;

public sealed class SubgraphModel
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public List<string> NodeIds { get; } = [];
    public string? ParentSubgraphId { get; set; }
}