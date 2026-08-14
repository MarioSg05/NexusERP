import axios from "axios";
import { useState } from "react";
import { Check, X } from "lucide-react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { ConfirmSalesOrderDialog } from "../components/ConfirmSalesOrderDialog/ConfirmSalesOrderDialog";

import { useCancelSalesOrder } from "../hooks/useCancelSalesOrder";
import { useConfirmSalesOrder } from "../hooks/useConfirmSalesOrder";
import { useSalesOrder } from "../hooks/useSalesOrder";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import { formatCurrency } from "../../../shared/lib/formatCurrency";
import { formatDate } from "../../../shared/lib/formatDate";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { SalesOrderStatus } from "../models/SalesOrderModel";

function getStatusClasses(
  status: SalesOrderStatus,
): string {
  switch (status) {
    case "Pending":
      return "bg-amber-50 text-amber-700";

    case "Confirmed":
      return "bg-emerald-50 text-emerald-700";

    case "Cancelled":
      return "bg-red-50 text-red-700";
  }
}

export function SalesOrderDetailPage() {
  const { id = "" } = useParams();
  const navigate = useNavigate();

  const [showConfirmDialog, setShowConfirmDialog] =
    useState(false);

  const [confirmError, setConfirmError] =
    useState<string | null>(null);

  const [actionError, setActionError] =
    useState<string | null>(null);

  const {
    data: salesOrder,
    isLoading,
    error,
    refetch,
  } = useSalesOrder(id);

  const confirmSalesOrder =
    useConfirmSalesOrder();

  const cancelSalesOrder =
    useCancelSalesOrder();

  const isActionPending =
    confirmSalesOrder.isPending ||
    cancelSalesOrder.isPending;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading sales order...
      </p>
    );
  }

  if (error || !salesOrder) {
    return (
      <QueryErrorState
        title="Unable to load sales order"
        description="We couldn't retrieve the sales order information."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  async function handleConfirm() {
    setConfirmError(null);

    try {
      await confirmSalesOrder.mutateAsync(id);

      setShowConfirmDialog(false);
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setConfirmError(
          error.response?.data.detail ??
            "Unable to confirm sales order.",
        );

        return;
      }

      setConfirmError(
        "An unexpected error occurred.",
      );
    }
  }

  async function handleCancel() {
    setActionError(null);

    try {
      await cancelSalesOrder.mutateAsync(id);
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setActionError(
          error.response?.data.detail ??
            "Unable to cancel sales order.",
        );

        return;
      }

      setActionError(
        "An unexpected error occurred.",
      );
    }
  }

  return (
    <>
      <div>
        <div className="flex flex-col gap-6 sm:flex-row sm:items-start sm:justify-between">
          <div>
            <div className="flex flex-wrap items-center gap-3">
              <h1 className="text-3xl font-bold tracking-tight text-slate-900">
                Sales Order
              </h1>

              <span
                className={[
                  "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                  getStatusClasses(
                    salesOrder.status,
                  ),
                ].join(" ")}
              >
                {salesOrder.status}
              </span>
            </div>

            <p className="mt-2 font-mono text-sm text-slate-500">
              {salesOrder.id}
            </p>
          </div>

          {salesOrder.status ===
            "Pending" && (
            <div className="flex flex-wrap gap-3">
              <button
                type="button"
                onClick={() => {
                  void handleCancel();
                }}
                disabled={isActionPending}
                className="inline-flex items-center gap-2 rounded-lg border border-red-200 bg-white px-4 py-2 text-sm font-medium text-red-700 hover:bg-red-50 disabled:opacity-50"
              >
                <X size={16} />
                Cancel Order
              </button>

              <button
                type="button"
                onClick={() => {
                  setActionError(null);
                  setConfirmError(null);
                  setShowConfirmDialog(true);
                }}
                disabled={isActionPending}
                className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
              >
                <Check size={16} />
                Confirm Order
              </button>
            </div>
          )}
        </div>

        {actionError && (
          <div
            role="alert"
            className="mt-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
          >
            {actionError}
          </div>
        )}

        <div className="mt-8 grid grid-cols-1 gap-4 md:grid-cols-3">
          <div className="rounded-xl border border-slate-200 bg-slate-50 p-5">
            <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
              Customer
            </p>

            <p className="mt-2 font-medium text-slate-900">
              {salesOrder.customerName}
            </p>
          </div>

          <div className="rounded-xl border border-slate-200 bg-slate-50 p-5">
            <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
              Order Date
            </p>

            <p className="mt-2 font-medium text-slate-900">
              {formatDate(
                salesOrder.orderDate,
              )}
            </p>
          </div>

          <div className="rounded-xl border border-slate-200 bg-slate-50 p-5">
            <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
              Total
            </p>

            <p className="mt-2 text-lg font-semibold text-slate-900">
              {formatCurrency(
                salesOrder.total,
              )}
            </p>
          </div>
        </div>

        <div className="mt-8 overflow-hidden rounded-xl border border-slate-200">
          <div className="border-b border-slate-200 bg-slate-50 px-6 py-4">
            <h2 className="font-semibold text-slate-900">
              Order Items
            </h2>
          </div>

          <div className="overflow-x-auto">
            <table className="w-full border-collapse text-left">
              <thead className="bg-white">
                <tr className="border-b border-slate-200">
                  <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                    Product
                  </th>

                  <th className="px-6 py-4 text-xs font-semibold uppercase tracking-wide text-slate-500">
                    SKU
                  </th>

                  <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                    Quantity
                  </th>

                  <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                    Unit Price
                  </th>

                  <th className="px-6 py-4 text-right text-xs font-semibold uppercase tracking-wide text-slate-500">
                    Line Total
                  </th>
                </tr>
              </thead>

              <tbody className="divide-y divide-slate-200 bg-white">
                {salesOrder.items.map(
                  (item) => (
                    <tr key={item.id}>
                      <td className="px-6 py-4 font-medium text-slate-900">
                        {item.productName}
                      </td>

                      <td className="px-6 py-4 text-sm text-slate-600">
                        {item.sku}
                      </td>

                      <td className="px-6 py-4 text-right text-sm text-slate-700">
                        {item.quantity}
                      </td>

                      <td className="px-6 py-4 text-right text-sm text-slate-700">
                        {formatCurrency(
                          item.unitPrice,
                        )}
                      </td>

                      <td className="px-6 py-4 text-right text-sm font-medium text-slate-900">
                        {formatCurrency(
                          item.lineTotal,
                        )}
                      </td>
                    </tr>
                  ),
                )}
              </tbody>

              <tfoot>
                <tr className="border-t border-slate-200 bg-slate-50">
                  <td
                    colSpan={4}
                    className="px-6 py-4 text-right text-sm font-semibold text-slate-700"
                  >
                    Total
                  </td>

                  <td className="px-6 py-4 text-right font-semibold text-slate-900">
                    {formatCurrency(
                      salesOrder.total,
                    )}
                  </td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>

        <div className="mt-6 flex justify-end">
          <button
            type="button"
            onClick={() => {
              navigate("/sales");
            }}
            className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50"
          >
            Back to Sales
          </button>
        </div>
      </div>

      {showConfirmDialog && (
        <ConfirmSalesOrderDialog
          isSubmitting={
            confirmSalesOrder.isPending
          }
          errorMessage={confirmError}
          onConfirm={handleConfirm}
          onClose={() => {
            if (
              !confirmSalesOrder.isPending
            ) {
              setShowConfirmDialog(false);
              setConfirmError(null);
            }
          }}
        />
      )}
    </>
  );
}