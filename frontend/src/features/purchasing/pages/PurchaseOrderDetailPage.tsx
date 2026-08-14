import axios from "axios";
import { useState } from "react";
import {
  Check,
  X,
} from "lucide-react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import { useApprovePurchaseOrder } from "../hooks/useApprovePurchaseOrder";
import { useCancelPurchaseOrder } from "../hooks/useCancelPurchaseOrder";
import { usePurchaseOrder } from "../hooks/usePurchaseOrder";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import { formatCurrency } from "../../../shared/lib/formatCurrency";
import { formatDate } from "../../../shared/lib/formatDate";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { PurchaseOrderStatus } from "../models/PurchaseOrderModel";

function getStatusClasses(
  status: PurchaseOrderStatus,
): string {
  switch (status) {
    case "Pending":
      return "bg-amber-50 text-amber-700";

    case "Approved":
      return "bg-emerald-50 text-emerald-700";

    case "Cancelled":
      return "bg-red-50 text-red-700";
  }
}

export function PurchaseOrderDetailPage() {
  const { id = "" } = useParams();
  const navigate = useNavigate();

  const [actionError, setActionError] =
    useState<string | null>(null);

  const {
    data: purchaseOrder,
    isLoading,
    error,
    refetch,
  } = usePurchaseOrder(id);

  const approvePurchaseOrder =
    useApprovePurchaseOrder();

  const cancelPurchaseOrder =
    useCancelPurchaseOrder();

  const isActionPending =
    approvePurchaseOrder.isPending ||
    cancelPurchaseOrder.isPending;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading purchase order...
      </p>
    );
  }

  if (error || !purchaseOrder) {
    return (
      <QueryErrorState
        title="Unable to load purchase order"
        description="We couldn't retrieve the purchase order information."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  async function handleApprove() {
    setActionError(null);

    try {
      await approvePurchaseOrder.mutateAsync(id);
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setActionError(
          error.response?.data.detail ??
            "Unable to approve purchase order.",
        );

        return;
      }

      setActionError(
        "An unexpected error occurred.",
      );
    }
  }

  async function handleCancel() {
    setActionError(null);

    try {
     await cancelPurchaseOrder.mutateAsync(id);
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setActionError(
          error.response?.data.detail ??
            "Unable to cancel purchase order.",
        );

        return;
      }

      setActionError(
        "An unexpected error occurred.",
      );
    }
  }

  return (
    <div>
      <div className="flex flex-col gap-6 sm:flex-row sm:items-start sm:justify-between">
        <div>
          <div className="flex flex-wrap items-center gap-3">
            <h1 className="text-3xl font-bold tracking-tight text-slate-900">
              Purchase Order
            </h1>

            <span
              className={[
                "inline-flex rounded-full px-2.5 py-1 text-xs font-medium",
                getStatusClasses(
                  purchaseOrder.status,
                ),
              ].join(" ")}
            >
              {purchaseOrder.status}
            </span>
          </div>

          <p className="mt-2 font-mono text-sm text-slate-500">
            {purchaseOrder.id}
          </p>
        </div>

        {purchaseOrder.status ===
          "Pending" && (
          <div className="flex flex-wrap gap-3">
            <button
              type="button"
              onClick={() => {
                void handleCancel();
              }}
              disabled={isActionPending}
              className="inline-flex items-center gap-2 rounded-lg border border-red-200 bg-white px-4 py-2 text-sm font-medium text-red-700 transition-colors hover:bg-red-50 disabled:cursor-not-allowed disabled:opacity-50"
            >
              <X size={16} />
              Cancel Order
            </button>

            <button
              type="button"
              onClick={() => {
                void handleApprove();
              }}
              disabled={isActionPending}
              className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
            >
              <Check size={16} />
              Approve Order
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
            Supplier
          </p>

          <p className="mt-2 font-medium text-slate-900">
            {purchaseOrder.supplierName}
          </p>
        </div>

        <div className="rounded-xl border border-slate-200 bg-slate-50 p-5">
          <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
            Order Date
          </p>

          <p className="mt-2 font-medium text-slate-900">
            {formatDate(
              purchaseOrder.orderDate,
            )}
          </p>
        </div>

        <div className="rounded-xl border border-slate-200 bg-slate-50 p-5">
          <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
            Total
          </p>

          <p className="mt-2 text-lg font-semibold text-slate-900">
            {formatCurrency(
              purchaseOrder.total,
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
              {purchaseOrder.items.map(
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
                    purchaseOrder.total,
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
            navigate("/purchasing");
          }}
          className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50"
        >
          Back to Purchasing
        </button>
      </div>
    </div>
  );
}