import { ApiError } from '@microsoft/kiota-abstractions'
import { ProblemDetails, RedirectProblemDetails } from '@api/models'
import { useToast } from '@components/Toast/ToastContext.tsx'
import { useCallback } from 'preact/hooks'

interface Result {
  isBackendUnavailable: boolean
}

export const useErrorHandler = () => {
  const { showError } = useToast()

  const handleError = useCallback(
    (
      error: string | Error | ApiError | ProblemDetails | RedirectProblemDetails,
      showToastFor401: boolean = true,
    ): Result => {
      if (typeof error === 'string') {
        showError(error, undefined, 5000)
        return { isBackendUnavailable: false }
      }

      if (!('responseStatusCode' in error)) {
        // eslint-disable-next-line no-console
        console.error('JS error', error)
        showError(error.message, undefined, 5000)
        return { isBackendUnavailable: false }
      }

      if (
        !navigator.onLine ||
        error.responseStatusCode === 503 ||
        error.responseStatusCode === 502 ||
        error.responseStatusCode === 504
      ) {
        showError('Backend is unavailable', undefined, 5000)
        return { isBackendUnavailable: true }
      }

      if (error.responseStatusCode === 401 && 'redirectUrl' in error) {
        const redirectUrl = new URL(error.redirectUrl as string, window.location.origin)
        redirectUrl.searchParams.set('returnUrl', window.location.href)

        if (showToastFor401) {
          showError('You are not authenticated', redirectUrl.toString(), 0, 'Login')
        } else {
          window.location.href = redirectUrl.toString()
        }
        return { isBackendUnavailable: false }
      }

      if ('detail' in error) {
        showError(error.detail as string, undefined, 7000)
      } else {
        showError(`HTTP error: ${error.responseStatusCode}`, undefined, 5000)
      }

      return { isBackendUnavailable: false }
    },
    [showError],
  )

  return {
    handleError,
  }
}
