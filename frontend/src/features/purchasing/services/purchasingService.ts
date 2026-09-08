import { apiClient } from "../../../shared/api/client";

import type {
  CreatePurchaseOrderRequest,
  CreatePurchaseOrderResponse,
} from "../models/CreatePurchaseOrderModel";

import type { PurchaseOrderDetail } from "../models/PurchaseOrderDetailModel";
import type { PurchaseOrder } from "../models/PurchaseOrderModel";
import type { PurchaseOrderStatusResponse } from "../models/PurchaseOrderStatusModel";

export async function getPurchaseOrders(): Promise<PurchaseOrder[]> {
  const response =
    await apiClient.get<PurchaseOrder[]>(
      "/purchase-orders",
    );

  return response.data;
}

export async function getPurchaseOrderById(
  id: string,
): Promise<PurchaseOrderDetail> {
  const response =
    await apiClient.get<PurchaseOrderDetail>(
      `/purchase-orders/${id}`,
    );

  return response.data;
}

export async function createPurchaseOrder(
  request: CreatePurchaseOrderRequest,
): Promise<CreatePurchaseOrderResponse> {
  const response =
    await apiClient.post<CreatePurchaseOrderResponse>(
      "/purchase-orders",
      request,
    );

  return response.data;
}

export async function approvePurchaseOrder(
  id: string,
): Promise<PurchaseOrderStatusResponse> {
  const response =
    await apiClient.post<PurchaseOrderStatusResponse>(
      `/purchase-orders/${id}/approve`,
    );

  return response.data;
}

export async function cancelPurchaseOrder(
  id: string,
): Promise<PurchaseOrderStatusResponse> {
  const response =
    await apiClient.post<PurchaseOrderStatusResponse>(
      `/purchase-orders/${id}/cancel`,
    );

  return response.data;
}