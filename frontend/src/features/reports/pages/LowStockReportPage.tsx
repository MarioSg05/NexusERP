import { useState, type FormEvent } from "react";

import { Filter } from "lucide-react";

import { InventoryReportTable } from "../components/InventoryReportTable/InventoryReportTable";
import { ReportEmptyState } from "../components/ReportEmptyState/ReportEmptyState";
import { ReportTableSkeleton } from "../components/ReportTableSkeleton/ReportTableSkeleton";

import { useLowStockReport } from "../hooks/useLowStockReport";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function LowStockReportPage() {
    const [inputValue, setInputValue] = useState("10");

    const [minimumStock, setMinimumStock] = useState(10);

    const [validationError, setValidationError] = useState<string | null>(null);

    const {
        data: items,
        isLoading,
        error,
        refetch,
    } = useLowStockReport(minimumStock);

    function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (inputValue.trim() === "") {
            setValidationError("Minimum stock is required.");

            return;
        }

        const value = Number(inputValue);

        if (!Number.isInteger(value) || value < 0) {
            setValidationError(
                "Minimum stock must be a non-negative whole number.",
            );

            return;
        }

        setValidationError(null);
        setMinimumStock(value);
    }

    return (
        <div>
            <div>
                <h2 className="text-xl font-semibold text-slate-900">
                    Low Stock Report
                </h2>

                <p className="mt-2 text-sm text-slate-500">
                    Products at or below the selected stock threshold.
                </p>
            </div>

            <form
                onSubmit={handleSubmit}
                className="mt-6 flex flex-col gap-3 rounded-xl border border-slate-200 bg-slate-50 p-4 sm:flex-row sm:items-end"
            >
                <div className="w-full sm:max-w-xs">
                    <label
                        htmlFor="minimum-stock"
                        className="mb-2 block text-sm font-medium text-slate-700"
                    >
                        Minimum stock
                    </label>

                    <input
                        id="minimum-stock"
                        type="number"
                        value={inputValue}
                        onChange={(event) => {
                            setInputValue(event.target.value);

                            if (validationError) {
                                setValidationError(null);
                            }
                        }}
                        className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                    />

                    {validationError && (
                        <p className="mt-2 text-sm text-red-600">
                            {validationError}
                        </p>
                    )}
                </div>

                <button
                    type="submit"
                    className="inline-flex h-[42px] items-center justify-center gap-2 rounded-lg bg-blue-600 px-4 text-sm font-medium text-white transition-colors hover:bg-blue-700"
                >
                    <Filter size={16} />
                    Apply
                </button>
            </form>

            <div className="mt-6">
                <div className="mb-3 text-sm text-slate-500">
                    Showing products with stock less than or equal to{" "}
                    <span className="font-semibold text-slate-700">
                        {minimumStock}
                    </span>
                    .
                </div>

                {isLoading ? (
                    <ReportTableSkeleton />
                ) : error || !items ? (
                    <QueryErrorState
                        title="Unable to load low stock report"
                        description="We couldn't retrieve the low stock report."
                        onRetry={() => {
                            void refetch();
                        }}
                    />
                ) : items.length === 0 ? (
                    <ReportEmptyState
                        title="No low-stock products"
                        description={`No products currently have stock less than or equal to ${minimumStock}.`}
                    />
                ) : (
                    <InventoryReportTable items={items} />
                )}
            </div>
        </div>
    );
}
