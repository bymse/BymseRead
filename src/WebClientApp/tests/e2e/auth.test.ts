import { test, expect } from '@playwright/test'
import { loginAction, registerAction } from '../actions/authActions'
import { SITE_URL } from '../utils/constants'

test.describe('Authentication', () => {
  test('Register_Should_Succeed', async ({ page }) => {
    await page.goto(SITE_URL)

    const credentials = await registerAction(page)

    expect(credentials.email).toBeTruthy()
    expect(credentials.password).toBeTruthy()
    expect(page.url()).toContain(new URL(SITE_URL).origin)
  })

  test('Register_Should_MaintainAuth_When_OpeningNewPage', async ({ page }) => {
    await page.goto(SITE_URL)

    await registerAction(page)

    await page.goto(SITE_URL)

    const currentUrl = page.url()
    expect(currentUrl).not.toContain('/realms/bymse-read')
    expect(currentUrl).toContain(new URL(SITE_URL).origin)
  })

  test('Login_Should_Succeed_When_UsingNewBrowsingContext', async ({ browser }) => {
    const firstContext = await browser.newContext()
    const firstPage = await firstContext.newPage()

    await firstPage.goto(SITE_URL)
    const credentials = await registerAction(firstPage)
    await firstContext.close()

    const secondContext = await browser.newContext()
    const secondPage = await secondContext.newPage()

    await secondPage.goto(SITE_URL)
    await loginAction(secondPage, credentials)

    const currentUrl = secondPage.url()
    expect(currentUrl).not.toContain('/realms/bymse-read')
    expect(currentUrl).toContain(new URL(SITE_URL).origin)

    await secondContext.close()
  })
})
