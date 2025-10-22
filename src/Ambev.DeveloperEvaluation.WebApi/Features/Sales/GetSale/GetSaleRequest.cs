using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Request model for retrieving a sale
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// The unique sale number
    /// </summary>
    public int SaleNumber { get; set; }
}