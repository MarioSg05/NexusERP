export interface UpdateSupplierRequest {
  email: string | null;
  phone: string | null;
}

export interface UpdateSupplierResponse {
  id: string;
  name: string;
  taxIdentifier: string;
  email: string | null;
  phone: string | null;
  isActive: boolean;
}