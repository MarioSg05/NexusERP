import { apiClient } from "../../../shared/api/client";

import type { InventoryItem } from "../models/InventoryModel";

import type {
  CreateInventoryRequest,
  CreateInventoryResponse,
} from "../models/CreateInventoryModel";

import type {
  InventoryStockRequest,
  InventoryStockResponse,
} from "../models/InventoryStockModel";

export async function getInventory(): Promise<InventoryItem[]> {
  const response =
    await apiClient.get<InventoryItem[]>("/inventory");

  return response.data;
}

export async function createInventory(
  request: CreateInventoryRequest,
): Promise<CreateInventoryResponse> {
  const response =
    await apiClient.post<CreateInventoryResponse>(
      "/inventory",
      request,
    );

  return response.data;
}

export async function increaseInventoryStock(
  id: string,
  request: InventoryStockRequest,
): Promise<InventoryStockResponse> {
  const response =
    await apiClient.post<InventoryStockResponse>(
      `/inventory/${id}/increase`,
      request,
    );

  return response.data;
}

export async function decreaseInventoryStock(
  id: string,
  request: InventoryStockRequest,
): Promise<InventoryStockResponse> {
  const response =
    await apiClient.post<InventoryStockResponse>(
      `/inventory/${id}/decrease`,
      request,
    );

  return response.data;
}

export async function adjustInventoryStock(
  id: string,
  request: InventoryStockRequest,
): Promise<InventoryStockResponse> {
  const response =
    await apiClient.put<InventoryStockResponse>(
      `/inventory/${id}/adjust`,
      request,
    );

  return response.data;
}