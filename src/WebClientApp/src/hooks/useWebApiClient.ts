import { useRef } from 'preact/hooks'
import { AnonymousAuthenticationProvider } from '@microsoft/kiota-abstractions'
import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary'
import { BymseReadClient, createBymseReadClient } from '@api/bymseReadClient'

export const useWebApiClient = () => {
  const clientRef = useRef<BymseReadClient>(null)

  if (!clientRef.current) {
    const authProvider = new AnonymousAuthenticationProvider()
    const adapter = new FetchRequestAdapter(authProvider, undefined, undefined, undefined)

    clientRef.current = createBymseReadClient(adapter)
  }

  return {
    client: clientRef.current,
  }
}
