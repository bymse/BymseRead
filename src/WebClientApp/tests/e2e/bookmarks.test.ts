import { test, expect } from '@playwright/test'
import { SITE_URL } from '../utils/constants.js'
import { registerAction } from '../actions/authActions.js'
import {
  generateBookTitle,
  scrollToPage,
  openBookmarksPanel,
  markAsLastPage,
  getLastPageBookmark,
  goToBooks,
  clickBookCard,
  expectPageVisible,
  getCurrentPage,
  clickLastPageBookmark,
  createAndOpenBook,
  waitForPdfLoaded,
} from '../actions/bookActions.js'

test.describe('Bookmarks', () => {
  test('Bookmark_Should_PersistAndDisplay', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    const bookId = await createAndOpenBook(page, title, '10-page.pdf')

    await scrollToPage(page, 5)

    await openBookmarksPanel(page)

    await markAsLastPage(page)

    const bookmark = await getLastPageBookmark(page)
    expect(bookmark.exists).toBe(true)
    expect(bookmark.pageNumber).toBe(5)

    await page.getByTestId('bookmarks-close-button').click()

    await goToBooks(page)
    await page.reload()

    await clickBookCard(page, bookId)

    await waitForPdfLoaded(page)

    await openBookmarksPanel(page)

    const bookmarkAfterReturn = await getLastPageBookmark(page)
    expect(bookmarkAfterReturn.exists).toBe(true)
    expect(bookmarkAfterReturn.pageNumber).toBe(5)

    await page.getByTestId('bookmarks-close-button').click()
  })

  test('Bookmark_Should_NavigateToPage_When_Clicked', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    await createAndOpenBook(page, title, '10-page.pdf')

    await scrollToPage(page, 5)

    await openBookmarksPanel(page)

    await markAsLastPage(page)

    await scrollToPage(page, 1)

    const currentPage = await getCurrentPage(page)
    expect(currentPage).toBe(1)

    await openBookmarksPanel(page)

    const bookmark = await getLastPageBookmark(page)
    expect(bookmark.exists).toBe(true)
    expect(bookmark.pageNumber).toBe(5)

    await clickLastPageBookmark(page)

    await expectPageVisible(page, 5)

    const currentPageAfterClick = await getCurrentPage(page)
    expect(currentPageAfterClick).toBe(5)
  })
})
