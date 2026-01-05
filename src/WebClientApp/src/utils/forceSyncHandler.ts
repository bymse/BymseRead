import { RequestOption } from '@microsoft/kiota-abstractions'
import type { Middleware } from '@microsoft/kiota-http-fetchlibrary'
import { forceSync } from '@storage/serviceWorkerMessages.ts'

let needSync = true

export class ForceSyncHandler implements Middleware {
  next: Middleware | undefined
  public async execute(
    url: string,
    requestInit: RequestInit,
    requestOptions?: Record<string, RequestOption>,
  ): Promise<Response> {
    try {
      const response = await this.next?.execute(url, requestInit, requestOptions)
      if (response?.ok && needSync) {
        needSync = false
        void forceSync().catch()
      }
      if (response?.status && response.status > 500) {
        needSync = true
      }
      return response!
    } catch (e) {
      needSync = true
      throw e
    }
  }
}
