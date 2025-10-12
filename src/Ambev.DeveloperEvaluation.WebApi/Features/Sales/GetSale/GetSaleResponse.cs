using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Response model for sale retrieval
/// </summary>
public class GetSaleResponse
{
    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number
    /// </summary>
    public int SaleNumber { get; set; }

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
    /// The total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The status of the sale
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The items in the sale
    /// </summary>
    public required ICollection<SaleItem> Items { get; set; }
}