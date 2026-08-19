export interface SupplierDetail {
  id: string;
  name: string;
  taxIdentifier: string;
  email: string | null;
  phone: string | null;
  isActive: boolean;
}