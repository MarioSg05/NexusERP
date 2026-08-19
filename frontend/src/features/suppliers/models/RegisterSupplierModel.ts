export interface RegisterSupplierRequest {
  name: string;
  taxIdentifier: string;
  email: string | null;
  phone: string | null;
}

export interface RegisterSupplierResponse {
  id: string;
  taxIdentifier: string;
}