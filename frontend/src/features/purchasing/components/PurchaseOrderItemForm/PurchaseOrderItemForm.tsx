import { Trash2 } from "lucide-react";

import type { Product } from "../../../products/models/ProductModel";
import type { CreatePurchaseOrderItemRequest } from "../../models/CreatePurchaseOrderModel";

interface PurchaseOrderItemFormProps {
  item: CreatePurchaseOrderItemRequest;
  products: Product[];
  canRemove: boolean;
  onChange: (
    item: CreatePurchaseOrderItemRequest,
  ) => void;
  onRemove: () => void;
}

export function PurchaseOrderItemForm({
  item,
  products,
  canRemove,
  onChange,
  onRemove,
}: PurchaseOrderItemFormProps) {
  const selectedProductIds = new Set<string>();

  return (
    <div className="grid grid-cols-1 gap-4 rounded-xl border border-slate-200 p-4 lg:grid-cols-[minmax(0,2fr)_minmax(120px,0.7fr)_minmax(150px,1fr)_minmax(150px,1fr)_auto] lg:items-end">
      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700">
          Product
        </label>

        <select
          value={item.productId}
          onChange={(event) => {
            onChange({
              ...item,
              productId: event.target.value,
            });
          }}
          required
          className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        >
          <option value="">
            Select a product
          </option>

          {products.map((product) => (
            <option
              key={product.id}
              value={product.id}
              disabled={
                selectedProductIds.has(
                  product.id,
                )
              }
            >
              {product.sku} — {product.name}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700">
          Quantity
        </label>

        <input
          type="number"
          min="1"
          step="1"
          required
          value={item.quantity}
          onChange={(event) => {
            onChange({
              ...item,
              quantity: Number(
                event.target.value,
              ),
            });
          }}
          className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        />
      </div>

      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700">
          Unit price
        </label>

        <input
          type="number"
          min="0"
          step="0.01"
          required
          value={item.unitPrice}
          onChange={(event) => {
            onChange({
              ...item,
              unitPrice: Number(
                event.target.value,
              ),
            });
          }}
          className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        />
      </div>

      <div>
        <p className="mb-2 text-sm font-medium text-slate-700">
          Line total
        </p>

        <div className="flex h-[42px] items-center rounded-lg bg-slate-50 px-3 text-sm font-semibold text-slate-900">
          $
          {(
            item.quantity *
            item.unitPrice
          ).toFixed(2)}
        </div>
      </div>

      <button
        type="button"
        onClick={onRemove}
        disabled={!canRemove}
        aria-label="Remove item"
        title="Remove item"
        className="inline-flex h-[42px] w-[42px] items-center justify-center rounded-lg text-slate-500 transition-colors hover:bg-red-50 hover:text-red-600 disabled:cursor-not-allowed disabled:opacity-30"
      >
        <Trash2 size={17} />
      </button>
    </div>
  );
}