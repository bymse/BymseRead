export async function executeWithDelayOnTimeout(
  promise: Promise<void>,
  timeout: number,
  minDelay: number,
  onTimeout: () => void | Promise<void>,
): Promise<void> {
  const startTime = Date.now()
  let timeoutOccured = false

  const timer = setTimeout(() => {
    timeoutOccured = true
    void onTimeout()
  }, timeout)

  const result = await promise
  clearTimeout(timer)

  const elapsed = Date.now() - startTime
  const remainingDelay = Math.max(0, minDelay - elapsed)

  if (timeoutOccured && remainingDelay > 0) {
    await new Promise(resolve => setTimeout(resolve, remainingDelay))
  }

  return result
}
