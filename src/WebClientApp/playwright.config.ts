import { defineConfig, devices } from '@playwright/test'
import { SITE_URL } from './tests/utils/constants'

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: 'html',
  use: {
    baseURL: SITE_URL,
    trace: 'retain-on-failure',
    launchOptions: {
      args: [`--unsafely-treat-insecure-origin-as-secure=${SITE_URL}`],
    },
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
})
