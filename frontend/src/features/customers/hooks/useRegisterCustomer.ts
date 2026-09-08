import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { registerCustomer } from "../services/customerService";

export function useRegisterCustomer() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: registerCustomer,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["customers"],
      });
    },
  });
}