import {
  useState,
  type FormEvent,
} from "react";

import axios from "axios";
import {
  Eye,
  EyeOff,
  LockKeyhole,
  Mail,
} from "lucide-react";

import { Navigate } from "react-router-dom";

import { useAuth } from "../hooks/useAuth";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";

export function LoginPage() {
  const {
    login,
    isAuthenticated,
    isLoading,
  } = useAuth();

  const [email, setEmail] =
    useState("");

  const [password, setPassword] =
    useState("");

  const [
    showPassword,
    setShowPassword,
  ] = useState(false);

  const [
    isSubmitting,
    setIsSubmitting,
  ] = useState(false);

  const [
    submitError,
    setSubmitError,
  ] = useState<string | null>(null);

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-slate-50">
        <div className="text-center">
          <div className="mx-auto h-8 w-8 animate-spin rounded-full border-2 border-slate-300 border-t-blue-600" />

          <p className="mt-4 text-sm text-slate-500">
            Restoring session...
          </p>
        </div>
      </div>
    );
  }

  if (isAuthenticated) {
    return (
      <Navigate
        to="/"
        replace
      />
    );
  }

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setSubmitError(null);
    setIsSubmitting(true);

    try {
      await login({
        email,
        password,
      });
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to sign in.",
        );
      } else {
        setSubmitError(
          "An unexpected error occurred.",
        );
      }
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="flex min-h-screen bg-slate-50">
      <div className="flex w-full items-center justify-center px-6 py-12">
        <div className="w-full max-w-md">
          <div className="mb-8 text-center">
            <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-xl bg-blue-600 text-lg font-bold text-white shadow-sm">
              N
            </div>

            <h1 className="mt-5 text-3xl font-bold tracking-tight text-slate-900">
              NexusERP
            </h1>

            <p className="mt-2 text-sm text-slate-500">
              Sign in to continue to your workspace.
            </p>
          </div>

          <div className="rounded-xl border border-slate-200 bg-white p-8 shadow-sm">
            <div>
              <h2 className="text-xl font-semibold text-slate-900">
                Sign in
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                Enter your account credentials.
              </p>
            </div>

            <form
              onSubmit={(event) => {
                void handleSubmit(event);
              }}
              className="mt-6 space-y-5"
            >
              <div>
                <label
                  htmlFor="login-email"
                  className="mb-2 block text-sm font-medium text-slate-700"
                >
                  Email
                </label>

                <div className="relative">
                  <Mail
                    size={17}
                    className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-slate-400"
                  />

                  <input
                    id="login-email"
                    type="email"
                    autoComplete="email"
                    required
                    value={email}
                    onChange={(event) => {
                      setEmail(
                        event.target.value,
                      );

                      if (submitError) {
                        setSubmitError(null);
                      }
                    }}
                    disabled={isSubmitting}
                    placeholder="you@example.com"
                    className="w-full rounded-lg border border-slate-300 bg-white py-2.5 pl-10 pr-3 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
                  />
                </div>
              </div>

              <div>
                <label
                  htmlFor="login-password"
                  className="mb-2 block text-sm font-medium text-slate-700"
                >
                  Password
                </label>

                <div className="relative">
                  <LockKeyhole
                    size={17}
                    className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-slate-400"
                  />

                  <input
                    id="login-password"
                    type={
                      showPassword
                        ? "text"
                        : "password"
                    }
                    autoComplete="current-password"
                    required
                    value={password}
                    onChange={(event) => {
                      setPassword(
                        event.target.value,
                      );

                      if (submitError) {
                        setSubmitError(null);
                      }
                    }}
                    disabled={isSubmitting}
                    className="w-full rounded-lg border border-slate-300 bg-white py-2.5 pl-10 pr-11 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
                  />

                  <button
                    type="button"
                    onClick={() => {
                      setShowPassword(
                        (current) =>
                          !current,
                      );
                    }}
                    disabled={isSubmitting}
                    aria-label={
                      showPassword
                        ? "Hide password"
                        : "Show password"
                    }
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 transition-colors hover:text-slate-700 disabled:opacity-50"
                  >
                    {showPassword ? (
                      <EyeOff size={17} />
                    ) : (
                      <Eye size={17} />
                    )}
                  </button>
                </div>
              </div>

              {submitError && (
                <div
                  role="alert"
                  className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
                >
                  {submitError}
                </div>
              )}

              <button
                type="submit"
                disabled={isSubmitting}
                className="inline-flex w-full items-center justify-center rounded-lg bg-blue-600 px-4 py-2.5 text-sm font-medium text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
              >
                {isSubmitting
                  ? "Signing in..."
                  : "Sign in"}
              </button>
            </form>
          </div>

          <p className="mt-6 text-center text-xs text-slate-400">
            NexusERP · Enterprise Resource Planning
          </p>
        </div>
      </div>
    </div>
  );
}