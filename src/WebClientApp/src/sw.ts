import {precacheAndRoute, cleanupOutdatedCaches, createHandlerBoundToURL,} from 'workbox-precaching'
import { registerRoute, NavigationRoute } from 'workbox-routing'

cleanupOutdatedCaches()

// @ts-expect-error
precacheAndRoute(self.__WB_MANIFEST)

registerRoute(new NavigationRoute(createHandlerBoundToURL("index.html"), {
  allowlist: [/.*/],
  denylist: [/^\/web-api\//, /^\/bymse-read\//]
}));

self.skipWaiting()