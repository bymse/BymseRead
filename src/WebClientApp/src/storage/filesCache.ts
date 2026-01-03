import { BookFiles } from './serviceWorkerMessages.ts'
import { resetBookFilesMeta, setBookFilesMeta } from './metaStore'
import { registerRoute } from 'workbox-routing'
import { createPartialResponse } from 'workbox-range-requests'

const CACHE_NAME = 'bymse-read-files-v1'

export const initFilesCache = (): void => {
  registerRoute(
    request => request.url.pathname.includes('bymse-read/file'),
    async ({ request }) => {
      const cache = await caches.open(CACHE_NAME)
      const cachedResponse = await cache.match(request, { ignoreSearch: true })

      if (cachedResponse) {
        return request.headers.has('range') ? createPartialResponse(request, cachedResponse) : cachedResponse
      }

      return fetch(request)
    },
  )
}

export const handleAddFiles = async (files: BookFiles[]): Promise<void> => {
  if (!files || files.length === 0) {
    return
  }

  const cache = await caches.open(CACHE_NAME)

  await Promise.all(
    files.map(async ({ bookId, fileUrl, coverUrl }) => {
      const fileUrlCached = await cacheIfNeed(cache, fileUrl)
      const coverUrlCached = await cacheIfNeed(cache, coverUrl)

      await setBookFilesMeta(bookId, fileUrlCached ? fileUrl : undefined, coverUrlCached ? coverUrl : undefined)
    }),
  )
}

export const handleRemoveFiles = async (files: BookFiles[]): Promise<void> => {
  if (!files || files.length === 0) {
    return
  }

  const cache = await caches.open(CACHE_NAME)

  await Promise.all(
    files.map(async ({ bookId, fileUrl, coverUrl }) => {
      if (!bookId || (!fileUrl && !coverUrl)) {
        return
      }

      if (fileUrl) {
        await cache.delete(fileUrl)
      }

      if (coverUrl) {
        await cache.delete(coverUrl)
      }

      await resetBookFilesMeta(bookId, fileUrl, coverUrl)
    }),
  )
}

async function cacheIfNeed(cache: Cache, url?: string): Promise<boolean> {
  if (!url) {
    return Promise.resolve(false)
  }

  const match = await cache.match(url, { ignoreSearch: true })
  if (match?.ok) {
    return Promise.resolve(true)
  }

  const response = await fetch(url)
  if (response.ok) {
    await cache.put(url, response)
    return Promise.resolve(true)
  }
  return Promise.resolve(false)
}
