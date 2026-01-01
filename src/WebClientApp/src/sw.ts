import { BookFiles, ServiceWorkerMessage } from '@storage/filesCache.ts'
import { precacheAndRoute, cleanupOutdatedCaches, createHandlerBoundToURL } from 'workbox-precaching'
import { registerRoute, NavigationRoute } from 'workbox-routing'
import { createPartialResponse } from 'workbox-range-requests'
import { resetBookFilesMeta, setBookFilesMeta } from './storage/metaStore'

declare let self: ServiceWorkerGlobalScope

cleanupOutdatedCaches()

if (import.meta.env.PROD) {
  precacheAndRoute(self.__WB_MANIFEST)
}

const CACHE_NAME = 'bymse-read-files-v1'

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

if (import.meta.env.PROD) {
  registerRoute(
    new NavigationRoute(createHandlerBoundToURL('index.html'), {
      allowlist: [/.*/],
      denylist: [/^\/web-api\//, /^\/bymse-read\//],
    }),
  )
}

self.addEventListener('message', event => {
  const data = event.data as ServiceWorkerMessage

  let promise: Promise<void> | null = null
  if (data.type === 'CACHE_ADD_FILES') {
    promise = handleAddFiles(data.payload.files)
  } else if (data.type === 'CACHE_REMOVE_FILES') {
    promise = handleRemoveFiles(data.payload.files)
  }

  if (promise !== null) {
    promise = promise.catch(e => {
      // eslint-disable-next-line no-console
      console.error('Failed to handle service worker message', e, data)
    })
    event.waitUntil(promise)
  }
})

const handleAddFiles = async (files: BookFiles[]): Promise<void> => {
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

const handleRemoveFiles = async (files: BookFiles[]): Promise<void> => {
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

void self.skipWaiting()
