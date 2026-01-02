import { RequestOption } from '@microsoft/kiota-abstractions'
import type { Middleware } from '@microsoft/kiota-http-fetchlibrary'

export class AuthHandler implements Middleware {
  static getRedirectUrl = (location: string) => {
    const url = new URL(location)
    url.searchParams.set('returnUrl', window.location.href)

    return url.toString()
  }

  next: Middleware | undefined
  public async execute(
    url: string,
    requestInit: RequestInit,
    requestOptions?: Record<string, RequestOption>,
  ): Promise<Response> {
    const response = await this.next?.execute(url, requestInit, requestOptions)
    if (response?.status === 401 && requestInit.method === 'GET') {
      const location = response.headers.get('Location')
      if (location) {
        window.location.href = AuthHandler.getRedirectUrl(location)
      }
    }

    return response!
  }
}
