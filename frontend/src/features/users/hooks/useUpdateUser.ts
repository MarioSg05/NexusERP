import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import { updateUser } from "../services/userService";

import type { UpdateUserRequest } from "../models/UpdateUserModel";

interface UpdateUserVariables {
  id: string;
  request: UpdateUserRequest;
}

export function useUpdateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      request,
    }: UpdateUserVariables) =>
      updateUser(id, request),

    onSuccess: async (_, variables) => {
      await Promise.all([
        queryClient.invalidateQueries({
          queryKey: ["users"],
        }),

        queryClient.invalidateQueries({
          queryKey: [
            "users",
            variables.id,
          ],
        }),
      ]);
    },
  });
}