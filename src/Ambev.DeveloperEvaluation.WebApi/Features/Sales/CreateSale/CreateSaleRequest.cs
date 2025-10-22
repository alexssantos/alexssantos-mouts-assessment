using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request model for creating a new sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// The customer identifier
    /// </summary>
    public required string CustomerId { get; set; }

    /// <summary>
    /// The branch identifier
    /// </summary>
    public required string BranchId { get; set; }

    /// <summary>
    /// The sale date
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The items to be included in the sale
    /// </summary>
    public required ICollection<CreateSaleItemRequest> Items { get; set; }
}

/// <summary>
/// Request model for creating a sale item
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// The product identifier
    /// </summary>
    public required string ProductId { get; set; }

    /// <summary>
    /// The quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
}