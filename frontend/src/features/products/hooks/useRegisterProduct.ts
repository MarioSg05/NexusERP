import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { registerProduct } from "../services/productService";

export function useRegisterProduct() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: registerProduct,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["products"],
      });
    },
  });
}