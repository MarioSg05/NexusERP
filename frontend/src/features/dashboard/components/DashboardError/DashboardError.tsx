import { AlertCircle, RefreshCw } from "lucide-react";

interface DashboardErrorProps {
  onRetry: () => void;
}

export function DashboardError({
  onRetry,
}: DashboardErrorProps) {
  return (
    <div
      role="alert"
      className="flex min-h-72 flex-col items-center justify-center rounded-xl border border-slate-200 bg-white p-8 text-center"
    >
      <div className="flex h-12 w-12 items-center justify-center rounded-full bg-red-50 text-red-600">
        <AlertCircle size={24} />
      </div>

      <h2 className="mt-4 text-lg font-semibold text-slate-900">
        Unable to load dashboard
      </h2>

      <p className="mt-2 max-w-md text-sm text-slate-500">
        We couldn't retrieve the latest dashboard information.
        Check your connection and try again.
      </p>

      <button
        type="button"
        onClick={onRetry}
        className="mt-6 flex items-center gap-2 rounded-lg bg-slate-900 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-slate-800"
      >
        <RefreshCw size={16} />

        Try again
      </button>
    </div>
  );
}