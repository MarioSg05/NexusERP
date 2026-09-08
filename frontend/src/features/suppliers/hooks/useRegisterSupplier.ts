import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { registerSupplier } from "../services/supplierService";

export function useRegisterSupplier() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: registerSupplier,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["suppliers"],
      });
    },
  });
}