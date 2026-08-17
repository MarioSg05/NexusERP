const UNAUTHORIZED_EVENT =
  "nexuserp:unauthorized";

export function notifyUnauthorized() {
  window.dispatchEvent(
    new Event(UNAUTHORIZED_EVENT),
  );
}

export function subscribeToUnauthorized(
  listener: () => void,
) {
  window.addEventListener(
    UNAUTHORIZED_EVENT,
    listener,
  );

  return () => {
    window.removeEventListener(
      UNAUTHORIZED_EVENT,
      listener,
    );
  };
}