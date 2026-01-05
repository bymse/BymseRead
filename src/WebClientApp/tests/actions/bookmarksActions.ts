import { Page } from '@playwright/test'

export async function openBookmarksPanel(page: Page): Promise<void> {
  const bookmarksPanel = page.getByTestId('bookmarks-panel')
  if (!(await bookmarksPanel.isVisible())) {
    await page.getByTestId('reader-header-bookmark-button').click()
  }

  await page.getByTestId('bookmarks-panel').waitFor()
}

export async function markAsLastPage(page: Page): Promise<void> {
  await page.getByTestId('bookmarks-mark-as-last-page-button').click()
}

export async function clickLastPageBookmark(page: Page): Promise<void> {
  await page.getByTestId('bookmarks-last-page-card').click()
}

export async function getLastPageBookmark(page: Page): Promise<{ exists: boolean; pageNumber: number | null }> {
  const lastPageCard = page.getByTestId('bookmarks-last-page-card')
  const exists = await lastPageCard.isVisible()

  if (!exists) {
    return { exists: false, pageNumber: null }
  }

  const pageNumberElement = page.getByTestId('bookmarks-last-page-number')
  const pageText = await pageNumberElement.textContent()
  const pageNumber = pageText ? parseInt(pageText, 10) : null

  return { exists: true, pageNumber }
}
