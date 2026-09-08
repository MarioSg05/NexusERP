import { apiClient } from "../../../shared/api/client";

import type { Supplier } from "../models/SupplierModel";
import type { SupplierDetail } from "../models/SupplierDetailModel";

import type {
  RegisterSupplierRequest,
  RegisterSupplierResponse,
} from "../models/RegisterSupplierModel";

import type {
  UpdateSupplierRequest,
  UpdateSupplierResponse,
} from "../models/UpdateSupplierModel";

import type { SupplierStatusResponse } from "../models/SupplierStatusModel";

export async function getSuppliers(): Promise<
  Supplier[]
> {
  const response =
    await apiClient.get<Supplier[]>(
      "/suppliers",
    );

  return response.data;
}

export async function getSupplierById(
  id: string,
): Promise<SupplierDetail> {
  const response =
    await apiClient.get<SupplierDetail>(
      `/suppliers/${id}`,
    );

  return response.data;
}

export async function registerSupplier(
  request: RegisterSupplierRequest,
): Promise<RegisterSupplierResponse> {
  const response =
    await apiClient.post<RegisterSupplierResponse>(
      "/suppliers",
      request,
    );

  return response.data;
}

export async function updateSupplier(
  id: string,
  request: UpdateSupplierRequest,
): Promise<UpdateSupplierResponse> {
  const response =
    await apiClient.put<UpdateSupplierResponse>(
      `/suppliers/${id}`,
      request,
    );

  return response.data;
}

export async function activateSupplier(
  id: string,
): Promise<SupplierStatusResponse> {
  const response =
    await apiClient.post<SupplierStatusResponse>(
      `/suppliers/${id}/activate`,
    );

  return response.data;
}

export async function deactivateSupplier(
  id: string,
): Promise<SupplierStatusResponse> {
  const response =
    await apiClient.post<SupplierStatusResponse>(
      `/suppliers/${id}/deactivate`,
    );

  return response.data;
}