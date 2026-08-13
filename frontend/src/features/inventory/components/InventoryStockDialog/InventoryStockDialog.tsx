import {
  useEffect,
  useState,
  type FormEvent,
} from "react";
import { X } from "lucide-react";

import type { InventoryItem } from "../../models/InventoryModel";

export type InventoryStockMode =
  | "increase"
  | "decrease"
  | "adjust";

interface InventoryStockDialogProps {
  item: InventoryItem;
  mode: InventoryStockMode;
  isSubmitting: boolean;
  errorMessage?: string | null;
  onSubmit: (quantity: number) => Promise<void>;
  onClose: () => void;
}

function getDialogContent(
  mode: InventoryStockMode,
) {
  switch (mode) {
    case "increase":
      return {
        title: "Increase Stock",
        label: "Quantity to add",
        submitLabel: "Increase Stock",
      };

    case "decrease":
      return {
        title: "Decrease Stock",
        label: "Quantity to remove",
        submitLabel: "Decrease Stock",
      };

    case "adjust":
      return {
        title: "Adjust Stock",
        label: "New quantity",
        submitLabel: "Adjust Stock",
      };
  }
}

export function InventoryStockDialog({
  item,
  mode,
  isSubmitting,
  errorMessage,
  onSubmit,
  onClose,
}: InventoryStockDialogProps) {
  const [quantity, setQuantity] = useState("");

  const content = getDialogContent(mode);

  const numericQuantity =
    Number(quantity);

  const hasQuantity =
    quantity.trim() !== "";

  const projectedQuantity =
    mode === "increase"
      ? item.quantity + numericQuantity
      : mode === "decrease"
        ? item.quantity - numericQuantity
        : numericQuantity;

  useEffect(() => {
    function handleKeyDown(
      event: KeyboardEvent,
    ) {
      if (
        event.key === "Escape" &&
        !isSubmitting
      ) {
        onClose();
      }
    }

    window.addEventListener(
      "keydown",
      handleKeyDown,
    );

    return () => {
      window.removeEventListener(
        "keydown",
        handleKeyDown,
      );
    };
  }, [isSubmitting, onClose]);

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    await onSubmit(numericQuantity);
  }

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/40 p-4"
      role="presentation"
    >
      <div
        role="dialog"
        aria-modal="true"
        aria-labelledby="inventory-stock-dialog-title"
        className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl"
      >
        <div className="flex items-start justify-between gap-4">
          <div>
            <h2
              id="inventory-stock-dialog-title"
              className="text-xl font-semibold text-slate-900"
            >
              {content.title}
            </h2>

            <p className="mt-1 text-sm text-slate-500">
              {item.productName}
            </p>

            <p className="mt-1 text-xs text-slate-400">
              {item.sku}
            </p>
          </div>

          <button
            type="button"
            onClick={onClose}
            disabled={isSubmitting}
            aria-label="Close dialog"
            className="inline-flex h-9 w-9 items-center justify-center rounded-lg text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 disabled:cursor-not-allowed disabled:opacity-50"
          >
            <X size={18} />
          </button>
        </div>

        <div className="mt-6 rounded-lg bg-slate-50 p-4">
          <div className="flex items-center justify-between text-sm">
            <span className="text-slate-500">
              Current quantity
            </span>

            <span className="font-semibold text-slate-900">
              {item.quantity}
            </span>
          </div>
        </div>

        <form
          onSubmit={(event) => {
            void handleSubmit(event);
          }}
          className="mt-6 space-y-6"
        >
          <div>
            <label
              htmlFor="stock-quantity"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              {content.label}
            </label>

            <input
              id="stock-quantity"
              type="number"
              value={quantity}
              onChange={(event) => {
                setQuantity(
                  event.target.value,
                );
              }}
              min={mode === "adjust" ? 0 : 1}
              step="1"
              required
              autoFocus
              disabled={isSubmitting}
              className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
            />
          </div>

          {hasQuantity &&
            Number.isFinite(numericQuantity) && (
              <div className="flex items-center justify-between rounded-lg border border-slate-200 px-4 py-3 text-sm">
                <span className="text-slate-500">
                  Resulting quantity
                </span>

                <span
                  className={[
                    "font-semibold",
                    projectedQuantity < 0
                      ? "text-red-600"
                      : "text-slate-900",
                  ].join(" ")}
                >
                  {projectedQuantity}
                </span>
              </div>
            )}

          {errorMessage && (
            <div
              role="alert"
              className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
            >
              {errorMessage}
            </div>
          )}

          <div className="flex justify-end gap-3 border-t border-slate-200 pt-6">
            <button
              type="button"
              onClick={onClose}
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
                : content.submitLabel}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}