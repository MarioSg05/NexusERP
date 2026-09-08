import { apiClient } from "../../../shared/api/client";

import type { BusinessInsights } from "../models/BusinessInsightsModel";

export async function generateBusinessInsights(): Promise<BusinessInsights> {
  const response =
    await apiClient.post<BusinessInsights>(
      "/ai/business-insights",
    );

  return response.data;
}