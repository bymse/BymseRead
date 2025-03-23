import { ApiError } from '@microsoft/kiota-abstractions'
import { ProblemDetails, RedirectProblemDetails } from '@api/models'
import { useToast } from '@components/Toast/ToastContext.tsx'
import { useCallback } from 'preact/hooks'

export const useErrorHandler = () => {
  const { showError } = useToast()

  const handleError = useCallback(
    (error: string | Error | ApiError | ProblemDetails | RedirectProblemDetails) => {
      if (typeof error === 'string') {
        showError(error, 5000)
        return
      }

      if (!('responseStatusCode' in error)) {
        // eslint-disable-next-line no-console
        console.error('JS error', error)
        showError(error.message, 5000)
        return
      }

      if ('redirectUrl' in error) {
        window.location.href = error.redirectUrl as string
        return
      }

      if ('detail' in error) {
        showError(error.detail as string, 7000)
      } else {
        showError(`HTTP error: ${error.responseStatusCode}`, 5000)
      }
    },
    [showError],
  )

  return {
    handleError,
  }
}
