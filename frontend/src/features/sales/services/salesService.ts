import { apiClient } from "../../../shared/api/client";

import type {
  CreateSalesOrderRequest,
  CreateSalesOrderResponse,
} from "../models/CreateSalesOrderModel";

import type { SalesOrderDetail } from "../models/SalesOrderDetailModel";
import type { SalesOrder } from "../models/SalesOrderModel";
import type { SalesOrderStatusResponse } from "../models/SalesOrderStatusModel";

export async function getSalesOrders(): Promise<SalesOrder[]> {
  const response =
    await apiClient.get<SalesOrder[]>(
      "/sales-orders",
    );

  return response.data;
}

export async function getSalesOrderById(
  id: string,
): Promise<SalesOrderDetail> {
  const response =
    await apiClient.get<SalesOrderDetail>(
      `/sales-orders/${id}`,
    );

  return response.data;
}

export async function createSalesOrder(
  request: CreateSalesOrderRequest,
): Promise<CreateSalesOrderResponse> {
  const response =
    await apiClient.post<CreateSalesOrderResponse>(
      "/sales-orders",
      request,
    );

  return response.data;
}

export async function confirmSalesOrder(
  id: string,
): Promise<SalesOrderStatusResponse> {
  const response =
    await apiClient.post<SalesOrderStatusResponse>(
      `/sales-orders/${id}/confirm`,
    );

  return response.data;
}

export async function cancelSalesOrder(
  id: string,
): Promise<SalesOrderStatusResponse> {
  const response =
    await apiClient.post<SalesOrderStatusResponse>(
      `/sales-orders/${id}/cancel`,
    );

  return response.data;
}