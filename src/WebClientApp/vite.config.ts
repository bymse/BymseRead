import { defineConfig, normalizePath, ProxyOptions } from 'vite'
import { viteStaticCopy } from 'vite-plugin-static-copy'
import preact from '@preact/preset-vite'
import path from 'path'
import { createRequire } from 'node:module'
import { pwaConfig } from './pwa.config.ts'

const require = createRequire(import.meta.url)
const cMapsDir = normalizePath(path.join(path.dirname(require.resolve('pdfjs-dist/package.json')), 'cmaps'))
const standardFontsDir = normalizePath(
  path.join(path.dirname(require.resolve('pdfjs-dist/package.json')), 'standard_fonts'),
)

function proxyWithFallback(target: string): ProxyOptions {
  return {
    target,
    changeOrigin: true,
    configure: proxy => {
      proxy.on('error', (err, req, res) => {
        if (!res || res.headersSent) return

        res.writeHead(503, { 'Content-Type': 'application/json' })
        res.end(
          JSON.stringify({
            error: 'Proxy target unavailable',
            target,
            details: err.message,
          }),
        )
      })
    },
  }
}

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    pwaConfig(),
    preact(),
    viteStaticCopy({
      targets: [
        { src: cMapsDir, dest: '' },
        { src: standardFontsDir, dest: '' },
      ],
    }),
  ],
  resolve: {
    alias: {
      '@components': path.resolve(__dirname, './src/components'),
      '@assets': path.resolve(__dirname, './src/assets'),
      '@icons': path.resolve(__dirname, './src/icons'),
      '@hooks': path.resolve(__dirname, './src/hooks'),
      '@api': path.resolve(__dirname, './generated/api'),
      '@utils': path.resolve(__dirname, './src/utils'),
      '@storage': path.resolve(__dirname, './src/storage'),
    },
  },
  server: {
    proxy: {
      '/web-api': proxyWithFallback('http://localhost:5299'),
      '/bymse-read/': proxyWithFallback('http://minio:9000'),
    },
  },
})
