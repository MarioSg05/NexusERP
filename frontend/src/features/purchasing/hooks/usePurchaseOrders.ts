import { useQuery } from "@tanstack/react-query";

import { getPurchaseOrders } from "../services/purchasingService";

export function usePurchaseOrders() {
  return useQuery({
    queryKey: ["purchase-orders"],
    queryFn: getPurchaseOrders,
  });
}