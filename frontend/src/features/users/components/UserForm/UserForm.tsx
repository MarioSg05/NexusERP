import {
  useState,
  type FormEvent,
} from "react";

import {
  Eye,
  EyeOff,
} from "lucide-react";

import type { UserRole } from "../../../auth/models/UserRole";

interface UserFormValues {
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
}

export interface UserFormSubmitValues {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: UserRole;
}

interface UserFormProps {
  initialValues?: UserFormValues;
  includePassword?: boolean;
  submitLabel?: string;
  isSubmitting?: boolean;
  errorMessage?: string | null;
  onSubmit: (
  values: UserFormSubmitValues,
  ) => void | Promise<void>;
  onCancel: () => void;
  disableRole?: boolean;
}

export function UserForm({
  initialValues,
  includePassword = false,
  submitLabel = "Save User",
  isSubmitting = false,
  errorMessage,
  disableRole = false,
  onSubmit,
  onCancel,
}: UserFormProps) {
  const [firstName, setFirstName] =
    useState(
      initialValues?.firstName ?? "",
    );

  const [lastName, setLastName] =
    useState(
      initialValues?.lastName ?? "",
    );

  const [email, setEmail] =
    useState(
      initialValues?.email ?? "",
    );

  const [password, setPassword] =
    useState("");

  const [
    showPassword,
    setShowPassword,
  ] = useState(false);

  const [role, setRole] =
    useState<UserRole>(
      initialValues?.role ?? "Viewer",
    );

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    await onSubmit({
      firstName: firstName.trim(),
      lastName: lastName.trim(),
      email: email.trim(),
      password,
      role,
    });
  }

  return (
    <form
      onSubmit={(event) => {
        void handleSubmit(event);
      }}
      className="mt-8 space-y-6"
    >
      <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
        <div>
          <label
            htmlFor="firstName"
            className="block text-sm font-medium text-slate-700"
          >
            First Name
          </label>

          <input
            id="firstName"
            type="text"
            value={firstName}
            onChange={(event) => {
              setFirstName(
                event.target.value,
              );
            }}
            required
            maxLength={100}
            disabled={isSubmitting}
            className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          />
        </div>

        <div>
          <label
            htmlFor="lastName"
            className="block text-sm font-medium text-slate-700"
          >
            Last Name
          </label>

          <input
            id="lastName"
            type="text"
            value={lastName}
            onChange={(event) => {
              setLastName(
                event.target.value,
              );
            }}
            required
            maxLength={100}
            disabled={isSubmitting}
            className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          />
        </div>
      </div>

      <div>
        <label
          htmlFor="email"
          className="block text-sm font-medium text-slate-700"
        >
          Email
        </label>

        <input
          id="email"
          type="email"
          value={email}
          onChange={(event) => {
            setEmail(event.target.value);
          }}
          required
          disabled={isSubmitting}
          autoComplete="email"
          className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
        />
      </div>

      {includePassword && (
        <div>
          <label
            htmlFor="password"
            className="block text-sm font-medium text-slate-700"
          >
            Password
          </label>

          <div className="relative mt-2">
            <input
              id="password"
              type={
                showPassword
                  ? "text"
                  : "password"
              }
              value={password}
              onChange={(event) => {
                setPassword(
                  event.target.value,
                );
              }}
              required
              minLength={8}
              disabled={isSubmitting}
              autoComplete="new-password"
              className="w-full rounded-lg border border-slate-300 px-3 py-2 pr-11 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
            />

            <button
              type="button"
              onClick={() => {
                setShowPassword(
                  (current) => !current,
                );
              }}
              disabled={isSubmitting}
              aria-label={
                showPassword
                  ? "Hide password"
                  : "Show password"
              }
              title={
                showPassword
                  ? "Hide password"
                  : "Show password"
              }
              className="absolute inset-y-0 right-0 flex w-11 items-center justify-center text-slate-500 transition-colors hover:text-slate-700 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {showPassword ? (
                <EyeOff size={18} />
              ) : (
                <Eye size={18} />
              )}
            </button>
          </div>

          <p className="mt-1 text-xs text-slate-500">
            Minimum 8 characters.
          </p>
        </div>
      )}

      <div>
        <label
          htmlFor="role"
          className="block text-sm font-medium text-slate-700"
        >
          Role
        </label>

        <select
          id="role"
          value={role}
          onChange={(event) => {
            setRole(
              event.target.value as UserRole,
            );
          }}
            disabled={
            isSubmitting ||
            disableRole
            }
          className="mt-2 w-full rounded-lg border border-slate-300 bg-white px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
        >
          <option value="Viewer">
            Viewer
          </option>

          <option value="Manager">
            Manager
          </option>

          <option value="Administrator">
            Administrator
          </option>
        </select>
      </div>

      {errorMessage && (
        <div
          role="alert"
          className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
        >
          {errorMessage}
        </div>
      )}

      <div className="flex justify-end gap-3">
        <button
          type="button"
          onClick={onCancel}
          disabled={isSubmitting}
          className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50"
        >
          Cancel
        </button>

        <button
          type="submit"
          disabled={isSubmitting}
          className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
        >
          {isSubmitting
            ? "Saving..."
            : submitLabel}
        </button>
      </div>
    </form>
  );
}