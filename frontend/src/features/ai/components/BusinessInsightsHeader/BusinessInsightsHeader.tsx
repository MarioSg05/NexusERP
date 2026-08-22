import { Sparkles } from "lucide-react";

interface BusinessInsightsHeaderProps {
  hasInsights: boolean;
  isGenerating: boolean;
  onGenerate: () => void;
}

export function BusinessInsightsHeader({
  hasInsights,
  isGenerating,
  onGenerate,
}: BusinessInsightsHeaderProps) {
  return (
    <header className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
      <div>
        <h1 className="text-3xl font-bold tracking-tight text-slate-900">
          Business Insights
        </h1>

        <p className="mt-2 text-slate-500">
          Review deterministic ERP insights and an optional locally generated AI summary.
        </p>
      </div>

      <button
        type="button"
        onClick={onGenerate}
        disabled={isGenerating}
        className="inline-flex items-center justify-center gap-2 rounded-lg bg-blue-600 px-4 py-2.5 text-sm font-semibold text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-60"
      >
        <Sparkles size={17} />

        {isGenerating
          ? "Generating..."
          : hasInsights
            ? "Regenerate"
            : "Generate Insights"}
      </button>
    </header>
  );
}