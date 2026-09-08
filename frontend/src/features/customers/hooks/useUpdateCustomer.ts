import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { updateCustomer } from "../services/customerService";

import type { UpdateCustomerRequest } from "../models/UpdateCustomerModel";

interface UpdateCustomerVariables {
  id: string;
  request: UpdateCustomerRequest;
}

export function useUpdateCustomer() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: UpdateCustomerVariables) =>
      updateCustomer(id, request),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["customers"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "customers",
            variables.id,
          ],
        }),
      ]);
    },
  });
}