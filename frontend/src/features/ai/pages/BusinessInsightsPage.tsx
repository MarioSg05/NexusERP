import {
  Boxes,
  CircleAlert,
  ReceiptText,
  ShoppingCart,
  Sparkles,
} from "lucide-react";

import { AiSummaryCard } from "../components/AiSummaryCard/AiSummaryCard";
import { BusinessInsightsHeader } from "../components/BusinessInsightsHeader/BusinessInsightsHeader";
import { BusinessInsightsSection } from "../components/BusinessInsightsSection/BusinessInsightsSection";
import { BusinessInsightsSkeleton } from "../components/BusinessInsightsSkeleton/BusinessInsightsSkeleton";
import { useBusinessInsights } from "../hooks/useBusinessInsights";
import { useGenerateBusinessInsights } from "../hooks/useGenerateBusinessInsights";

export function BusinessInsightsPage() {
  const {
    data: insights,
  } = useBusinessInsights();

  const generateInsights =
    useGenerateBusinessInsights();

  function handleGenerate() {
    generateInsights.mutate();
  }

  const isInitialGeneration =
    generateInsights.isPending &&
    !insights;

  return (
    <>
      <BusinessInsightsHeader
        hasInsights={Boolean(insights)}
        isGenerating={
          generateInsights.isPending
        }
        onGenerate={handleGenerate}
      />

      {!insights &&
        !generateInsights.isPending &&
        !generateInsights.isError && (
          <section className="rounded-xl border border-dashed border-slate-300 bg-slate-50 px-6 py-14 text-center">
            <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-xl bg-white text-blue-600 shadow-sm">
              <Sparkles size={22} />
            </div>

            <h2 className="mt-4 text-lg font-semibold text-slate-900">
              Generate business insights
            </h2>

            <p className="mx-auto mt-2 max-w-xl text-sm leading-6 text-slate-500">
              NexusERP will analyze the current business snapshot and optionally enhance it with a locally generated AI summary.
            </p>
          </section>
        )}

      {isInitialGeneration && (
        <BusinessInsightsSkeleton />
      )}

      {generateInsights.isError &&
        !insights && (
          <section className="rounded-xl border border-red-200 bg-red-50 p-6">
            <div className="flex gap-3">
              <CircleAlert
                size={20}
                className="mt-0.5 shrink-0 text-red-600"
              />

              <div>
                <h2 className="font-semibold text-red-900">
                  Unable to generate business insights
                </h2>

                <p className="mt-1 text-sm leading-6 text-red-700">
                  NexusERP couldn't generate the business snapshot. Check your connection and try again.
                </p>

                <button
                  type="button"
                  onClick={handleGenerate}
                  className="mt-4 text-sm font-semibold text-red-800 underline underline-offset-4"
                >
                  Try again
                </button>
              </div>
            </div>
          </section>
        )}

      {generateInsights.isError &&
        insights && (
          <section className="mb-6 rounded-xl border border-red-200 bg-red-50 p-4">
            <div className="flex gap-3">
              <CircleAlert
                size={18}
                className="mt-0.5 shrink-0 text-red-600"
              />

              <div>
                <p className="text-sm font-medium text-red-900">
                  Unable to refresh business insights
                </p>

                <p className="mt-1 text-sm text-red-700">
                  The previous business insights remain available.
                </p>
              </div>
            </div>
          </section>
        )}

      {insights && (
        <>
          <section>
            <div className="mb-4">
              <h2 className="text-xl font-semibold text-slate-900">
                Current Business Snapshot
              </h2>

              <p className="mt-1 text-sm text-slate-500">
                Verified insights calculated directly from current NexusERP data.
              </p>
            </div>

            <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">
              <BusinessInsightsSection
                title="Inventory"
                items={insights.inventory}
                icon={Boxes}
              />

              <BusinessInsightsSection
                title="Sales"
                items={insights.sales}
                icon={ReceiptText}
              />

              <BusinessInsightsSection
                title="Purchasing"
                items={insights.purchasing}
                icon={ShoppingCart}
              />
            </div>
          </section>

          <section className="mt-6 rounded-xl border border-amber-200 bg-amber-50 p-6">
            <div className="flex gap-3">
              <CircleAlert
                size={20}
                className="mt-0.5 shrink-0 text-amber-700"
              />

              <div>
                <h2 className="font-semibold text-amber-950">
                  Areas Requiring Attention
                </h2>

                <ul className="mt-3 space-y-2">
                  {insights.attentionAreas.map(
                    (item) => (
                      <li
                        key={item}
                        className="text-sm leading-6 text-amber-900"
                      >
                        {item}
                      </li>
                    ),
                  )}
                </ul>
              </div>
            </div>
          </section>

          <div className="mt-6">
            <AiSummaryCard
              summary={insights.aiSummary}
            />
          </div>
        </>
      )}
    </>
  );
}