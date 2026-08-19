import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { SupplierForm } from "../components/SupplierForm/SupplierForm";
import { useRegisterSupplier } from "../hooks/useRegisterSupplier";

import type { RegisterSupplierRequest } from "../models/RegisterSupplierModel";
import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";

export function NewSupplierPage() {
  const navigate = useNavigate();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const registerSupplier =
    useRegisterSupplier();

  async function handleSubmit(
    request: RegisterSupplierRequest,
  ) {
    setSubmitError(null);

    try {
      await registerSupplier.mutateAsync(
        request,
      );

      navigate("/suppliers");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to create supplier.",
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
        New Supplier
      </h1>

      <p className="mt-2 text-slate-500">
        Register a new supplier in NexusERP.
      </p>

      <SupplierForm
        submitLabel="Create Supplier"
        isSubmitting={
          registerSupplier.isPending
        }
        errorMessage={submitError}
        onSubmit={handleSubmit}
        onCancel={() => {
          navigate("/suppliers");
        }}
      />
    </div>
  );
}