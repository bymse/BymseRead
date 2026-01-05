import { test, expect } from '@playwright/test'
import { registerUser } from '../actions/authActions.js'
import {
  addBook,
  deleteBook,
  goToBookFromList,
  ensureBookInSection,
  ensureBookNotExists,
  makeBookActive,
  goToBooks,
} from '../actions/booksActions.js'
import { clickToastLink } from '../actions/coomonActions.js'
import { ensureBookLoaded } from '../actions/readerActions.js'
import { SITE_URL } from '../utils/constants.js'

test.describe('Book Management', () => {
  test('AddBook_Should_ShowInList_And_ShowToast_When_SubmittedViaForm', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBook(page, 'file-otel.pdf')

    const toast = page.getByTestId('toast')
    await expect(toast).toBeVisible()
    await expect(toast).toContainText('New book was added')

    await ensureBookInSection(page, 'new', bookId, title)
  })

  test('AddBook_Should_NavigateToBookPage_When_ClickingToastLink', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBook(page, 'file-otel.pdf')
    await clickToastLink(page)
    await ensureBookLoaded(page, bookId, title)
  })

  test('AddBook_Should_NavigateToBookPage_When_ClickingBookCard', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBook(page, 'file-otel.pdf')

    await goToBookFromList(page, bookId, title)
    await ensureBookLoaded(page, bookId, title)
  })

  test('AddMultipleBooks_Should_AppearInNewSection', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const book1 = await addBook(page, 'file-otel.pdf')
    const book2 = await addBook(page, 'file-otel.pdf')
    const book3 = await addBook(page, 'file-otel.pdf')

    await ensureBookInSection(page, 'new', book1.bookId, book1.title)
    await ensureBookInSection(page, 'new', book2.bookId, book2.title)
    await ensureBookInSection(page, 'new', book3.bookId, book3.title)
  })

  test('DeleteBook_Should_RemoveFromList_And_BeUnavailable', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const book1 = await addBook(page, 'file-otel.pdf')
    const book2 = await addBook(page, 'file-otel.pdf')
    const book3 = await addBook(page, 'file-otel.pdf')

    await ensureBookInSection(page, 'new', book1.bookId, book1.title)
    await ensureBookInSection(page, 'new', book2.bookId, book2.title)
    await ensureBookInSection(page, 'new', book3.bookId, book3.title)

    await deleteBook(page, book2.bookId)

    await ensureBookInSection(page, 'new', book1.bookId, book1.title)
    await ensureBookInSection(page, 'new', book3.bookId, book3.title)

    await ensureBookNotExists(page, book2.bookId)
  })

  test('Book_Should_BeInActiveSection_When_ProgressMade', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)

    const { bookId, title } = await addBook(page, '10-page.pdf')

    await ensureBookInSection(page, 'new', bookId, title)

    await makeBookActive(page, bookId)

    await goToBooks(page)

    await ensureBookInSection(page, 'active', bookId, title)
  })
})
