using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Dashboard.Widgets;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class DashboardQueries
    : IDashboardQueries
{
    private readonly ApplicationDbContext _context;

    public DashboardQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardInventoryWidget>
    GetInventoryWidgetAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Database
            .SqlQuery<DashboardInventoryWidget>(
                $"""
            SELECT
                (SELECT COUNT(*)
                    FROM Products) AS TotalProducts,

                (SELECT COUNT(*)
                    FROM Products
                    WHERE IsActive = 1) AS ActiveProducts,

                (SELECT COUNT(*)
                    FROM Inventory
                    WHERE Quantity <= 10) AS LowStockProducts
            """)
            .SingleAsync(cancellationToken);
    }

    public async Task<DashboardSalesWidget>
        GetSalesWidgetAsync(
            CancellationToken cancellationToken = default)
    {
        return await _context.Database
            .SqlQuery<DashboardSalesWidget>(
                $"""
            SELECT
                COUNT(*) AS TotalSalesOrders,

            ISNULL(
                SUM(
                  CASE
                     WHEN Status = 1 THEN 1
                     ELSE 0
               END),
             0) AS PendingSalesOrders,

                ISNULL(SUM(Total), 0) AS TotalSalesAmount
            FROM SalesOrders
            """)
            .SingleAsync(cancellationToken);
    }

    public async Task<DashboardPurchasingWidget>
    GetPurchasingWidgetAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Database
            .SqlQuery<DashboardPurchasingWidget>(
                $"""
            SELECT
                COUNT(*) AS TotalPurchaseOrders,

                ISNULL(
                    SUM(
                        CASE
                            WHEN Status = 1 THEN 1
                            ELSE 0
                        END),
                    0) AS PendingPurchaseOrders,

                ISNULL(SUM(Total), 0) AS TotalPurchasingAmount
            FROM PurchaseOrders
            """)
            .SingleAsync(cancellationToken);
    }
}