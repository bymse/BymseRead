import { defineConfig } from 'vite'
import preact from '@preact/preset-vite'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [preact()],
  resolve: {
    alias: {
      '@components': path.resolve(__dirname, './src/components'),
      '@assets': path.resolve(__dirname, './src/assets'),
      '@icons': path.resolve(__dirname, './src/icons'),
      '@hooks': path.resolve(__dirname, './src/hooks'),
      '@api': path.resolve(__dirname, './generated/api'),
      '@utils': path.resolve(__dirname, './src/utils'),
    },
  },
  define: {
    __LOCAL_ACCESS_TOKEN__:
      process.env.NODE_ENV === 'development'
        ? JSON.stringify(
            'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlYzRjNTJmNS04ZmM0LTQxZjMtOGI2Yy1lZjRhYjM0NGQ0ZTciLCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.EElJybgH-irDqgOg59_iyq3hqqMAgTQwWAsRWPzQ9EE',
          )
        : '',
  },
  server: {
    proxy: {
      '/web-api': {
        target: 'http://localhost:5299',
        changeOrigin: true,
      },
    },
  },
})
