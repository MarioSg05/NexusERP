import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

import { ProductForm } from "../components/ProductForm/ProductForm";
import { useRegisterProduct } from "../hooks/useRegisterProduct";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { RegisterProductRequest } from "../models/RegisterProductModel";

export function NewProductPage() {
  const navigate = useNavigate();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const registerProduct =
    useRegisterProduct();

  async function handleSubmit(
    request: RegisterProductRequest,
  ) {
    setSubmitError(null);

    try {
      await registerProduct.mutateAsync(request);

      navigate("/products");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to create product.",
        );

        return;
      }

      setSubmitError(
        "An unexpected error occurred.",
      );
    }
  }

  return (
    <div>
      <h1 className="text-3xl font-bold tracking-tight text-slate-900">
        New Product
      </h1>

      <p className="mt-2 text-slate-500">
        Register a new product in NexusERP.
      </p>

      <ProductForm
        isSubmitting={registerProduct.isPending}
        errorMessage={submitError}
        onSubmit={handleSubmit}
        onCancel={() => {
          navigate("/products");
        }}
      />
    </div>
  );
}