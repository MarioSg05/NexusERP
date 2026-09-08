import { useState, type FormEvent } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

import { useProducts } from "../../products/hooks/useProducts";
import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import { useCreateInventory } from "../hooks/useCreateInventory";
import { useInventory } from "../hooks/useInventory";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";

export function CreateInventoryPage() {
  const navigate = useNavigate();

  const [productId, setProductId] = useState("");
  const [quantity, setQuantity] = useState("0");
  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const {
    data: products,
    isLoading: isLoadingProducts,
    error: productsError,
    refetch: refetchProducts,
  } = useProducts();

  const {
    data: inventory,
    isLoading: isLoadingInventory,
    error: inventoryError,
    refetch: refetchInventory,
  } = useInventory();

  const createInventory =
    useCreateInventory();

  const isLoading =
    isLoadingProducts || isLoadingInventory;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading available products...
      </p>
    );
  }

  if (
    productsError ||
    inventoryError ||
    !products ||
    !inventory
  ) {
    return (
      <QueryErrorState
        title="Unable to prepare inventory"
        description="We couldn't retrieve the information required to create inventory. Check your connection and try again."
        onRetry={() => {
          void Promise.all([
            refetchProducts(),
            refetchInventory(),
          ]);
        }}
      />
    );
  }

  const inventoryProductIds =
    new Set(
      inventory.map(
        (item) => item.productId,
      ),
    );

  const availableProducts =
    products.filter(
      (product) =>
        !inventoryProductIds.has(product.id),
    );

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setSubmitError(null);

    try {
      await createInventory.mutateAsync({
        productId,
        quantity: Number(quantity),
      });

      navigate("/inventory");
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to create inventory.",
        );

        return;
      }

      setSubmitError(
        "An unexpected error occurred.",
      );
    }
  }

  return (
    <div>
      <h1 className="text-3xl font-bold tracking-tight text-slate-900">
        Create Inventory
      </h1>

      <p className="mt-2 text-slate-500">
        Create inventory for a product and set its initial stock quantity.
      </p>

      {availableProducts.length === 0 ? (
        <div className="mt-8 rounded-xl border border-slate-200 bg-slate-50 p-8 text-center">
          <h2 className="text-lg font-semibold text-slate-900">
            All products have inventory
          </h2>

          <p className="mt-2 text-sm text-slate-500">
            There are no products available for inventory creation.
          </p>

          <button
            type="button"
            onClick={() => {
              navigate("/inventory");
            }}
            className="mt-6 rounded-lg bg-slate-900 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-slate-800"
          >
            Back to Inventory
          </button>
        </div>
      ) : (
        <form
          onSubmit={(event) => {
            void handleSubmit(event);
          }}
          className="mt-8 space-y-6"
        >
          <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div>
              <label
                htmlFor="inventory-product"
                className="mb-2 block text-sm font-medium text-slate-700"
              >
                Product
              </label>

              <select
                id="inventory-product"
                value={productId}
                onChange={(event) => {
                  setProductId(
                    event.target.value,
                  );
                }}
                required
                disabled={createInventory.isPending}
                className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
              >
                <option value="">
                  Select a product
                </option>

                {availableProducts.map(
                  (product) => (
                    <option
                      key={product.id}
                      value={product.id}
                    >
                      {product.sku} — {product.name}
                    </option>
                  ),
                )}
              </select>
            </div>

            <div>
              <label
                htmlFor="inventory-quantity"
                className="mb-2 block text-sm font-medium text-slate-700"
              >
                Initial quantity
              </label>

              <input
                id="inventory-quantity"
                type="number"
                value={quantity}
                onChange={(event) => {
                  setQuantity(
                    event.target.value,
                  );
                }}
                required
                min="0"
                step="1"
                disabled={createInventory.isPending}
                className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
              />
            </div>
          </div>

          {submitError && (
            <div
              role="alert"
              className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
            >
              {submitError}
            </div>
          )}

          <div className="flex justify-end gap-3 border-t border-slate-200 pt-6">
            <button
              type="button"
              onClick={() => {
                navigate("/inventory");
              }}
              disabled={createInventory.isPending}
              className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50"
            >
              Cancel
            </button>

            <button
              type="submit"
              disabled={createInventory.isPending}
              className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {createInventory.isPending
                ? "Creating..."
                : "Create Inventory"}
            </button>
          </div>
        </form>
      )}
    </div>
  );
}