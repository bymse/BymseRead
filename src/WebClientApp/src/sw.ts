import { BookFiles, ServiceWorkerMessage } from '@storage/filesCache.ts'
import { precacheAndRoute, cleanupOutdatedCaches, createHandlerBoundToURL } from 'workbox-precaching'
import { registerRoute, NavigationRoute } from 'workbox-routing'
import { createPartialResponse } from 'workbox-range-requests'
import { setBookFilesMeta } from './storage/metaStore'

declare let self: ServiceWorkerGlobalScope

cleanupOutdatedCaches()

precacheAndRoute(self.__WB_MANIFEST)

const CACHE_NAME = 'bymse-read-pdfs-v1'

registerRoute(
  request => request.url.pathname.includes('bymse-read/file'),
  async ({ request }) => {
    const cache = await caches.open(CACHE_NAME)
    const cachedResponse = await cache.match(request, { ignoreSearch: false })

    if (cachedResponse) {
      return request.headers.has('range') ? createPartialResponse(request, cachedResponse) : cachedResponse
    }

    return fetch(request)
  },
)

registerRoute(
  new NavigationRoute(createHandlerBoundToURL('index.html'), {
    allowlist: [/.*/],
    denylist: [/^\/web-api\//, /^\/bymse-read\//],
  }),
)

self.addEventListener('message', event => {
  const data: ServiceWorkerMessage = event.data

  if (data.type === 'CACHE_ADD_FILES') {
    void handleAddFiles(data.payload.files)
  } else if (data.type === 'CACHE_REMOVE_FILES') {
    void handleRemoveFiles(data.payload.files)
  }
})

const handleAddFiles = async (files: BookFiles[]): Promise<void> => {
  if (!files || files.length === 0) {
    return
  }

  const cache = await caches.open(CACHE_NAME)

  await Promise.allSettled(
    files.map(async ({ bookId, fileUrl, coverUrl }) => {
      if (fileUrl) {
        const response = await fetch(fileUrl)
        response.ok && (await cache.put(fileUrl, response))
      }

      if (coverUrl) {
        const response = await fetch(coverUrl)
        response.ok && (await cache.put(coverUrl, response))
      }

      await setBookFilesMeta(bookId, fileUrl, coverUrl)
    }),
  )
}

const handleRemoveFiles = async (files: BookFiles[]): Promise<void> => {
  if (!files || files.length === 0) {
    return
  }

  const cache = await caches.open(CACHE_NAME)

  await Promise.allSettled(
    files.map(async ({ bookId, fileUrl }) => {
      if (!bookId || !fileUrl) {
        return
      }

      await cache.delete(fileUrl)
    }),
  )
}

void self.skipWaiting()
