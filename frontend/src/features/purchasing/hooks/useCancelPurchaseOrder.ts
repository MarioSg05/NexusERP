import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { cancelPurchaseOrder } from "../services/purchasingService";

export function useCancelPurchaseOrder() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: cancelPurchaseOrder,

    onSuccess: async (_, id) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["purchase-orders"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "purchase-orders",
            id,
          ],
        }),
      ]);
    },
  });
}