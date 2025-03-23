import { useRef } from 'preact/hooks'
import {
  BaseBearerTokenAuthenticationProvider,
  AccessTokenProvider,
  AllowedHostsValidator,
  type RequestOption,
} from '@microsoft/kiota-abstractions'
import { FetchRequestAdapter, Middleware, MiddlewareFactory } from '@microsoft/kiota-http-fetchlibrary'
import { KiotaClientFactory } from '@microsoft/kiota-http-fetchlibrary'
import { BymseReadClient, createBymseReadClient } from '@api/bymseReadClient'
import { ProblemDetails } from '@api/models'

const staticAccessTokenProvider: AccessTokenProvider = {
  getAuthorizationToken: async () => Promise.resolve(__LOCAL_ACCESS_TOKEN__),
  getAllowedHostsValidator: () => new AllowedHostsValidator(),
}

// Create a middleware that handles 401 responses
class AuthRedirectMiddleware implements Middleware {
  public next: Middleware | undefined

  public async execute(
    url: string,
    requestInit: RequestInit,
    requestOptions?: Record<string, RequestOption>,
  ): Promise<Response> {
    if (!this.next) {
      throw new Error('Next middleware is not provided')
    }

    const response = await this.next.execute(url, requestInit, requestOptions)

    if (response.status === 401) {
      const clonedResponse = response.clone()
      const details = (await clonedResponse.json()) as ProblemDetails
      
      if (details && details.redirectUrl) {
        // Redirect to the login URL from the response
        window.location.href = details.redirectUrl
      }
    }

    // Return the original response
    return response
  }
}

export const useWebApiClient = () => {
  const clientRef = useRef<BymseReadClient>(null)

  if (!clientRef.current) {
    const authProvider = new BaseBearerTokenAuthenticationProvider(staticAccessTokenProvider)
    const handlers = MiddlewareFactory.getDefaultMiddlewares()
    handlers.unshift(new AuthRedirectMiddleware())
    const httpClient = KiotaClientFactory.create(undefined, handlers)

    const adapter = new FetchRequestAdapter(authProvider, undefined, undefined, httpClient)

    clientRef.current = createBymseReadClient(adapter)
  }

  return {
    client: clientRef.current,
  }
}
