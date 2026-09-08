public sealed class GetLowStockReportResponse
{
    public Guid ProductId { get; init; }

    public string Sku { get; init; } = string.Empty;

    public string ProductName { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public bool IsActive { get; init; }
}