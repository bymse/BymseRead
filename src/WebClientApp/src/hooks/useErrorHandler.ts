import { ApiError } from '@microsoft/kiota-abstractions'
import { ProblemDetails, RedirectProblemDetails } from '@api/models'
import { useToast } from '@components/Toast/ToastContext.tsx'
import { useCallback } from 'preact/hooks'

export const useErrorHandler = () => {
  const { showError } = useToast()

  const handleError = useCallback(
    (error: string | Error | ApiError | ProblemDetails | RedirectProblemDetails, showToastFor401: boolean = true) => {
      if (typeof error === 'string') {
        showError(error, undefined, 5000)
        return
      }

      if (!('responseStatusCode' in error)) {
        // eslint-disable-next-line no-console
        console.error('JS error', error)
        showError(error.message, undefined, 5000)
        return
      }

      if (error.responseStatusCode === 401 && 'redirectUrl' in error) {
        const redirectUrl = new URL(error.redirectUrl as string, window.location.origin)
        redirectUrl.searchParams.set('returnUrl', window.location.href)

        if (showToastFor401) {
          showError('You are not authenticated', redirectUrl.toString(), 0, 'Login')
        } else {
          window.location.href = redirectUrl.toString()
        }
        return
      }

      if ('detail' in error) {
        showError(error.detail as string, undefined, 7000)
      } else {
        showError(`HTTP error: ${error.responseStatusCode}`, undefined, 5000)
      }
    },
    [showError],
  )

  return {
    handleError,
  }
}
