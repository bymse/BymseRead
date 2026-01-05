import { test, expect, Page, defineConfig } from '@playwright/test'
import { registerUser } from '../actions/authActions.js'
import {
  clickLastPageBookmark,
  getLastPageBookmark,
  markAsLastPage,
  openBookmarksPanel,
} from '../actions/bookmarksActions.js'
import {
  addBook,
  ensureBookInSection,
  makeBookActive,
  goToBooks,
  addBookThenOpenReader,
  goToBookFromList,
  ensureCoverLoaded,
} from '../actions/booksActions.js'
import { ensureBookLoaded, setPageInHeader, expectPageVisible, ensurePageNumber } from '../actions/readerActions.js'
import { S3_URL, SITE_URL } from '../utils/constants.js'

test.describe('Offline functionality', () => {
  test('ActiveBooks_Should_BeAvailableOffline_When_ProgressMade', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const book1 = await addBook(page, '10-page.pdf')
    const book2 = await addBook(page, '10-page.pdf')
    const book3 = await addBook(page, '10-page.pdf')
    const book4 = await addBook(page, '10-page.pdf')

    await makeBookActive(page, book1.bookId)
    await makeBookActive(page, book3.bookId)

    await goToBooks(page)

    await setOffline(page, true)
    await page.reload()

    await ensureBookInSection(page, 'offline', book1.bookId, book1.title)
    await ensureBookInSection(page, 'offline', book3.bookId, book3.title)

    const book2Card = page.getByTestId(`book-card-${book2.bookId}`)
    const book4Card = page.getByTestId(`book-card-${book4.bookId}`)
    await expect(book2Card).not.toBeVisible()
    await expect(book4Card).not.toBeVisible()
  })

  test('OfflineBook_Should_HaveCover', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)
    await goToBooks(page)

    const { bookId, title } = await addBook(page, '10-page.pdf')
    await makeBookActive(page, bookId)
    await goToBooks(page)
    await ensureCoverLoaded(page, bookId)

    await setOffline(page, true)
    await page.reload()

    await ensureBookInSection(page, 'offline', bookId, title)
    await ensureCoverLoaded(page, bookId)
  })

  test('ActiveBookPdf_Should_BeAvailableOffline_When_ProgressMade', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')

    await setPageInHeader(page, 5)

    await goToBooks(page)

    await setOffline(page, true)

    await page.reload()

    await goToBookFromList(page, bookId, title)
    await ensurePageNumber(page, 5)

    await setPageInHeader(page, 3, false)
  })

  test('PageAndBookmarksChange_Should_BeSynced_WhenOnline', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBook(page, '10-page.pdf')

    await makeBookActive(page, bookId)

    await goToBooks(page)

    await setOffline(page, true)

    await page.reload()

    await goToBookFromList(page, bookId, title)

    await setPageInHeader(page, 5, false)
    await openBookmarksPanel(page)
    await markAsLastPage(page)
    await setPageInHeader(page, 8, false)
    await page.waitForTimeout(1000)

    await goToBooks(page)
    await setOffline(page, false)
    await page.reload()
    await page.waitForTimeout(500)

    await goToBookFromList(page, bookId, title)
    await ensurePageNumber(page, 8)

    await openBookmarksPanel(page)
    const bookmark = await getLastPageBookmark(page)
    expect(bookmark.pageNumber).toBe(5)
  })
})

const setOffline = async (page: Page, offline: boolean) => {
  //todo set real offline with SW caching assets
  if (offline) {
    const context = page.context()
    await context.route('web-api/**', (route, request) => {
      route.abort('connectionfailed')
    })

    await context.route('bymse-read/**', (route, request) => {
      route.abort('connectionfailed')
    })
  } else {
    const context = page.context()
    await context.unrouteAll()
  }
}
