import {
  useMemo,
  useState,
  type FormEvent,
} from "react";
import axios from "axios";
import { Plus } from "lucide-react";
import { useNavigate } from "react-router-dom";

import { SalesOrderItemForm } from "../components/SalesOrderItemForm/SalesOrderItemForm";

import { useCustomers } from "../../customers/hooks/useCustomers";
import { useInventory } from "../../inventory/hooks/useInventory";
import { useProducts } from "../../products/hooks/useProducts";
import { useCreateSalesOrder } from "../hooks/useCreateSalesOrder";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";
import { formatCurrency } from "../../../shared/lib/formatCurrency";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { CreateSalesOrderItemRequest } from "../models/CreateSalesOrderModel";

const createEmptyItem =
  (): CreateSalesOrderItemRequest => ({
    productId: "",
    quantity: 1,
    unitPrice: 0,
  });

export function NewSalesOrderPage() {
  const navigate = useNavigate();

  const [customerId, setCustomerId] =
    useState("");

  const [items, setItems] = useState<
    CreateSalesOrderItemRequest[]
  >([createEmptyItem()]);

  const [submitError, setSubmitError] =
    useState<string | null>(null);

  const {
    data: customers,
    isLoading: isLoadingCustomers,
    error: customersError,
    refetch: refetchCustomers,
  } = useCustomers();

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

  const createSalesOrder =
    useCreateSalesOrder();

  const activeCustomers =
    customers?.filter(
      (customer) => customer.isActive,
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
    isLoadingCustomers ||
    isLoadingProducts ||
    isLoadingInventory;

  if (isLoading) {
    return (
      <p className="text-sm text-slate-500">
        Loading sales order data...
      </p>
    );
  }

  if (
    customersError ||
    productsError ||
    inventoryError ||
    !customers ||
    !products ||
    !inventory
  ) {
    return (
      <QueryErrorState
        title="Unable to prepare sales order"
        description="We couldn't retrieve the customers, products, and inventory required to create a sales order."
        onRetry={() => {
          void Promise.all([
            refetchCustomers(),
            refetchProducts(),
            refetchInventory(),
          ]);
        }}
      />
    );
  }

  function updateItem(
    index: number,
    updatedItem: CreateSalesOrderItemRequest,
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
        "A product can only appear once in a sales order.",
      );

      return;
    }

    try {
      const response =
        await createSalesOrder.mutateAsync({
          customerId,
          items,
        });

      navigate(
        `/sales/${response.id}`,
      );
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setSubmitError(
          error.response?.data.detail ??
            "Unable to create sales order.",
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
          New Sales Order
        </h1>

        <p className="mt-2 text-slate-500">
          Create a sales order for a customer.
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
            htmlFor="sales-order-customer"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Customer
          </label>

          <select
            id="sales-order-customer"
            value={customerId}
            onChange={(event) => {
              setCustomerId(
                event.target.value,
              );
            }}
            required
            disabled={
              createSalesOrder.isPending
            }
            className="w-full max-w-xl rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
          >
            <option value="">
              Select a customer
            </option>

            {activeCustomers.map(
              (customer) => (
                <option
                  key={customer.id}
                  value={customer.id}
                >
                  {customer.name} —{" "}
                  {customer.email}
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
                Add the products included in this sales order.
              </p>
            </div>

            <button
              type="button"
              onClick={addItem}
              disabled={
                createSalesOrder.isPending
              }
              className="inline-flex items-center gap-2 rounded-lg border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:opacity-50"
            >
              <Plus size={16} />
              Add Item
            </button>
          </div>

          <div className="mt-4 space-y-4">
            {items.map((item, index) => (
              <SalesOrderItemForm
                key={index}
                item={item}
                products={activeProducts}
                inventory={inventory}
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
              navigate("/sales");
            }}
            disabled={
              createSalesOrder.isPending
            }
            className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50 disabled:opacity-50"
          >
            Cancel
          </button>

          <button
            type="submit"
            disabled={
              createSalesOrder.isPending
            }
            className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
          >
            {createSalesOrder.isPending
              ? "Creating..."
              : "Create Sales Order"}
          </button>
        </div>
      </form>
    </div>
  );
}