using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Reports.GetInventoryReport;
using NexusERP.Application.Reports.GetLowStockReport;
using NexusERP.Application.Reports.GetSalesReport;
using NexusERP.Application.Reports.GetPurchasingReport;
using System.Diagnostics;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class ReportQueries : IReportQueries
{
    private readonly ApplicationDbContext _context;

    public ReportQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // Inventory Reports
    // =====================================================

    public async Task<IReadOnlyCollection<GetInventoryReportResponse>>
        GetInventoryReportAsync(
            CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Join(
                _context.Inventories.AsNoTracking(),
                product => product.Id,
                inventory => inventory.ProductId,
                (product, inventory) =>
                    new GetInventoryReportResponse(
                        product.Id,
                        product.Sku.Value,
                        product.Name.Value,
                        inventory.Quantity.Value,
                        product.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<GetLowStockReportResponse>>
        GetLowStockReportAsync(
            int minimumStock,
            CancellationToken cancellationToken = default)
    {
        return await _context.Database
            .SqlQuery<GetLowStockReportResponse>(
                $"""
                SELECT
                    p.Id AS ProductId,
                    p.Sku,
                    p.Name AS ProductName,
                    i.Quantity,
                    p.IsActive
                FROM Products AS p
                INNER JOIN Inventory AS i
                    ON p.Id = i.ProductId
                WHERE i.Quantity <= {minimumStock}
                ORDER BY
                    i.Quantity ASC,
                    p.Name ASC
                """)
            .ToListAsync(cancellationToken);
    }

    // =====================================================
    // Sales Reports
    // =====================================================

    public async Task<IReadOnlyCollection<GetSalesReportResponse>>
    GetSalesReportAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default)
    {
        var fromDate = from?.ToDateTime(TimeOnly.MinValue);
        var toDate = to?.ToDateTime(TimeOnly.MaxValue);

        if (fromDate is null && toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
                SELECT
                    Id AS SalesOrderId,
                    CustomerId,
                    OrderDate,
                    CASE Status
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Confirmed'
                        WHEN 2 THEN 'Cancelled'
                        ELSE 'Unknown'
                    END AS Status,
                    Total
                FROM SalesOrders
                ORDER BY
                    OrderDate DESC
                """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null && toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
            SELECT
                Id AS SalesOrderId,
                CustomerId,
                OrderDate,
                CASE Status
                    WHEN 0 THEN 'Pending'
                    WHEN 1 THEN 'Confirmed'
                    WHEN 2 THEN 'Cancelled'
                    ELSE 'Unknown'
                END AS Status,
                Total
            FROM SalesOrders
            WHERE OrderDate >= {fromDate}
            ORDER BY
                OrderDate DESC
            """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is null && toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
            SELECT
                Id AS SalesOrderId,
                CustomerId,
                OrderDate,
                CASE Status
                    WHEN 0 THEN 'Pending'
                    WHEN 1 THEN 'Confirmed'
                    WHEN 2 THEN 'Cancelled'
                    ELSE 'Unknown'
                END AS Status,
                Total
            FROM SalesOrders
            WHERE OrderDate <= {toDate}
            ORDER BY
                OrderDate DESC
            """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null && toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
            SELECT
                Id AS SalesOrderId,
                CustomerId,
                OrderDate,
                CASE Status
                    WHEN 0 THEN 'Pending'
                    WHEN 1 THEN 'Confirmed'
                    WHEN 2 THEN 'Cancelled'
                    ELSE 'Unknown'
                END AS Status,
                Total
            FROM SalesOrders
            WHERE
                OrderDate >= {fromDate}
                AND OrderDate <= {toDate}
            ORDER BY
                OrderDate DESC
            """)
                .ToListAsync(cancellationToken);
        }

        throw new UnreachableException(
    "Unexpected sales report filter combination.");
    }

    // =====================================================
    // Purchasing Reports
    // =====================================================
    public async Task<IReadOnlyCollection<GetPurchasingReportResponse>>
    GetPurchasingReportAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default)
    {
        var fromDate = from?.ToDateTime(TimeOnly.MinValue);
        var toDate = to?.ToDateTime(TimeOnly.MaxValue);

        if (fromDate is null && toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                SELECT
                    Id AS PurchaseOrderId,
                    SupplierId,
                    OrderDate,
                    CASE Status
                        WHEN 1 THEN 'Pending'
                        WHEN 2 THEN 'Approved'
                        WHEN 3 THEN 'Cancelled'
                        ELSE 'Unknown'
                    END AS Status,
                    Total
                FROM PurchaseOrders
                ORDER BY
                    OrderDate DESC
                """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null && toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                SELECT
                    Id AS PurchaseOrderId,
                    SupplierId,
                    OrderDate,
                    CASE Status
                        WHEN 1 THEN 'Pending'
                        WHEN 2 THEN 'Approved'
                        WHEN 3 THEN 'Cancelled'
                        ELSE 'Unknown'
                    END AS Status,
                    Total
                FROM PurchaseOrders
                WHERE OrderDate >= {fromDate}
                ORDER BY
                    OrderDate DESC
                """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is null && toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                SELECT
                    Id AS PurchaseOrderId,
                    SupplierId,
                    OrderDate,
                    CASE Status
                        WHEN 1 THEN 'Pending'
                        WHEN 2 THEN 'Approved'
                        WHEN 3 THEN 'Cancelled'
                        ELSE 'Unknown'
                    END AS Status,
                    Total
                FROM PurchaseOrders
                WHERE OrderDate <= {toDate}
                ORDER BY
                    OrderDate DESC
                """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null && toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                SELECT
                    Id AS PurchaseOrderId,
                    SupplierId,
                    OrderDate,
                    CASE Status
                        WHEN 1 THEN 'Pending'
                        WHEN 2 THEN 'Approved'
                        WHEN 3 THEN 'Cancelled'
                        ELSE 'Unknown'
                    END AS Status,
                    Total
                FROM PurchaseOrders
                WHERE
                    OrderDate >= {fromDate}
                    AND OrderDate <= {toDate}
                ORDER BY
                    OrderDate DESC
                """)
                .ToListAsync(cancellationToken);
        }

        throw new UnreachableException(
            "Unexpected purchasing report filter combination.");
    }

}