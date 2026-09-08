import { useState } from "react";
import axios from "axios";

import { InventoryEmptyState } from "../components/InventoryEmptyState/InventoryEmptyState";
import { InventoryHeader } from "../components/InventoryHeader/InventoryHeader";
import { InventorySkeleton } from "../components/InventorySkeleton/InventorySkeleton";
import {
  InventoryStockDialog,
  type InventoryStockMode,
} from "../components/InventoryStockDialog/InventoryStockDialog";
import { InventoryTable } from "../components/InventoryTable/InventoryTable";

import { useAdjustInventoryStock } from "../hooks/useAdjustInventoryStock";
import { useDecreaseInventoryStock } from "../hooks/useDecreaseInventoryStock";
import { useIncreaseInventoryStock } from "../hooks/useIncreaseInventoryStock";
import { useInventory } from "../hooks/useInventory";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

import type { ApiProblemDetails } from "../../../shared/api/ApiProblemDetails";
import type { InventoryItem } from "../models/InventoryModel";

interface SelectedStockAction {
  item: InventoryItem;
  mode: InventoryStockMode;
}

export function InventoryPage() {
  const [selectedAction, setSelectedAction] =
    useState<SelectedStockAction | null>(null);

  const [stockError, setStockError] =
    useState<string | null>(null);

  const {
    data: inventory,
    isLoading,
    error,
    refetch,
  } = useInventory();

  const increaseStock =
    useIncreaseInventoryStock();

  const decreaseStock =
    useDecreaseInventoryStock();

  const adjustStock =
    useAdjustInventoryStock();

  if (isLoading) {
    return <InventorySkeleton />;
  }

  if (error || !inventory) {
    return (
      <QueryErrorState
        title="Unable to load inventory"
        description="We couldn't retrieve the inventory information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  const isStockMutationPending =
    increaseStock.isPending ||
    decreaseStock.isPending ||
    adjustStock.isPending;

  function handleStockAction(
    item: InventoryItem,
    mode: InventoryStockMode,
  ) {
    setStockError(null);

    setSelectedAction({
      item,
      mode,
    });
  }

  function handleCloseDialog() {
    if (isStockMutationPending) {
      return;
    }

    setStockError(null);
    setSelectedAction(null);
  }

  async function handleStockSubmit(
    quantity: number,
  ) {
    if (!selectedAction) {
      return;
    }

    setStockError(null);

    const variables = {
      id: selectedAction.item.id,
      request: {
        quantity,
      },
    };

    try {
      switch (selectedAction.mode) {
        case "increase":
          await increaseStock.mutateAsync(
            variables,
          );
          break;

        case "decrease":
          await decreaseStock.mutateAsync(
            variables,
          );
          break;

        case "adjust":
          await adjustStock.mutateAsync(
            variables,
          );
          break;
      }

      setSelectedAction(null);
    } catch (error) {
      if (
        axios.isAxiosError<ApiProblemDetails>(
          error,
        )
      ) {
        setStockError(
          error.response?.data.detail ??
            "Unable to update inventory stock.",
        );

        return;
      }

      setStockError(
        "An unexpected error occurred.",
      );
    }
  }

  return (
    <>
      <InventoryHeader />

      <div className="mt-8">
        {inventory.length === 0 ? (
          <InventoryEmptyState />
        ) : (
          <InventoryTable
            inventory={inventory}
            onStockAction={handleStockAction}
          />
        )}
      </div>

      {selectedAction && (
        <InventoryStockDialog
          item={selectedAction.item}
          mode={selectedAction.mode}
          isSubmitting={isStockMutationPending}
          errorMessage={stockError}
          onSubmit={handleStockSubmit}
          onClose={handleCloseDialog}
        />
      )}
    </>
  );
}