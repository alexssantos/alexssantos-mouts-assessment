namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Request model for cancelling a sale
/// </summary>
public class CancelSaleRequest
{
    /// <summary>
    /// The sale number to cancel
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// The reason for cancellation
    /// </summary>
    public required string CancellationReason { get; set; }
}