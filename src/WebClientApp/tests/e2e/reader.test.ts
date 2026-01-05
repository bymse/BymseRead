import { test } from '@playwright/test'
import { addBookThenOpenReader, goToBookFromList, goToBooks } from '../actions/booksActions.js'
import { ensureBookLoaded, ensurePageNumber, scrollToPageNumber, setPageInHeader } from '../actions/readerActions.js'
import { SITE_URL } from '../utils/constants.js'
import { registerUser } from '../actions/authActions.js'
import { openBookmarksPanel } from '../actions/bookmarksActions.js'

test.describe('Reader tests', () => {
  test('ReaderOnScroll_Should_PersistCurrentPage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePageNumber(page, 1)

    await scrollToPageNumber(page, 3)
    await ensurePageNumber(page, 3)

    await goToBooks(page)
    await page.reload()

    await goToBookFromList(page, bookId, title)
    await ensurePageNumber(page, 3)
  })

  test('PageInput_Should_UpdateCurrentPage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePageNumber(page, 1)

    await setPageInHeader(page, 7)
    await ensurePageNumber(page, 7)

    await setPageInHeader(page, 4)
    await ensurePageNumber(page, 4)
  })

  test('PageInput_Should_PersistCurrentPage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePageNumber(page, 1)

    await setPageInHeader(page, 3)
    await ensurePageNumber(page, 3)

    await goToBooks(page)
    await page.reload()

    await goToBookFromList(page, bookId, title)

    await ensurePageNumber(page, 3)
  })

  test('PageKeys_Should_ScrollAndChangePage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePageNumber(page, 1)
    await setPageInHeader(page, 3)
    await page.reload()
    await ensureBookLoaded(page, bookId, title)

    await page.keyboard.press('PageDown')
    await page.keyboard.press('PageDown')
    await page.waitForTimeout(1000)
    await ensurePageNumber(page, 4)

    await openBookmarksPanel(page)

    await page.keyboard.press('PageUp')
    await page.keyboard.press('PageUp')
    await page.keyboard.press('PageUp')
    await page.keyboard.press('PageUp')
    await page.waitForTimeout(1000)
    await ensurePageNumber(page, 2)
  })

  test('ArrowKeys_Should_ScrollAndChangePage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePageNumber(page, 1)
    await setPageInHeader(page, 3)
    await page.reload()
    await ensureBookLoaded(page, bookId, title)

    for (let i = 0; i < 40; i++) {
      await page.keyboard.press('ArrowDown')
    }
    await page.waitForTimeout(1000)
    await ensurePageNumber(page, 4)

    await openBookmarksPanel(page)

    for (let i = 0; i < 80; i++) {
      await page.keyboard.press('ArrowUp')
    }
    await page.waitForTimeout(1000)
    await ensurePageNumber(page, 2)
  })
})
