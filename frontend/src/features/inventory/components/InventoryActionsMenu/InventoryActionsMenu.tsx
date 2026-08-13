import {
    useEffect,
    useRef,
    useState,
} from "react";
import { createPortal } from "react-dom";
import {
    Minus,
    MoreHorizontal,
    Plus,
    SlidersHorizontal,
} from "lucide-react";

import type { InventoryStockMode } from "../InventoryStockDialog/InventoryStockDialog";

interface InventoryActionsMenuProps {
    onSelect: (
        mode: InventoryStockMode,
    ) => void;
}

interface MenuPosition {
    top: number;
    left: number;
}

const MENU_WIDTH = 192;
const MENU_HEIGHT = 122;
const MENU_GAP = 8;
const VIEWPORT_PADDING = 8;

export function InventoryActionsMenu({
    onSelect,
}: InventoryActionsMenuProps) {
    const [isOpen, setIsOpen] =
        useState(false);

    const [menuPosition, setMenuPosition] =
        useState<MenuPosition>({
            top: 0,
            left: 0,
        });

    const buttonRef =
        useRef<HTMLButtonElement>(null);

    const menuRef =
        useRef<HTMLDivElement>(null);

    function updateMenuPosition() {
        const button =
            buttonRef.current;

        if (!button) {
            return;
        }

        const rect =
            button.getBoundingClientRect();

        const spaceBelow =
            window.innerHeight -
            rect.bottom;

        const shouldOpenUpward =
            spaceBelow <
            MENU_HEIGHT +
                MENU_GAP +
                VIEWPORT_PADDING;

        const top = shouldOpenUpward
            ? rect.top -
              MENU_HEIGHT -
              MENU_GAP
            : rect.bottom +
              MENU_GAP;

        const desiredLeft =
            rect.right - MENU_WIDTH;

        const left = Math.max(
            VIEWPORT_PADDING,
            Math.min(
                desiredLeft,
                window.innerWidth -
                    MENU_WIDTH -
                    VIEWPORT_PADDING,
            ),
        );

        setMenuPosition({
            top: Math.max(
                VIEWPORT_PADDING,
                top,
            ),
            left,
        });
    }

    function handleToggle() {
        if (!isOpen) {
            updateMenuPosition();
        }

        setIsOpen(
            (current) => !current,
        );
    }

    function handleSelect(
        mode: InventoryStockMode,
    ) {
        setIsOpen(false);
        onSelect(mode);
    }

    useEffect(() => {
        if (!isOpen) {
            return;
        }

        function handlePointerDown(
            event: PointerEvent,
        ) {
            const target =
                event.target as Node;

            const clickedButton =
                buttonRef.current?.contains(
                    target,
                );

            const clickedMenu =
                menuRef.current?.contains(
                    target,
                );

            if (
                !clickedButton &&
                !clickedMenu
            ) {
                setIsOpen(false);
            }
        }

        function handleKeyDown(
            event: KeyboardEvent,
        ) {
            if (event.key === "Escape") {
                setIsOpen(false);
            }
        }

        function handleViewportChange() {
            updateMenuPosition();
        }

        document.addEventListener(
            "pointerdown",
            handlePointerDown,
        );

        window.addEventListener(
            "keydown",
            handleKeyDown,
        );

        window.addEventListener(
            "resize",
            handleViewportChange,
        );

        window.addEventListener(
            "scroll",
            handleViewportChange,
            true,
        );

        return () => {
            document.removeEventListener(
                "pointerdown",
                handlePointerDown,
            );

            window.removeEventListener(
                "keydown",
                handleKeyDown,
            );

            window.removeEventListener(
                "resize",
                handleViewportChange,
            );

            window.removeEventListener(
                "scroll",
                handleViewportChange,
                true,
            );
        };
    }, [isOpen]);

    return (
        <>
            <button
                ref={buttonRef}
                type="button"
                onClick={handleToggle}
                aria-label="Inventory actions"
                aria-haspopup="menu"
                aria-expanded={isOpen}
                className="inline-flex h-9 w-9 items-center justify-center rounded-lg text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900"
            >
                <MoreHorizontal size={18} />
            </button>

            {isOpen &&
                createPortal(
                    <div
                        ref={menuRef}
                        role="menu"
                        style={{
                            top: menuPosition.top,
                            left: menuPosition.left,
                            width: MENU_WIDTH,
                        }}
                        className="fixed z-50 overflow-hidden rounded-lg border border-slate-200 bg-white py-1 shadow-lg"
                    >
                        <button
                            type="button"
                            role="menuitem"
                            onClick={() => {
                                handleSelect(
                                    "increase",
                                );
                            }}
                            className="flex w-full items-center gap-3 px-4 py-2 text-left text-sm text-slate-700 transition-colors hover:bg-slate-50"
                        >
                            <Plus size={16} />
                            Increase stock
                        </button>

                        <button
                            type="button"
                            role="menuitem"
                            onClick={() => {
                                handleSelect(
                                    "decrease",
                                );
                            }}
                            className="flex w-full items-center gap-3 px-4 py-2 text-left text-sm text-slate-700 transition-colors hover:bg-slate-50"
                        >
                            <Minus size={16} />
                            Decrease stock
                        </button>

                        <button
                            type="button"
                            role="menuitem"
                            onClick={() => {
                                handleSelect(
                                    "adjust",
                                );
                            }}
                            className="flex w-full items-center gap-3 px-4 py-2 text-left text-sm text-slate-700 transition-colors hover:bg-slate-50"
                        >
                            <SlidersHorizontal
                                size={16}
                            />
                            Adjust stock
                        </button>
                    </div>,
                    document.body,
                )}
        </>
    );
}