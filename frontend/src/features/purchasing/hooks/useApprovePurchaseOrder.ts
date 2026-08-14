import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { approvePurchaseOrder } from "../services/purchasingService";

export function useApprovePurchaseOrder() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: approvePurchaseOrder,

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