using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Reports.GetInventoryReport;
using NexusERP.Application.Reports.GetLowStockReport;
using NexusERP.Application.Reports.GetPurchasingReport;
using NexusERP.Application.Reports.GetSalesReport;

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
        var fromDate =
            from?.ToDateTime(TimeOnly.MinValue);

        var toDate =
            to?.ToDateTime(TimeOnly.MaxValue);

        if (fromDate is null &&
            toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
                    SELECT
                        so.Id AS SalesOrderId,
                        so.CustomerId,
                        c.Name AS CustomerName,
                        so.OrderDate,
                        CASE so.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Confirmed'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        so.Total
                    FROM SalesOrders AS so
                    INNER JOIN Customers AS c
                        ON so.CustomerId = c.Id
                    ORDER BY
                        so.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null &&
            toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
                    SELECT
                        so.Id AS SalesOrderId,
                        so.CustomerId,
                        c.Name AS CustomerName,
                        so.OrderDate,
                        CASE so.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Confirmed'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        so.Total
                    FROM SalesOrders AS so
                    INNER JOIN Customers AS c
                        ON so.CustomerId = c.Id
                    WHERE
                        so.OrderDate >= {fromDate}
                    ORDER BY
                        so.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is null &&
            toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
                    SELECT
                        so.Id AS SalesOrderId,
                        so.CustomerId,
                        c.Name AS CustomerName,
                        so.OrderDate,
                        CASE so.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Confirmed'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        so.Total
                    FROM SalesOrders AS so
                    INNER JOIN Customers AS c
                        ON so.CustomerId = c.Id
                    WHERE
                        so.OrderDate <= {toDate}
                    ORDER BY
                        so.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null &&
            toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetSalesReportResponse>(
                    $"""
                    SELECT
                        so.Id AS SalesOrderId,
                        so.CustomerId,
                        c.Name AS CustomerName,
                        so.OrderDate,
                        CASE so.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Confirmed'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        so.Total
                    FROM SalesOrders AS so
                    INNER JOIN Customers AS c
                        ON so.CustomerId = c.Id
                    WHERE
                        so.OrderDate >= {fromDate}
                        AND so.OrderDate <= {toDate}
                    ORDER BY
                        so.OrderDate DESC
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
        var fromDate =
            from?.ToDateTime(TimeOnly.MinValue);

        var toDate =
            to?.ToDateTime(TimeOnly.MaxValue);

        if (fromDate is null &&
            toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                    SELECT
                        po.Id AS PurchaseOrderId,
                        po.SupplierId,
                        s.Name AS SupplierName,
                        po.OrderDate,
                        CASE po.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Approved'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        po.Total
                    FROM PurchaseOrders AS po
                    INNER JOIN Suppliers AS s
                        ON po.SupplierId = s.Id
                    ORDER BY
                        po.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null &&
            toDate is null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                    SELECT
                        po.Id AS PurchaseOrderId,
                        po.SupplierId,
                        s.Name AS SupplierName,
                        po.OrderDate,
                        CASE po.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Approved'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        po.Total
                    FROM PurchaseOrders AS po
                    INNER JOIN Suppliers AS s
                        ON po.SupplierId = s.Id
                    WHERE
                        po.OrderDate >= {fromDate}
                    ORDER BY
                        po.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is null &&
            toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                    SELECT
                        po.Id AS PurchaseOrderId,
                        po.SupplierId,
                        s.Name AS SupplierName,
                        po.OrderDate,
                        CASE po.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Approved'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        po.Total
                    FROM PurchaseOrders AS po
                    INNER JOIN Suppliers AS s
                        ON po.SupplierId = s.Id
                    WHERE
                        po.OrderDate <= {toDate}
                    ORDER BY
                        po.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        if (fromDate is not null &&
            toDate is not null)
        {
            return await _context.Database
                .SqlQuery<GetPurchasingReportResponse>(
                    $"""
                    SELECT
                        po.Id AS PurchaseOrderId,
                        po.SupplierId,
                        s.Name AS SupplierName,
                        po.OrderDate,
                        CASE po.Status
                            WHEN 1 THEN 'Pending'
                            WHEN 2 THEN 'Approved'
                            WHEN 3 THEN 'Cancelled'
                            ELSE 'Unknown'
                        END AS Status,
                        po.Total
                    FROM PurchaseOrders AS po
                    INNER JOIN Suppliers AS s
                        ON po.SupplierId = s.Id
                    WHERE
                        po.OrderDate >= {fromDate}
                        AND po.OrderDate <= {toDate}
                    ORDER BY
                        po.OrderDate DESC
                    """)
                .ToListAsync(cancellationToken);
        }

        throw new UnreachableException(
            "Unexpected purchasing report filter combination.");
    }
}