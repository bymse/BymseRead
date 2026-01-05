import { expect, Page } from '@playwright/test'

export async function ensureBookLoaded(page: Page, bookId: string, expectedTitle?: string): Promise<void> {
  await waitForPdfLoaded(page)
  if (expectedTitle) {
    await expect(page.getByTestId('reader-header-title')).toHaveText(expectedTitle)
  }
  expect(page.url()).toContain(`/books/${bookId}`)
}

export async function getPageFromHeader(page: Page): Promise<number> {
  const pageInput = page.getByTestId('page-input')
  await pageInput.waitFor({ state: 'visible' })
  const value = await pageInput.inputValue()
  return parseInt(value, 10)
}

export async function setPageInHeader(
  page: Page,
  pageNumber: number,
  waitCurrentPageResponse: boolean = true,
): Promise<void> {
  const pageInput = page.getByTestId('page-input')
  await pageInput.waitFor({ state: 'visible' })
  await pageInput.clear()
  await pageInput.fill(pageNumber.toString())
  await pageInput.press('Enter')
  await expectPageVisible(page, pageNumber)
  if (waitCurrentPageResponse) await page.waitForResponse('**/current-page')
}

export async function scrollToPageNumber(page: Page, pageNumber: number): Promise<void> {
  await Promise.all([page.waitForResponse('**/current-page'), await page.mouse.wheel(0, pageNumber * 1000)])
}

export async function ensurePageNumber(page: Page, pageNumber: number): Promise<void> {
  expect(await getPageFromHeader(page)).toBe(pageNumber)
  await expectPageVisible(page, pageNumber)
}

export async function expectPageVisible(page: Page, pageNumber: number): Promise<void> {
  const selector = page.locator(`[data-page-number="${pageNumber}"]`)
  await expect(selector).toBeInViewport()
}

async function waitForPdfLoaded(page: Page): Promise<void> {
  await page.getByTestId('pdf-viewer-container').waitFor()
  await page.waitForSelector('[data-page-number]')
  await page.getByTestId('page-input').waitFor({ state: 'visible' })
}
