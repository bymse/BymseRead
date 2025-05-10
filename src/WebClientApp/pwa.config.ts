import { VitePWA } from 'vite-plugin-pwa'

export const pwaConfig = () =>
  VitePWA({
    injectRegister: 'inline',
    registerType: 'autoUpdate',
    devOptions: {
      enabled: true,
      navigateFallbackAllowlist: [/.*/],
    },
    workbox: {
      globPatterns: ['assets/*.{woff2,mjs,svg}', '**\/*.{js,wasm,css,html}'],
      maximumFileSizeToCacheInBytes: 5 * 1024 * 1024, // 5MB
      navigateFallbackDenylist: [/^\/web-api\//, /^\/bymse-read\//],
    },
    includeAssets: ['/icons/browser-icon.svg'],
    manifest: {
      name: 'BymseRead',
      short_name: 'BymseRead',
      description: 'Web application that lets users manage and read their PDF books seamlessly in one place.',
      start_url: '/?from=pwa',
      display: 'standalone',
      background_color: '#F2F3F8',
      theme_color: '#F2F3F8',
      icons: [
        {
          src: '/icons/pwa-512x512.svg',
          sizes: 'any',
          type: 'image/svg+xml',
        },
        {
          src: '/icons/pwa-96x96.png',
          sizes: '96x96',
          type: 'image/png',
        },
        {
          src: '/icons/pwa-128x128.png',
          sizes: '128x128',
          type: 'image/png',
        },
        {
          src: '/icons/pwa-144x144.png',
          sizes: '144x144',
          type: 'image/png',
        },
        {
          src: '/icons/pwa-192x192.png',
          sizes: '192x192',
          type: 'image/png',
        },
        {
          src: '/icons/pwa-256x256.png',
          sizes: '256x256',
          type: 'image/png',
        },
        {
          src: '/icons/pwa-384x384.png',
          sizes: '384x384',
          type: 'image/png',
        },
        {
          src: '/icons/pwa-512x512.png',
          sizes: '512x512',
          type: 'image/png',
        },
      ],
    },
  })
