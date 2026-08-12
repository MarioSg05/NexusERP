import {
  useState,
  type FormEvent,
} from "react";

import type { RegisterProductRequest } from "../../models/RegisterProductModel";

interface ProductFormInitialValues {
  name: string;
  sku: string;
  price: number;
}

interface ProductFormProps {
  initialValues?: ProductFormInitialValues;
  isEditMode?: boolean;
  submitLabel?: string;
  isSubmitting: boolean;
  errorMessage?: string | null;
  onSubmit: (
    request: RegisterProductRequest,
  ) => Promise<void>;
  onCancel: () => void;
}

export function ProductForm({
  initialValues,
  isEditMode = false,
  submitLabel = "Create Product",
  isSubmitting,
  errorMessage,
  onSubmit,
  onCancel,
}: ProductFormProps) {
  const [name, setName] = useState(
    initialValues?.name ?? "",
  );

  const [sku, setSku] = useState(
    initialValues?.sku ?? "",
  );

  const [price, setPrice] = useState(
    initialValues?.price.toString() ?? "",
  );

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    await onSubmit({
      name: name.trim(),
      sku: sku.trim(),
      price: Number(price),
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
            htmlFor="product-name"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Product name
          </label>

          <input
            id="product-name"
            type="text"
            value={name}
            onChange={(event) => {
              setName(event.target.value);
            }}
            required
            maxLength={200}
            disabled={isSubmitting}
            className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          />
        </div>

        <div>
          <label
            htmlFor="product-sku"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            SKU
          </label>

          <input
            id="product-sku"
            type="text"
            value={sku}
            onChange={(event) => {
              setSku(event.target.value);
            }}
            required
            maxLength={50}
            disabled={isSubmitting || isEditMode}
            className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          />

          {isEditMode && (
            <p className="mt-2 text-xs text-slate-500">
              SKU cannot be changed after the product is created.
            </p>
          )}
        </div>

        <div>
          <label
            htmlFor="product-price"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Price
          </label>

          <input
            id="product-price"
            type="number"
            value={price}
            onChange={(event) => {
              setPrice(event.target.value);
            }}
            required
            min="0"
            step="0.01"
            disabled={isSubmitting}
            className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          />
        </div>
      </div>

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