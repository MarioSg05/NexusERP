import { useState } from "react";
import axios from "axios";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { ProductForm } from "../components/ProductForm/ProductForm";
import { useProduct } from "../hooks/useProduct";
import { useUpdateProduct } from "../hooks/useUpdateProduct";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { RegisterProductRequest } from "../models/RegisterProductModel";

export function EditProductPage() {
  const navigate = useNavigate();
  const { id = "" } = useParams();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const {
    data: product,
    isLoading,
    error,
  } = useProduct(id);

  const updateProduct =
    useUpdateProduct();

  if (isLoading) {
    return <p>Loading product...</p>;
  }

  if (error || !product) {
    return <p>Unable to load product.</p>;
  }

  async function handleSubmit(
    request: RegisterProductRequest,
  ) {
    setSubmitError(null);

    try {
      await updateProduct.mutateAsync({
        id,
        request: {
          name: request.name,
          price: request.price,
        },
      });

      navigate("/products");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to update product.",
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
        Edit Product
      </h1>

      <p className="mt-2 text-slate-500">
        Update product information and pricing.
      </p>

      <ProductForm
        initialValues={{
          name: product.name,
          sku: product.sku,
          price: product.price,
        }}
        isEditMode
        submitLabel="Save Changes"
        isSubmitting={updateProduct.isPending}
        errorMessage={submitError}
        onSubmit={handleSubmit}
        onCancel={() => {
          navigate("/products");
        }}
      />
    </div>
  );
}