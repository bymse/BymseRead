import { useRef } from 'preact/hooks'
import {
  BaseBearerTokenAuthenticationProvider,
  AccessTokenProvider,
  AllowedHostsValidator,
} from '@microsoft/kiota-abstractions'
import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary'
import { BymseReadClient, createBymseReadClient } from '@api/bymseReadClient'

const staticAccessTokenProvider: AccessTokenProvider = {
  getAuthorizationToken: async () => Promise.resolve(__LOCAL_ACCESS_TOKEN__),
  getAllowedHostsValidator: () => new AllowedHostsValidator(),
}

export const useWebApiClient = () => {
  const clientRef = useRef<BymseReadClient>(null)

  if (!clientRef.current) {
    const authProvider = new BaseBearerTokenAuthenticationProvider(staticAccessTokenProvider)
    const adapter = new FetchRequestAdapter(authProvider)

    clientRef.current = createBymseReadClient(adapter)
  }

  const handleError = () => {}

  return {
    client: clientRef.current,
    onError: handleError,
  }
}
