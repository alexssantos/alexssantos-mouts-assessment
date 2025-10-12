using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Response model for sale creation
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// The unique identifier of the created sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number of the created sale
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
    /// The items in the sale
    /// </summary>
    public required ICollection<SaleItem> Items { get; set; }
}