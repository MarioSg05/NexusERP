import axios from "axios";
import { useState } from "react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { SupplierForm } from "../components/SupplierForm/SupplierForm";

import { useActivateSupplier } from "../hooks/useActivateSupplier";
import { useDeactivateSupplier } from "../hooks/useDeactivateSupplier";
import { useSupplier } from "../hooks/useSupplier";
import { useUpdateSupplier } from "../hooks/useUpdateSupplier";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { SupplierFormSubmitValues } from "../components/SupplierForm/SupplierForm";

export function EditSupplierPage() {
  const navigate = useNavigate();
  const { id = "" } = useParams();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const [statusError, setStatusError] =
    useState<string | null>(null);

  const {
    data: supplier,
    isLoading,
    error,
    refetch,
  } = useSupplier(id);

  const updateSupplier =
    useUpdateSupplier();

  const activateSupplier =
    useActivateSupplier();

  const deactivateSupplier =
    useDeactivateSupplier();

  const isStatusPending =
    activateSupplier.isPending ||
    deactivateSupplier.isPending;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading supplier...
      </p>
    );
  }

  if (error || !supplier) {
    return (
      <QueryErrorState
        title="Unable to load supplier"
        description="We couldn't retrieve the supplier information."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  async function handleSubmit(
    values: SupplierFormSubmitValues,
  ) {
    setSubmitError(null);

    try {
      await updateSupplier.mutateAsync({
        id,
        request: {
          email: values.email,
          phone: values.phone,
        },
      });

      navigate("/suppliers");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to update supplier.",
        );

        return;
      }

      setSubmitError(
        "An unexpected error occurred.",
      );
    }
  }

  async function handleActivate() {
    setStatusError(null);

    try {
      await activateSupplier.mutateAsync(
        id,
      );
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setStatusError(
          error.response?.data.detail ??
            "Unable to activate supplier.",
        );

        return;
      }

      setStatusError(
        "An unexpected error occurred.",
      );
    }
  }

  async function handleDeactivate() {
    setStatusError(null);

    try {
      await deactivateSupplier.mutateAsync(
        id,
      );
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setStatusError(
          error.response?.data.detail ??
            "Unable to deactivate supplier.",
        );

        return;
      }

      setStatusError(
        "An unexpected error occurred.",
      );
    }
  }

  return (
    <div>
      <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-slate-900">
            Edit Supplier
          </h1>

          <p className="mt-2 text-slate-500">
            Update supplier contact
            information and account status.
          </p>
        </div>

        <span
          className={[
            "inline-flex w-fit rounded-full px-2.5 py-1 text-xs font-medium",
            supplier.isActive
              ? "bg-emerald-50 text-emerald-700"
              : "bg-red-50 text-red-700",
          ].join(" ")}
        >
          {supplier.isActive
            ? "Active"
            : "Inactive"}
        </span>
      </div>

      <SupplierForm
        initialValues={{
          name: supplier.name,
          taxIdentifier:
            supplier.taxIdentifier,
          email: supplier.email,
          phone: supplier.phone,
        }}
        lockIdentity
        submitLabel="Save Changes"
        isSubmitting={
          updateSupplier.isPending
        }
        errorMessage={submitError}
        onSubmit={handleSubmit}
        onCancel={() => {
          navigate("/suppliers");
        }}
      />

      <div className="mt-8 border-t border-slate-200 pt-8">
        <h2 className="text-lg font-semibold text-slate-900">
          Supplier Status
        </h2>

        <p className="mt-1 text-sm text-slate-500">
          Activate or deactivate this
          supplier.
        </p>

        {statusError && (
          <div
            role="alert"
            className="mt-4 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
          >
            {statusError}
          </div>
        )}

        <div className="mt-4">
          {supplier.isActive ? (
            <button
              type="button"
              onClick={() => {
                void handleDeactivate();
              }}
              disabled={isStatusPending}
              className="rounded-lg border border-red-200 bg-white px-4 py-2 text-sm font-medium text-red-700 transition-colors hover:bg-red-50 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {deactivateSupplier.isPending
                ? "Deactivating..."
                : "Deactivate Supplier"}
            </button>
          ) : (
            <button
              type="button"
              onClick={() => {
                void handleActivate();
              }}
              disabled={isStatusPending}
              className="rounded-lg bg-emerald-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-emerald-700 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {activateSupplier.isPending
                ? "Activating..."
                : "Activate Supplier"}
            </button>
          )}
        </div>
      </div>
    </div>
  );
}