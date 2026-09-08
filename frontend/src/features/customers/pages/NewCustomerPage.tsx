import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

import { CustomerForm } from "../components/CustomerForm/CustomerForm";
import { useRegisterCustomer } from "../hooks/useRegisterCustomer";

import type { RegisterCustomerRequest } from "../models/RegisterCustomerModel";
import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";

export function NewCustomerPage() {
    const navigate = useNavigate();

    const [submitError, setSubmitError] = useState<string | null>(null);

    const registerCustomer = useRegisterCustomer();

    async function handleSubmit(request: RegisterCustomerRequest) {
        setSubmitError(null);

        try {
            await registerCustomer.mutateAsync(request);

            navigate("/customers");
        } catch (error) {
            if (axios.isAxiosError<ApiProblemDetails>(error)) {
                setSubmitError(
                    error.response?.data.detail ?? "Unable to create customer.",
                );

                return;
            }

            setSubmitError("An unexpected error occurred.");
        }
    }

    return (
        <div>
            <h1 className="text-3xl font-bold tracking-tight text-slate-900">
                New Customer
            </h1>

            <p className="mt-2 text-slate-500">
                Register a new customer in NexusERP.
            </p>

            <CustomerForm
                isSubmitting={registerCustomer.isPending}
                errorMessage={submitError}
                onSubmit={handleSubmit}
                onCancel={() => {
                    navigate("/customers");
                }}
            />
        </div>
    );
}
