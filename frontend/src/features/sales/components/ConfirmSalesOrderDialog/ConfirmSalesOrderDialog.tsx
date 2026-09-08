import { AlertTriangle, X } from "lucide-react";

interface ConfirmSalesOrderDialogProps {
  isSubmitting: boolean;
  errorMessage?: string | null;
  onConfirm: () => Promise<void>;
  onClose: () => void;
}

export function ConfirmSalesOrderDialog({
  isSubmitting,
  errorMessage,
  onConfirm,
  onClose,
}: ConfirmSalesOrderDialogProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/40 p-4">
      <div
        role="dialog"
        aria-modal="true"
        aria-labelledby="confirm-sales-order-title"
        className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl"
      >
        <div className="flex items-start justify-between gap-4">
          <div className="flex gap-3">
            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-amber-50 text-amber-700">
              <AlertTriangle size={20} />
            </div>

            <div>
              <h2
                id="confirm-sales-order-title"
                className="text-lg font-semibold text-slate-900"
              >
                Confirm Sales Order
              </h2>

              <p className="mt-2 text-sm leading-6 text-slate-500">
                Confirming this sales order will decrease inventory
                according to the quantities in the order.
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={onClose}
            disabled={isSubmitting}
            aria-label="Close dialog"
            className="inline-flex h-9 w-9 shrink-0 items-center justify-center rounded-lg text-slate-500 hover:bg-slate-100 hover:text-slate-900 disabled:opacity-50"
          >
            <X size={18} />
          </button>
        </div>

        {errorMessage && (
          <div
            role="alert"
            className="mt-5 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
          >
            {errorMessage}
          </div>
        )}

        <div className="mt-6 flex justify-end gap-3 border-t border-slate-200 pt-5">
          <button
            type="button"
            onClick={onClose}
            disabled={isSubmitting}
            className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50 disabled:opacity-50"
          >
            Cancel
          </button>

          <button
            type="button"
            onClick={() => {
              void onConfirm();
            }}
            disabled={isSubmitting}
            className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
          >
            {isSubmitting
              ? "Confirming..."
              : "Confirm Order"}
          </button>
        </div>
      </div>
    </div>
  );
}