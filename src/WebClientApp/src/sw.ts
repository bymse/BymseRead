import { precacheAndRoute, cleanupOutdatedCaches, createHandlerBoundToURL } from 'workbox-precaching'
import { registerRoute, NavigationRoute } from 'workbox-routing'
import { Queue } from 'workbox-background-sync'
import { initFilesCache } from './storage/filesCache'

declare let self: ServiceWorkerGlobalScope

cleanupOutdatedCaches()

if (import.meta.env.PROD) {
  precacheAndRoute(self.__WB_MANIFEST)
}

initFilesCache()

const bookQueues = new Map<string, Queue>()

const getQueueForBook = (bookId: string): Queue => {
  let queue = bookQueues.get(bookId)
  if (!queue) {
    queue = new Queue(`book-${bookId}-queue`, {
      maxRetentionTime: 24 * 60,
      onSync: async ({ queue: q }) => {
        let entry = await q.shiftRequest()
        while (entry) {
          try {
            await fetch(entry.request)
          } catch (error) {
            await q.unshiftRequest(entry)
            throw error
          }
          entry = await q.shiftRequest()
        }
      },
    })
    bookQueues.set(bookId, queue)
  }
  return queue
}

if (import.meta.env.PROD) {
  registerRoute(
    new NavigationRoute(createHandlerBoundToURL('index.html'), {
      allowlist: [/.*/],
      denylist: [/^\/web-api\//, /^\/bymse-read\//],
    }),
  )
}

self.addEventListener('fetch', event => {
  const url = new URL(event.request.url)

  const bookmarkMatch = url.pathname.match(/^\/web-api\/books\/([^/]+)\/bookmarks\/last-page$/)
  const progressMatch = url.pathname.match(/^\/web-api\/books\/([^/]+)\/progress\/current-page$/)

  if (bookmarkMatch && event.request.method === 'POST') {
    const bookId = bookmarkMatch[1]
    event.respondWith(
      fetch(event.request.clone()).catch(async () => {
        const queue = getQueueForBook(bookId)
        await queue.pushRequest({ request: event.request.clone() })
        return new Response(JSON.stringify({ queued: true, bookId }), {
          status: 202,
          headers: { 'Content-Type': 'application/json' },
        })
      }),
    )
  } else if (progressMatch && event.request.method === 'PUT') {
    const bookId = progressMatch[1]
    event.respondWith(
      fetch(event.request.clone()).catch(async () => {
        const queue = getQueueForBook(bookId)
        await queue.pushRequest({ request: event.request.clone() })
        return new Response(JSON.stringify({ queued: true, bookId }), {
          status: 202,
          headers: { 'Content-Type': 'application/json' },
        })
      }),
    )
  }
})

void self.skipWaiting()
