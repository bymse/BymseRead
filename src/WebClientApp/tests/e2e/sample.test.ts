import { test, expect } from '@playwright/test'

test.describe('Sample E2E Tests', () => {
  test('should navigate to application and load page', async ({ page }) => {
    await page.goto('/')
    await expect(page).toHaveURL(url => url.pathname === '/realms/bymse-read/login-actions/authenticate')
    await page.waitForLoadState('domcontentloaded')
  })
})
