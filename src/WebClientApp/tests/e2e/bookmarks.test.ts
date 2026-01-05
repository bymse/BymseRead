import { test, expect } from '@playwright/test'
import {
  clickLastPageBookmark,
  getLastPageBookmark,
  markAsLastPage,
  openBookmarksPanel,
} from '../actions/bookmarksActions.js'
import { addBookThenOpenReader, goToBookFromList, goToBooks } from '../actions/booksActions.js'
import { ensureBookLoaded, expectPageVisible, getPageFromHeader, setPageInHeader } from '../actions/readerActions.js'
import { SITE_URL } from '../utils/constants.js'
import { registerUser } from '../actions/authActions.js'

test.describe('Bookmarks', () => {
  test('Bookmark_Should_PersistAndDisplay', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')

    await setPageInHeader(page, 5)

    await openBookmarksPanel(page)

    await markAsLastPage(page)

    const bookmark = await getLastPageBookmark(page)
    expect(bookmark.exists).toBe(true)
    expect(bookmark.pageNumber).toBe(5)

    await page.getByTestId('bookmarks-close-button').click()

    await goToBooks(page)
    await page.reload()

    await goToBookFromList(page, bookId, title)

    await openBookmarksPanel(page)

    const bookmarkAfterReturn = await getLastPageBookmark(page)
    expect(bookmarkAfterReturn.exists).toBe(true)
    expect(bookmarkAfterReturn.pageNumber).toBe(5)
  })

  test('Bookmark_Should_NavigateToPage_When_Clicked', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    await addBookThenOpenReader(page, '10-page.pdf')

    await setPageInHeader(page, 5)

    await openBookmarksPanel(page)

    await markAsLastPage(page)

    await setPageInHeader(page, 1)

    const currentPage = await getPageFromHeader(page)
    expect(currentPage).toBe(1)

    await openBookmarksPanel(page)

    const bookmark = await getLastPageBookmark(page)
    expect(bookmark.exists).toBe(true)
    expect(bookmark.pageNumber).toBe(5)

    await clickLastPageBookmark(page)

    await expectPageVisible(page, 5)

    const currentPageAfterClick = await getPageFromHeader(page)
    expect(currentPageAfterClick).toBe(5)
  })
})
