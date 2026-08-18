import { Plus } from "lucide-react";
import { Link } from "react-router-dom";
import { useAuth } from "../../../auth/hooks/useAuth";

export function CustomersHeader() {
    const { canManageErp } = useAuth();

    return (
        <header className="flex items-start justify-between gap-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight text-slate-900">
                    Customers
                </h1>

                <p className="mt-2 text-slate-500">
                    Manage customer information and relationships.
                </p>
            </div>

            {canManageErp && (
                <Link
                    to="/customers/new"
                    className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700"
                >
                    <Plus size={18} />
                    New Customer
                </Link>
            )}
        </header>
    );
}
