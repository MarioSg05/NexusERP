export interface DashboardInventory {
  totalProducts: number;
  activeProducts: number;
  lowStockProducts: number;
}

export interface DashboardSales {
  totalSalesOrders: number;
  pendingSalesOrders: number;
  totalSalesAmount: number;
}

export interface DashboardPurchasing {
  totalPurchaseOrders: number;
  pendingPurchaseOrders: number;
  totalPurchasingAmount: number;
}

export interface Dashboard {
  inventory: DashboardInventory;
  sales: DashboardSales;
  purchasing: DashboardPurchasing;
}