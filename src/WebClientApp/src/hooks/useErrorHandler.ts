import { ApiError } from '@microsoft/kiota-abstractions'
import { ProblemDetails } from '@api/models'
import { useToast } from '@components/Toast/ToastContext.tsx'
import { AuthHandler } from '@utils/authHandler.ts'
import { useCallback } from 'preact/hooks'

interface Result {
  isBackendUnavailable: boolean
}

export const useErrorHandler = () => {
  const { showError } = useToast()

  const handleFetchError = useCallback(
    (error: Error | ApiError | ProblemDetails): Result => {
      if (!navigator.onLine) {
        return { isBackendUnavailable: true }
      }

      if (!('responseStatusCode' in error)) {
        return { isBackendUnavailable: true }
      }

      if (error.responseStatusCode === 503 || error.responseStatusCode === 502 || error.responseStatusCode === 504) {
        showError(`Backend code: ${error.responseStatusCode}`, undefined, 5000)
        return { isBackendUnavailable: true }
      }

      if (error.responseStatusCode === 401) {
        const location = error.responseHeaders!['Location'][0]
        const redirectUrl = AuthHandler.getRedirectUrl(location)
        showError('You are not authenticated', redirectUrl, 0, 'Login')
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
    handleFetchError,
  }
}
