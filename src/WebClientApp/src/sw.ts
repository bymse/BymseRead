import { handlePostponedUpdates } from '@storage/postponedUpdatesHandler.ts'
import { precacheAndRoute, cleanupOutdatedCaches } from 'workbox-precaching'
import { initFilesCache } from './storage/filesCache'

declare let self: ServiceWorkerGlobalScope

cleanupOutdatedCaches()

if (import.meta.env.PROD) {
  precacheAndRoute(self.__WB_MANIFEST)
}

initFilesCache()

handlePostponedUpdates()

void self.skipWaiting()
