import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { updateSupplier } from "../services/supplierService";

import type { UpdateSupplierRequest } from "../models/UpdateSupplierModel";

interface UpdateSupplierVariables {
  id: string;
  request: UpdateSupplierRequest;
}

export function useUpdateSupplier() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: UpdateSupplierVariables) =>
      updateSupplier(id, request),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["suppliers"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "suppliers",
            variables.id,
          ],
        }),
      ]);
    },
  });
}