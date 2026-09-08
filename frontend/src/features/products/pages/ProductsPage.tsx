import { ProductsEmptyState } from "../components/ProductsEmptyState/ProductsEmptyState";
import { ProductsHeader } from "../components/ProductsHeader/ProductsHeader";
import { ProductsSkeleton } from "../components/ProductsSkeleton/ProductsSkeleton";
import { ProductsTable } from "../components/ProductsTable/ProductsTable";
import { useProducts } from "../hooks/useProducts";

import { QueryErrorState } from "../../../shared/components/feedback/QueryErrorState/QueryErrorState";

export function ProductsPage() {
  const {
    data: products,
    isLoading,
    error,
    refetch,
  } = useProducts();

  if (isLoading) {
    return <ProductsSkeleton />;
  }

  if (error || !products) {
    return (
      <QueryErrorState
        title="Unable to load products"
        description="We couldn't retrieve the product information. Check your connection and try again."
        onRetry={() => {
          void refetch();
        }}
      />
    );
  }

  return (
    <>
      <ProductsHeader />

      <div className="mt-8">
        {products.length === 0 ? (
          <ProductsEmptyState />
        ) : (
          <ProductsTable products={products} />
        )}
      </div>
    </>
  );
}