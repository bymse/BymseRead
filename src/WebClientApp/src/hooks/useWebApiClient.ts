import type { FetchResponse } from '@microsoft/kiota-http-fetchlibrary/dist/es/src/utils/fetchDefinitions'
import { AuthHandler } from '@utils/authHandler.ts'
import { useRef } from 'preact/hooks'
import { AnonymousAuthenticationProvider } from '@microsoft/kiota-abstractions'
import {
  FetchRequestAdapter,
  KiotaClientFactory,
  MiddlewareFactory,
  RetryHandler,
  RetryHandlerOptions,
} from '@microsoft/kiota-http-fetchlibrary'
import { BymseReadClient, createBymseReadClient } from '@api/bymseReadClient'

export const useWebApiClient = () => {
  const clientRef = useRef<BymseReadClient>(null)

  if (!clientRef.current) {
    const handlers = MiddlewareFactory.getDefaultMiddlewares()
    if (handlers[0] instanceof RetryHandler) {
      const options = new RetryHandlerOptions({
        delay: 0.5,
        maxRetries: 2,
        shouldRetry: (_: number, __: number, ___: string, options: RequestInit | undefined, response: FetchResponse) =>
          options?.method === 'GET' && (response.status === 429 || response.status > 500),
      })
      handlers[0] = new RetryHandler(options)
    }

    handlers.unshift(new AuthHandler())

    const httpClient = KiotaClientFactory.create(undefined, handlers)
    const authProvider = new AnonymousAuthenticationProvider()
    const adapter = new FetchRequestAdapter(authProvider, undefined, undefined, httpClient)

    clientRef.current = createBymseReadClient(adapter)
  }

  return {
    client: clientRef.current,
  }
}
