import {
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";

import type { BusinessInsights } from "../models/BusinessInsightsModel";

import { generateBusinessInsights } from "../services/aiService";
import { businessInsightsQueryKey } from "./useBusinessInsights";

export function useGenerateBusinessInsights() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: generateBusinessInsights,

    onSuccess: (data) => {
      queryClient.setQueryData<BusinessInsights>(
        businessInsightsQueryKey,
        data,
      );
    },
  });
}