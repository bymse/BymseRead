import { test, expect } from '@playwright/test'
import { SITE_URL } from '../utils/constants.js'
import { registerAction } from '../actions/authActions.js'
import {
  addBook,
  generateBookTitle,
  waitForPdfLoaded,
  scrollToPage,
  goToBooks,
  clickBookCard,
  expectPageVisible,
  getCurrentPage,
  clickToastLink,
} from '../actions/bookActions.js'

test.describe('Progress Tracking', () => {
  test('CurrentPage_Should_PersistAndRestore', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    const bookId = await addBook(page, title, '10-page.pdf')

    await clickToastLink(page)

    await waitForPdfLoaded(page)

    const initialPage = await getCurrentPage(page)
    expect(initialPage).toBe(1)

    await scrollToPage(page, 3)

    const currentPage = await getCurrentPage(page)
    expect(currentPage).toBe(3)

    await goToBooks(page)

    await clickBookCard(page, bookId)
    await waitForPdfLoaded(page)

    await expectPageVisible(page, 3)

    const restoredPage = await getCurrentPage(page)
    expect(restoredPage).toBe(3)
  })

  test('PageInput_Should_UpdateCurrentPage_When_EnteringPageNumber', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    await addBook(page, title, '10-page.pdf')

    await clickToastLink(page)

    await waitForPdfLoaded(page)

    const initialPage = await getCurrentPage(page)
    expect(initialPage).toBe(1)

    await scrollToPage(page, 7)

    const currentPage = await getCurrentPage(page)
    expect(currentPage).toBe(7)

    await scrollToPage(page, 4)
    await expectPageVisible(page, 4)
  })
})
