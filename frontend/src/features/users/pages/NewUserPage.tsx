import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { UserForm } from "../components/UserForm/UserForm";
import { useRegisterUser } from "../hooks/useRegisterUser";

import type { RegisterUserRequest } from "../models/RegisterUserModel";
import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";

export function NewUserPage() {
  const navigate = useNavigate();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const registerUser =
    useRegisterUser();

  async function handleSubmit(
    request: RegisterUserRequest,
  ) {
    setSubmitError(null);

    try {
      await registerUser.mutateAsync(
        request,
      );

      navigate("/users");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to create user.",
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
        New User
      </h1>

      <p className="mt-2 text-slate-500">
        Create a NexusERP user and assign their initial role.
      </p>

      <UserForm
        includePassword
        submitLabel="Create User"
        isSubmitting={
          registerUser.isPending
        }
        errorMessage={submitError}
        onSubmit={handleSubmit}
        onCancel={() => {
          navigate("/users");
        }}
      />
    </div>
  );
}