import { useRef } from 'preact/hooks'
import { AnonymousAuthenticationProvider } from '@microsoft/kiota-abstractions'
import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary'
import { BymseReadClient, createBymseReadClient } from '@api/bymseReadClient'
import { WebApiRequestBuilder } from '@api/webApi'

export const useWebApiClient = (): WebApiRequestBuilder => {
  const clientRef = useRef<BymseReadClient>(null)

  if (!clientRef.current) {
    const authProvider = new AnonymousAuthenticationProvider()
    const adapter = new FetchRequestAdapter(authProvider)

    clientRef.current = createBymseReadClient(adapter)
  }

  return clientRef.current.webApi
}
