import {
  useState,
  type FormEvent,
} from "react";

import {
  Filter,
  RotateCcw,
} from "lucide-react";

import type { ReportDateFilters } from "../../models/SalesReportModel";

interface ReportDateFilterProps {
  filters: ReportDateFilters;
  onApply: (
    filters: ReportDateFilters,
  ) => void;
}

export function ReportDateFilter({
  filters,
  onApply,
}: ReportDateFilterProps) {
  const [from, setFrom] = useState(
    filters.from ?? "",
  );

  const [to, setTo] = useState(
    filters.to ?? "",
  );

  const [
    validationError,
    setValidationError,
  ] = useState<string | null>(null);

  function clearValidationError() {
    if (validationError) {
      setValidationError(null);
    }
  }

  function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    if (
      from &&
      to &&
      from > to
    ) {
      setValidationError(
        "From date cannot be later than To date.",
      );

      return;
    }

    setValidationError(null);

    onApply({
      from: from || undefined,
      to: to || undefined,
    });
  }

  function handleClear() {
    setFrom("");
    setTo("");
    setValidationError(null);

    onApply({});
  }

  return (
    <form
      onSubmit={handleSubmit}
      className="rounded-xl border border-slate-200 bg-slate-50 p-4"
    >
      <div className="flex flex-col gap-4 lg:flex-row lg:items-end">
        <div className="w-full lg:max-w-xs">
          <label
            htmlFor="report-from-date"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            From
          </label>

          <input
            id="report-from-date"
            type="date"
            value={from}
            onChange={(event) => {
              setFrom(
                event.target.value,
              );

              clearValidationError();
            }}
            className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
          />
        </div>

        <div className="w-full lg:max-w-xs">
          <label
            htmlFor="report-to-date"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            To
          </label>

          <input
            id="report-to-date"
            type="date"
            value={to}
            onChange={(event) => {
              setTo(
                event.target.value,
              );

              clearValidationError();
            }}
            className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
          />
        </div>

        <div className="flex flex-wrap gap-3">
          <button
            type="submit"
            className="inline-flex h-[42px] items-center justify-center gap-2 rounded-lg bg-blue-600 px-4 text-sm font-medium text-white transition-colors hover:bg-blue-700"
          >
            <Filter size={16} />
            Apply
          </button>

          <button
            type="button"
            onClick={handleClear}
            className="inline-flex h-[42px] items-center justify-center gap-2 rounded-lg border border-slate-300 bg-white px-4 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50"
          >
            <RotateCcw size={16} />
            Clear
          </button>
        </div>
      </div>

      {validationError && (
        <p
          role="alert"
          className="mt-3 text-sm text-red-600"
        >
          {validationError}
        </p>
      )}
    </form>
  );
}