import { useQuery } from "@tanstack/react-query";

import { getPurchaseOrderById } from "../services/purchasingService";

export function usePurchaseOrder(
  id: string,
) {
  return useQuery({
    queryKey: ["purchase-orders", id],
    queryFn: () =>
      getPurchaseOrderById(id),
    enabled: Boolean(id),
  });
}