import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { updateProduct } from "../services/productService";

import type { UpdateProductRequest } from "../models/UpdateProductModel";

interface UpdateProductVariables {
  id: string;
  request: UpdateProductRequest;
}

export function useUpdateProduct() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: UpdateProductVariables) =>
      updateProduct(id, request),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["products"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "products",
            variables.id,
          ],
        }),
      ]);
    },
  });
}