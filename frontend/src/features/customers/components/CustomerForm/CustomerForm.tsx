import { useState, type FormEvent } from "react";

import type {
    CustomerType,
    RegisterCustomerRequest,
} from "../../models/RegisterCustomerModel";

interface CustomerFormProps {
    initialValues?: CustomerFormInitialValues;
    submitLabel?: string;
    isSubmitting: boolean;
    errorMessage?: string | null;
    onSubmit: (request: RegisterCustomerRequest) => Promise<void>;
    onCancel: () => void;
}

interface CustomerFormInitialValues {
    name: string;
    email: string;
    phone: string;
    type: CustomerType;
}

export function CustomerForm({
    initialValues,
    submitLabel = "Create Customer",
    isSubmitting,
    errorMessage,
    onSubmit,
    onCancel,
}: CustomerFormProps) {
    const [name, setName] = useState(initialValues?.name ?? "");

    const [email, setEmail] = useState(initialValues?.email ?? "");

    const [phone, setPhone] = useState(initialValues?.phone ?? "");

    const [type, setType] = useState<CustomerType>(initialValues?.type ?? 1);

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        await onSubmit({
            name: name.trim(),
            email: email.trim(),
            phone: phone.trim() || null,
            type,
        });
    }

    return (
        <form
            onSubmit={(event) => {
                void handleSubmit(event);
            }}
            className="mt-8 space-y-6"
        >
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                <div>
                    <label
                        htmlFor="customer-name"
                        className="mb-2 block text-sm font-medium text-slate-700"
                    >
                        Customer name
                    </label>

                    <input
                        id="customer-name"
                        type="text"
                        value={name}
                        onChange={(event) => {
                            setName(event.target.value);
                        }}
                        required
                        maxLength={200}
                        disabled={isSubmitting}
                        className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
                    />
                </div>

                <div>
                    <label
                        htmlFor="customer-email"
                        className="mb-2 block text-sm font-medium text-slate-700"
                    >
                        Email
                    </label>

                    <input
                        id="customer-email"
                        type="email"
                        value={email}
                        onChange={(event) => {
                            setEmail(event.target.value);
                        }}
                        required
                        disabled={isSubmitting}
                        className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
                    />
                </div>

                <div>
                    <label
                        htmlFor="customer-phone"
                        className="mb-2 block text-sm font-medium text-slate-700"
                    >
                        Phone
                    </label>

                    <input
                        id="customer-phone"
                        type="tel"
                        value={phone}
                        onChange={(event) => {
                            setPhone(event.target.value);
                        }}
                        maxLength={25}
                        disabled={isSubmitting}
                        placeholder="+502 5555 1234"
                        className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
                    />
                </div>

                <div>
                    <label
                        htmlFor="customer-type"
                        className="mb-2 block text-sm font-medium text-slate-700"
                    >
                        Customer type
                    </label>

                    <select
                        id="customer-type"
                        value={type}
                        onChange={(event) => {
                            setType(Number(event.target.value) as CustomerType);
                        }}
                        disabled={isSubmitting}
                        className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
                    >
                        <option value={1}>Individual</option>

                        <option value={2}>Corporate</option>
                    </select>
                </div>
            </div>

            {errorMessage && (
                <div
                    role="alert"
                    className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
                >
                    {errorMessage}
                </div>
            )}

            <div className="flex justify-end gap-3 border-t border-slate-200 pt-6">
                <button
                    type="button"
                    onClick={onCancel}
                    disabled={isSubmitting}
                    className="rounded-lg border border-slate-300 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50"
                >
                    Cancel
                </button>

                <button
                    type="submit"
                    disabled={isSubmitting}
                    className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
                >
                    {isSubmitting ? "Saving..." : submitLabel}
                </button>
            </div>
        </form>
    );
}
