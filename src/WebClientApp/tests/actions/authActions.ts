import { Page } from '@playwright/test'
import { SITE_URL } from '../utils/constants.js'

export interface LoginCredentials {
  email: string
  password: string
}

export async function loginAction(page: Page, credentials: LoginCredentials): Promise<void> {
  await page.waitForURL(url => url.pathname.startsWith('/realms/bymse-read'))

  await page.waitForURL(url => url.href.includes('login-actions/authenticate'))

  await page.locator('#username').fill(credentials.email)
  await page.locator('#password').fill(credentials.password)

  await page.locator('#kc-login').click()

  const expectedOrigin = new URL(SITE_URL).origin
  await page.waitForURL(url => url.origin === expectedOrigin, { waitUntil: 'domcontentloaded' })
}

export async function registerUser(page: Page): Promise<LoginCredentials> {
  await page.waitForURL(url => url.pathname.startsWith('/realms/bymse-read'))

  const currentUrl = page.url()
  if (!currentUrl.includes('login-actions/registration')) {
    const registrationLink = page.locator('#kc-registration a')
    await registrationLink.click()
    await page.waitForURL(url => url.href.includes('login-actions/registration'))
  }

  const randomId = Math.random().toString(36).substring(2, 15)
  const credentials: LoginCredentials = {
    email: `test-${randomId}@example.com`,
    password: `TestPass${randomId}!`,
  }

  await page.locator('#email').fill(credentials.email)
  await page.locator('#password').fill(credentials.password)
  await page.locator('#password-confirm').fill(credentials.password)

  await page.locator('#kc-form-buttons input[type="submit"]').click()

  const expectedOrigin = new URL(SITE_URL).origin
  await page.waitForURL(url => url.origin === expectedOrigin, { waitUntil: 'domcontentloaded' })

  return credentials
}
