import axios from "axios";
import { useState } from "react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { UserForm } from "../components/UserForm/UserForm";

import { useActivateUser } from "../hooks/useActivateUser";
import { useChangeUserRole } from "../hooks/useChangeUserRole";
import { useDeactivateUser } from "../hooks/useDeactivateUser";
import { useUpdateUser } from "../hooks/useUpdateUser";
import { useUser } from "../hooks/useUser";

import { useAuth } from "../../auth/hooks/useAuth";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { UserFormSubmitValues } from "../components/UserForm/UserForm";

export function EditUserPage() {
  const navigate = useNavigate();
  const { id = "" } = useParams();

  const { user: currentUser } =
    useAuth();

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const [statusError, setStatusError] =
    useState<string | null>(null);

  const {
    data: user,
    isLoading,
    error,
    refetch,
  } = useUser(id);

  const updateUser = useUpdateUser();
  const changeUserRole =
    useChangeUserRole();
  const activateUser =
    useActivateUser();
  const deactivateUser =
    useDeactivateUser();

  const isSubmitting =
    updateUser.isPending ||
    changeUserRole.isPending;

  const isStatusPending =
    activateUser.isPending ||
    deactivateUser.isPending;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading user...
      </p>
    );
  }

  if (error || !user) {
    return (
      <QueryErrorState
        title="Unable to load user"
        description="We couldn't retrieve the user information."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

const isCurrentUser =
  currentUser?.userId === user.id;

const initialRole = user.role;

  async function handleSubmit(
    values: UserFormSubmitValues,
  ) {
    setSubmitError(null);

    try {
      await updateUser.mutateAsync({
        id,
        request: {
          firstName: values.firstName,
          lastName: values.lastName,
          email: values.email,
        },
      });

     if (values.role !== initialRole) {
        await changeUserRole.mutateAsync({
          id,
          request: {
            role: values.role,
          },
        });
      }

      navigate("/users");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to update user.",
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
      await activateUser.mutateAsync(id);
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setStatusError(
          error.response?.data.detail ??
            "Unable to activate user.",
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
      await deactivateUser.mutateAsync(
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
            "Unable to deactivate user.",
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
            Edit User
          </h1>

          <p className="mt-2 text-slate-500">
            Update user information, role,
            and account access.
          </p>
        </div>

        <span
          className={[
            "inline-flex w-fit rounded-full px-2.5 py-1 text-xs font-medium",
            user.isActive
              ? "bg-emerald-50 text-emerald-700"
              : "bg-red-50 text-red-700",
          ].join(" ")}
        >
          {user.isActive
            ? "Active"
            : "Inactive"}
        </span>
      </div>

      {isCurrentUser && (
        <div className="mt-6 rounded-lg border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-blue-700">
          This is your current account. You
          cannot remove your own Administrator
          role or deactivate your account.
        </div>
      )}

        <UserForm
        initialValues={{
            firstName: user.firstName,
            lastName: user.lastName,
            email: user.email,
            role: user.role,
        }}
        submitLabel="Save Changes"
        isSubmitting={isSubmitting}
        errorMessage={submitError}
        disableRole={isCurrentUser}
        onSubmit={handleSubmit}
        onCancel={() => {
            navigate("/users");
        }}
        />

      <div className="mt-8 border-t border-slate-200 pt-8">
        <div>
          <h2 className="text-lg font-semibold text-slate-900">
            Account Access
          </h2>

          <p className="mt-1 text-sm text-slate-500">
            Activate or deactivate this user
            account.
          </p>
        </div>

        {statusError && (
          <div
            role="alert"
            className="mt-4 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
          >
            {statusError}
          </div>
        )}

        <div className="mt-4">
          {user.isActive ? (
            <button
              type="button"
              onClick={() => {
                void handleDeactivate();
              }}
              disabled={
                isStatusPending ||
                isCurrentUser
              }
              className="rounded-lg border border-red-200 bg-white px-4 py-2 text-sm font-medium text-red-700 transition-colors hover:bg-red-50 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {deactivateUser.isPending
                ? "Deactivating..."
                : "Deactivate User"}
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
              {activateUser.isPending
                ? "Activating..."
                : "Activate User"}
            </button>
          )}
        </div>
      </div>
    </div>
  );
}