import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { activateSupplier } from "../services/supplierService";

export function useActivateSupplier() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: activateSupplier,

    onSuccess: async (_, id) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["suppliers"],
        }),

        queryClient.invalidateQueries({
          queryKey: ["suppliers", id],
        }),
      ]);
    },
  });
}