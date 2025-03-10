import { useState, useCallback } from 'preact/hooks'
import { executeWithDelayOnTimeout } from '@utils/executeWithDelayOnTimeout.ts'

export const useExecuteWithLoader = (callback: () => Promise<void>, timeout: number = 300, minDelay: number = 1000) => {
  const [isLoading, setIsLoading] = useState(false)
  const [showSpinner, setShowSpinner] = useState(false)

  const execute = useCallback(async () => {
    setIsLoading(true)
    setShowSpinner(false)

    await executeWithDelayOnTimeout(
      callback().finally(() => {
        setIsLoading(false)
        setShowSpinner(false)
      }),
      timeout,
      minDelay,
      () => setShowSpinner(true),
    )
  }, [callback, timeout, minDelay])

  return { isLoading, showSpinner, execute }
}
