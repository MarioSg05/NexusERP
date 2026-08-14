import { apiClient } from "../../../shared/api/client";

import type { Supplier } from "../models/SupplierModel";

export async function getSuppliers(): Promise<Supplier[]> {
  const response =
    await apiClient.get<Supplier[]>("/suppliers");

  return response.data;
}