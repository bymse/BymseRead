export async function executeWithDelayOnTimeout(
  promise: Promise<void>,
  timeout: number,
  minDelay: number,
  onTimeout: () => void | Promise<void>,
): Promise<void> {
  const startTime = Date.now()
  let timeoutOccured = false
  let isPromiseFinished = false

  const timer = setTimeout(() => {
    timeoutOccured = true
    if (!isPromiseFinished) {
      void onTimeout()
    }
  }, timeout)

  const result = await promise
  isPromiseFinished = true
  clearTimeout(timer)

  const elapsed = Date.now() - startTime
  const remainingDelay = Math.max(0, minDelay - elapsed)

  if (timeoutOccured && remainingDelay > 0) {
    await new Promise(resolve => setTimeout(resolve, minDelay))
  }

  return result
}
