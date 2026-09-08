import {
  useState,
  type FormEvent,
} from "react";

export interface SupplierFormSubmitValues {
  name: string;
  taxIdentifier: string;
  email: string | null;
  phone: string | null;
}

interface SupplierFormValues {
  name: string;
  taxIdentifier: string;
  email: string | null;
  phone: string | null;
}

interface SupplierFormProps {
  initialValues?: SupplierFormValues;
  lockIdentity?: boolean;
  submitLabel?: string;
  isSubmitting?: boolean;
  errorMessage?: string | null;
  onSubmit: (
    values: SupplierFormSubmitValues,
  ) => void | Promise<void>;
  onCancel: () => void;
}

export function SupplierForm({
  initialValues,
  lockIdentity = false,
  submitLabel = "Save Supplier",
  isSubmitting = false,
  errorMessage,
  onSubmit,
  onCancel,
}: SupplierFormProps) {
  const [name, setName] =
    useState(
      initialValues?.name ?? "",
    );

  const [
    taxIdentifier,
    setTaxIdentifier,
  ] = useState(
    initialValues?.taxIdentifier ?? "",
  );

  const [email, setEmail] =
    useState(
      initialValues?.email ?? "",
    );

  const [phone, setPhone] =
    useState(
      initialValues?.phone ?? "",
    );

  async function handleSubmit(
    event: FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    const normalizedEmail =
      email.trim();

    const normalizedPhone =
      phone.trim();

    await onSubmit({
      name: name.trim(),
      taxIdentifier:
        taxIdentifier.trim(),
      email:
        normalizedEmail.length > 0
          ? normalizedEmail
          : null,
      phone:
        normalizedPhone.length > 0
          ? normalizedPhone
          : null,
    });
  }

  return (
    <form
      onSubmit={(event) => {
        void handleSubmit(event);
      }}
      className="mt-8 space-y-6"
    >
      <div>
        <label
          htmlFor="supplierName"
          className="block text-sm font-medium text-slate-700"
        >
          Supplier Name
        </label>

        <input
          id="supplierName"
          type="text"
          value={name}
          onChange={(event) => {
            setName(event.target.value);
          }}
          required
          disabled={
            isSubmitting ||
            lockIdentity
          }
          className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
        />
      </div>

      <div>
        <label
          htmlFor="taxIdentifier"
          className="block text-sm font-medium text-slate-700"
        >
          Tax Identifier
        </label>

        <input
          id="taxIdentifier"
          type="text"
          value={taxIdentifier}
          onChange={(event) => {
            setTaxIdentifier(
              event.target.value,
            );
          }}
          required
          disabled={
            isSubmitting ||
            lockIdentity
          }
          className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
        />

        {lockIdentity && (
          <p className="mt-1 text-xs text-slate-500">
            Tax Identifier cannot be
            changed after registration.
          </p>
        )}
      </div>

      <div>
        <label
          htmlFor="supplierEmail"
          className="block text-sm font-medium text-slate-700"
        >
          Email
        </label>

        <input
          id="supplierEmail"
          type="email"
          value={email}
          onChange={(event) => {
            setEmail(event.target.value);
          }}
          disabled={isSubmitting}
          autoComplete="email"
          className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
        />

        <p className="mt-1 text-xs text-slate-500">
          Optional.
        </p>
      </div>

      <div>
        <label
          htmlFor="supplierPhone"
          className="block text-sm font-medium text-slate-700"
        >
          Phone
        </label>

        <input
          id="supplierPhone"
          type="tel"
          value={phone}
          onChange={(event) => {
            setPhone(event.target.value);
          }}
          disabled={isSubmitting}
          className="mt-2 w-full rounded-lg border border-slate-300 px-3 py-2 outline-none transition focus:border-blue-500 focus:ring-2 focus:ring-blue-100 disabled:cursor-not-allowed disabled:bg-slate-100"
        />

        <p className="mt-1 text-xs text-slate-500">
          Optional.
        </p>
      </div>

      {errorMessage && (
        <div
          role="alert"
          className="rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700"
        >
          {errorMessage}
        </div>
      )}

      <div className="flex justify-end gap-3">
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
          {isSubmitting
            ? "Saving..."
            : submitLabel}
        </button>
      </div>
    </form>
  );
}