import { test, expect } from '@playwright/test'
import { registerAction } from '../actions/authActions.js'
import {
  addBook,
  clickToastLink,
  clickBookCard,
  verifyBookInSection,
  deleteBook,
  verifyBookNotExists,
  generateBookTitle,
} from '../actions/bookActions.js'
import { SITE_URL } from '../utils/constants.js'

test.describe('Book Management', () => {
  test('AddBook_Should_ShowInList_And_ShowToast_When_SubmittedViaForm', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    const bookId = await addBook(page, title, 'file-otel.pdf')

    const toast = page.getByTestId('toast')
    await expect(toast).toBeVisible()
    await expect(toast).toContainText('New book was added')

    const toastLink = page.getByTestId('toast-link')
    await expect(toastLink).toBeVisible()

    await verifyBookInSection(page, 'new', bookId)
  })

  test('AddBook_Should_NavigateToBookPage_When_ClickingToastLink', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    const bookId = await addBook(page, title, 'file-otel.pdf')

    await clickToastLink(page)

    expect(page.url()).toContain(`/books/${bookId}`)
  })

  test('AddBook_Should_NavigateToBookPage_When_ClickingBookCard', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const title = generateBookTitle()
    const bookId = await addBook(page, title, 'file-otel.pdf')

    await page.getByTestId('toast').waitFor({ state: 'hidden', timeout: 10000 })

    await clickBookCard(page, bookId)

    expect(page.url()).toContain(`/books/${bookId}`)
  })

  test('AddMultipleBooks_Should_AppearInNewSection', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const book1Id = await addBook(page, generateBookTitle(), 'file-otel.pdf')
    await page.getByTestId('toast').waitFor({ state: 'hidden', timeout: 10000 })

    const book2Id = await addBook(page, generateBookTitle(), 'file-otel.pdf')
    await page.getByTestId('toast').waitFor({ state: 'hidden', timeout: 10000 })

    const book3Id = await addBook(page, generateBookTitle(), 'file-otel.pdf')
    await page.getByTestId('toast').waitFor({ state: 'hidden', timeout: 10000 })

    await verifyBookInSection(page, 'new', book1Id)
    await verifyBookInSection(page, 'new', book2Id)
    await verifyBookInSection(page, 'new', book3Id)

    const newSection = page.getByTestId('books-block-new')
    const bookCards = newSection.locator('[data-testid^="book-card-"]')
    await expect(bookCards).toHaveCount(3)
  })

  test('DeleteBook_Should_RemoveFromList_And_BeUnavailable', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerAction(page)

    const book1Id = await addBook(page, generateBookTitle(), 'file-otel.pdf')
    const book2Id = await addBook(page, generateBookTitle(), 'file-otel.pdf')
    const book3Id = await addBook(page, generateBookTitle(), 'file-otel.pdf')

    await verifyBookInSection(page, 'new', book1Id)
    await verifyBookInSection(page, 'new', book2Id)
    await verifyBookInSection(page, 'new', book3Id)

    await deleteBook(page, book2Id)

    await verifyBookInSection(page, 'new', book1Id)
    await verifyBookInSection(page, 'new', book3Id)

    await verifyBookNotExists(page, book2Id)
  })
})
