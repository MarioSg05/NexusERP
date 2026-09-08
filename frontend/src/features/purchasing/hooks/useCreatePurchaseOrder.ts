import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { createPurchaseOrder } from "../services/purchasingService";

export function useCreatePurchaseOrder() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createPurchaseOrder,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["purchase-orders"],
      });
    },
  });
}