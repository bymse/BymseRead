import { test } from '@playwright/test'
import { addBookThenOpenReader, goToBookFromList, goToBooks } from '../actions/booksActions.js'
import { ensurePage, scrollToPageNumber, setPageInHeader } from '../actions/readerActions.js'
import { SITE_URL } from '../utils/constants.js'
import { registerUser } from '../actions/authActions.js'

test.describe('Reader tests', () => {
  test('ReaderOnScroll_Should_PersistCurrentPage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePage(page, 1)

    await scrollToPageNumber(page, 3)
    await ensurePage(page, 3)

    await goToBooks(page)
    await page.reload()

    await goToBookFromList(page, bookId, title)
    await ensurePage(page, 3)
  })

  test('PageInput_Should_UpdateCurrentPage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePage(page, 1)

    await setPageInHeader(page, 7)
    await ensurePage(page, 7)

    await setPageInHeader(page, 4)
    await ensurePage(page, 4)
  })

  test('PageInput_Should_PersistCurrentPage', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBookThenOpenReader(page, '10-page.pdf')
    await ensurePage(page, 1)

    await setPageInHeader(page, 3)
    await ensurePage(page, 3)

    await goToBooks(page)
    await page.reload()

    await goToBookFromList(page, bookId, title)

    await ensurePage(page, 3)
  })
})
