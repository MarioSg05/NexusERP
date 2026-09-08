import { useState } from "react";
import axios from "axios";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { CustomerForm } from "../components/CustomerForm/CustomerForm";
import { useCustomer } from "../hooks/useCustomer";
import { useUpdateCustomer } from "../hooks/useUpdateCustomer";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type {
  CustomerType,
  RegisterCustomerRequest,
} from "../models/RegisterCustomerModel";

export function EditCustomerPage() {
  const navigate = useNavigate();
  const { id = "" } = useParams();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const {
    data: customer,
    isLoading,
    error,
  } = useCustomer(id);

  const updateCustomer =
    useUpdateCustomer();

  if (isLoading) {
    return <p>Loading customer...</p>;
  }

  if (error || !customer) {
    return <p>Unable to load customer.</p>;
  }

  const customerType: CustomerType =
    customer.type === "Corporate"
      ? 2
      : 1;

  async function handleSubmit(
    request: RegisterCustomerRequest,
  ) {
    setSubmitError(null);

    try {
      await updateCustomer.mutateAsync({
        id,
        request,
      });

      navigate("/customers");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to update customer.",
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
        Edit Customer
      </h1>

      <p className="mt-2 text-slate-500">
        Update customer information.
      </p>

      <CustomerForm
        initialValues={{
          name: customer.name,
          email: customer.email,
          phone: customer.phone ?? "",
          type: customerType,
        }}
        submitLabel="Save Changes"
        isSubmitting={updateCustomer.isPending}
        errorMessage={submitError}
        onSubmit={handleSubmit}
        onCancel={() => {
          navigate("/customers");
        }}
      />
    </div>
  );
}