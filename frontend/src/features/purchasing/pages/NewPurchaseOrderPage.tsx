import {
  useMemo,
  useState,
  type FormEvent,
} from "react";
import axios from "axios";
import { Plus } from "lucide-react";
import { useNavigate } from "react-router-dom";

import { PurchaseOrderItemForm } from "../components/PurchaseOrderItemForm/PurchaseOrderItemForm";

import { useProducts } from "../../products/hooks/useProducts";
import { useSuppliers } from "../../suppliers/hooks/useSuppliers";
import { useCreatePurchaseOrder } from "../hooks/useCreatePurchaseOrder";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";
import { formatCurrency } from "../../../shared/lib/formatCurrency";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { CreatePurchaseOrderItemRequest } from "../models/CreatePurchaseOrderModel";

const createEmptyItem =
  (): CreatePurchaseOrderItemRequest => ({
    productId: "",
    quantity: 1,
    unitPrice: 0,
  });

export function NewPurchaseOrderPage() {
  const navigate = useNavigate();

  const [supplierId, setSupplierId] =
    useState("");

  const [items, setItems] = useState<
    CreatePurchaseOrderItemRequest[]
  >([createEmptyItem()]);

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const {
    data: suppliers,
    isLoading: isLoadingSuppliers,
    error: suppliersError,
    refetch: refetchSuppliers,
  } = useSuppliers();

  const {
    data: products,
    isLoading: isLoadingProducts,
    error: productsError,
    refetch: refetchProducts,
  } = useProducts();

  const createPurchaseOrder =
    useCreatePurchaseOrder();

  const activeSuppliers =
    suppliers?.filter(
      (supplier) => supplier.isActive,
    ) ?? [];

  const activeProducts =
    products?.filter(
      (product) => product.isActive,
    ) ?? [];

  const total = useMemo(
    () =>
      items.reduce(
        (sum, item) =>
          sum +
          item.quantity *
            item.unitPrice,
        0,
      ),
    [items],
  );

  const isLoading =
    isLoadingSuppliers ||
    isLoadingProducts;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading purchase order data...
      </p>
    );
  }

  if (
    suppliersError ||
    productsError ||
    !suppliers ||
    !products
  ) {
    return (
      <QueryErrorState
        title="Unable to prepare purchase order"
        description="We couldn't retrieve the suppliers and products required to create a purchase order."
        onRetry={() => {
          void Promise.all([
            refetchSuppliers(),
            refetchProducts(),
          ]);
        }}
      />
    );
  }

  function updateItem(
    index: number,
    updatedItem: CreatePurchaseOrderItemRequest,
  ) {
    setItems((current) =>
      current.map((item, itemIndex) =>
        itemIndex === index
          ? updatedItem
          : item,
      ),
    );
  }

  function removeItem(index: number) {
    setItems((current) =>
      current.filter(
        (_, itemIndex) =>
          itemIndex !== index,
      ),
    );
  }

  function addItem() {
    setItems((current) => [
      ...current,
      createEmptyItem(),
    ]);
  }

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setSubmitError(null);

    const selectedProductIds =
      items
        .map((item) => item.productId)
        .filter(Boolean);

    const hasDuplicateProducts =
      new Set(selectedProductIds).size !==
      selectedProductIds.length;

    if (hasDuplicateProducts) {
      setSubmitError(
        "A product can only appear once in a purchase order.",
      );

      return;
    }

    try {
      const response =
        await createPurchaseOrder.mutateAsync({
          supplierId,
          items,
        });

      navigate(
        `/purchasing/${response.id}`,
      );
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to create purchase order.",
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
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-slate-900">
          New Purchase Order
        </h1>

        <p className="mt-2 text-slate-500">
          Create a purchase order for a supplier.
        </p>
      </div>

      <form
        onSubmit={(event) => {
          void handleSubmit(event);
        }}
        className="mt-8 space-y-8"
      >
        <div>
          <label
            htmlFor="purchase-order-supplier"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Supplier
          </label>

          <select
            id="purchase-order-supplier"
            value={supplierId}
            onChange={(event) => {
              setSupplierId(
                event.target.value,
              );
            }}
            required
            disabled={
              createPurchaseOrder.isPending
            }
            className="w-full max-w-xl rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          >
            <option value="">
              Select a supplier
            </option>

            {activeSuppliers.map(
              (supplier) => (
                <option
                  key={supplier.id}
                  value={supplier.id}
                >
                  {supplier.name} —{" "}
                  {supplier.taxIdentifier}
                </option>
              ),
            )}
          </select>
        </div>

        <section>
          <div className="flex items-center justify-between gap-4">
            <div>
              <h2 className="text-lg font-semibold text-slate-900">
                Order Items
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                Add the products included in this purchase order.
              </p>
            </div>

            <button
              type="button"
              onClick={addItem}
              disabled={
                createPurchaseOrder.isPending
              }
              className="inline-flex items-center gap-2 rounded-lg border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50"
            >
              <Plus size={16} />
              Add Item
            </button>
          </div>

          <div className="mt-4 space-y-4">
            {items.map((item, index) => (
              <PurchaseOrderItemForm
                key={index}
                item={item}
                products={activeProducts}
                canRemove={
                  items.length > 1
                }
                onChange={(
                  updatedItem,
                ) => {
                  updateItem(
                    index,
                    updatedItem,
                  );
                }}
                onRemove={() => {
                  removeItem(index);
                }}
              />
            ))}
          </div>
        </section>

        <div className="flex justify-end">
          <div className="w-full max-w-sm rounded-xl bg-slate-50 p-5">
            <div className="flex items-center justify-between">
              <span className="font-medium text-slate-600">
                Order Total
              </span>

              <span className="text-xl font-semibold text-slate-900">
                {formatCurrency(total)}
              </span>
            </div>
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
              navigate("/purchasing");
            }}
            disabled={
              createPurchaseOrder.isPending
            }
            className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50"
          >
            Cancel
          </button>

          <button
            type="submit"
            disabled={
              createPurchaseOrder.isPending
            }
            className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {createPurchaseOrder.isPending
              ? "Creating..."
              : "Create Purchase Order"}
          </button>
        </div>
      </form>
    </div>
  );
}