import { Plus } from "lucide-react";
import { Link } from "react-router-dom";
import { useAuth } from "../../../auth/hooks/useAuth";

export function InventoryHeader() {
    const { canManageErp } = useAuth();
    return (
        <header className="flex items-start justify-between gap-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight text-slate-900">
                    Inventory
                </h1>

                <p className="mt-2 text-slate-500">
                    Monitor stock levels and manage product inventory.
                </p>
            </div>

            {canManageErp && (
                <Link
                    to="/inventory/new"
                    className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700"
                >
                    <Plus size={18} />
                    Create Inventory
                </Link>
            )}
        </header>
    );
}
