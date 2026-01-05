import { Page } from '@playwright/test'

export async function clickToastLink(page: Page): Promise<void> {
  await page.getByTestId('toast-link').click()
  await page.getByTestId('book-page-header').waitFor()
}
