import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { deactivateSupplier } from "../services/supplierService";

export function useDeactivateSupplier() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: deactivateSupplier,

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