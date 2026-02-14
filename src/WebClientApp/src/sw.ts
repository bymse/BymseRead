import { forceSync, handlePostponedUpdates } from '@storage/postponedUpdatesHandler.ts'
import { ServiceWorkerMessage } from '@storage/serviceWorkerMessages.ts'
import { precacheAndRoute, cleanupOutdatedCaches, createHandlerBoundToURL } from 'workbox-precaching'
import { NavigationRoute, registerRoute } from 'workbox-routing'
import { handleAddFiles, handleRemoveFiles, initFilesCache } from './storage/filesCache'

declare let self: ServiceWorkerGlobalScope

cleanupOutdatedCaches()

if (import.meta.env.PROD) {
  precacheAndRoute(self.__WB_MANIFEST)
  registerRoute(
    new NavigationRoute(createHandlerBoundToURL('index.html'), {
      allowlist: [/.*/],
      denylist: [/^\/web-api\//, /^\/bymse-read\//, /^\/7e967059-bymse-read\//],
    }),
  )
}

initFilesCache()

handlePostponedUpdates()

self.addEventListener('message', event => {
  const data = event.data as ServiceWorkerMessage

  let promise: Promise<void> | null = null
  switch (data.type) {
    case 'CACHE_ADD_FILES':
      promise = handleAddFiles(data.payload.files)
      break
    case 'CACHE_REMOVE_FILES':
      promise = handleRemoveFiles(data.payload.files)
      break
    case 'FORCE_SYNC':
      promise = forceSync()
      break
  }

  if (promise !== null) {
    promise = promise.catch(e => {
      // eslint-disable-next-line no-console
      console.error('Failed to handle service worker message', e, data)
    })
    event.waitUntil(promise)
  }
})

void self.skipWaiting()
