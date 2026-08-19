import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { registerUser } from "../services/userService";

export function useRegisterUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: registerUser,

    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["users"],
      });
    },
  });
}