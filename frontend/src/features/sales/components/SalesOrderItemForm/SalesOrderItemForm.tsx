import { AlertTriangle, Trash2 } from "lucide-react";

import type { InventoryItem } from "../../../inventory/models/InventoryModel";
import type { Product } from "../../../products/models/ProductModel";
import type { CreateSalesOrderItemRequest } from "../../models/CreateSalesOrderModel";

interface SalesOrderItemFormProps {
  item: CreateSalesOrderItemRequest;
  products: Product[];
  inventory: InventoryItem[];
  canRemove: boolean;
  onChange: (
    item: CreateSalesOrderItemRequest,
  ) => void;
  onRemove: () => void;
}

export function SalesOrderItemForm({
  item,
  products,
  inventory,
  canRemove,
  onChange,
  onRemove,
}: SalesOrderItemFormProps) {
  const inventoryItem = inventory.find(
    (entry) =>
      entry.productId === item.productId,
  );

  const availableStock =
    inventoryItem?.quantity;

  const exceedsAvailableStock =
    availableStock !== undefined &&
    item.quantity > availableStock;

  return (
    <div className="rounded-xl border border-slate-200 p-4">
      <div className="grid grid-cols-1 gap-4 lg:grid-cols-[minmax(0,2fr)_minmax(120px,0.7fr)_minmax(150px,1fr)_minmax(150px,1fr)_auto] lg:items-end">
        <div>
          <label className="mb-2 block text-sm font-medium text-slate-700">
            Product
          </label>

          <select
            value={item.productId}
            onChange={(event) => {
              onChange({
                ...item,
                productId:
                  event.target.value,
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
              >
                {product.sku} —{" "}
                {product.name}
              </option>
            ))}
          </select>

          {item.productId && (
            <p className="mt-2 text-xs text-slate-500">
              Available stock:{" "}
              <span className="font-medium text-slate-700">
                {availableStock ?? 0}
              </span>
            </p>
          )}
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

      {exceedsAvailableStock && (
        <div className="mt-4 flex items-start gap-2 rounded-lg bg-amber-50 px-3 py-2 text-sm text-amber-700">
          <AlertTriangle
            size={16}
            className="mt-0.5 shrink-0"
          />

          <span>
            Requested quantity exceeds
            the currently available stock
            of {availableStock}. This order
            can be created, but it cannot be
            confirmed unless sufficient stock
            becomes available.
          </span>
        </div>
      )}
    </div>
  );
}