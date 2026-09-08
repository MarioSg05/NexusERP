import {
  Bot,
  CircleAlert,
} from "lucide-react";

interface AiSummaryCardProps {
  summary: string | null;
}

export function AiSummaryCard({
  summary,
}: AiSummaryCardProps) {
  return (
    <section className="rounded-xl border border-slate-200 bg-white p-6">
      <div className="flex items-center gap-3">
        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-50 text-blue-700">
          <Bot size={20} />
        </div>

        <div>
          <h2 className="text-lg font-semibold text-slate-900">
            AI Summary
          </h2>

          <p className="text-sm text-slate-500">
            Generated locally with Ollama.
          </p>
        </div>
      </div>

      {summary ? (
        <p className="mt-5 whitespace-pre-line text-sm leading-7 text-slate-700">
          {summary}
        </p>
      ) : (
        <div className="mt-5 rounded-lg bg-slate-50 p-4">
          <div className="flex gap-3">
            <CircleAlert
              size={18}
              className="mt-0.5 shrink-0 text-slate-500"
            />

            <div>
              <p className="text-sm font-medium text-slate-700">
                AI summary unavailable
              </p>

              <p className="mt-1 text-sm leading-6 text-slate-500">
                The deterministic business insights remain available because they are calculated directly by NexusERP.
              </p>
            </div>
          </div>
        </div>
      )}

      <p className="mt-5 border-t border-slate-100 pt-4 text-xs leading-5 text-slate-400">
        AI-generated summaries may contain inaccuracies. Use the verified business snapshot above as the source of truth.
      </p>
    </section>
  );
}