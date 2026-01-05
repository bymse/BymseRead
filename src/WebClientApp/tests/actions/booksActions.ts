import { expect, Page } from '@playwright/test'
import { fileURLToPath } from 'node:url'
import { clickToastLink } from './coomonActions.js'
import { ensureBookLoaded, setPageInHeader } from './readerActions.js'

const getFilePath = (fileName: string) => {
  return fileURLToPath(new URL(`../testData/${fileName}`, import.meta.url))
}

export async function addBook(page: Page, fileName: string): Promise<{ bookId: string; title: string }> {
  const title = `Test Book ${Date.now()}`
  await page.getByTestId('header-add-book-button').click()

  await page.getByTestId('add-book-form').waitFor()

  await page.getByTestId('input-title').fill(title)

  const filePath = getFilePath(fileName)
  await page.getByTestId('file-button-input').setInputFiles(filePath)

  await page.getByTestId('popup-form-submit').click()

  await page.getByTestId('add-book-form').waitFor({ state: 'hidden' })

  const toast = page.getByTestId('toast')
  await toast.waitFor()

  const toastLink = page.getByTestId('toast-link')
  const href = await toastLink.getAttribute('href')

  if (!href) {
    throw new Error('Toast link href not found')
  }

  const bookId = href.split('/').pop()

  if (!bookId) {
    throw new Error('Could not extract bookId from toast link')
  }

  return { bookId, title }
}

export async function addBookThenOpenReader(page: Page, fileName: string): Promise<{ bookId: string; title: string }> {
  const { bookId, title } = await addBook(page, fileName)
  await clickToastLink(page)
  await ensureBookLoaded(page, bookId, title)
  return { bookId, title }
}

export async function goToBookFromList(page: Page, bookId: string, title: string): Promise<void> {
  await page.getByTestId(`book-card-${bookId}`).click()
  await page.getByTestId('book-page-header').waitFor()
  await ensureBookLoaded(page, bookId, title)
}

export async function ensureBookInSection(
  page: Page,
  sectionName: string,
  bookId: string,
  title: string,
): Promise<void> {
  const section = page.getByTestId(`books-block-${sectionName.toLowerCase()}`)
  await section.waitFor()

  const bookCard = section.getByTestId(`book-card-${bookId}`)
  await expect(bookCard).toBeVisible()
  const texts = await bookCard.allInnerTexts()
  expect(texts[0]).toContain(title)
}

export async function deleteBook(page: Page, bookId: string): Promise<void> {
  await page.goto(`/books/${bookId}`)
  await ensureBookLoaded(page, bookId)

  await page.getByTestId('reader-header-dropdown-button').click()

  await page.getByTestId('reader-header-delete-button').click()

  await page.getByTestId('delete-book-modal').waitFor()

  await page.getByTestId('popup-form-submit').click()

  await page.waitForURL(url => url.pathname === '/books')
}

export async function ensureBookNotExists(page: Page, bookId: string): Promise<void> {
  await page.goto(`/books/${bookId}`)

  const notFoundPage = page.getByTestId('not-found-page')
  await expect(notFoundPage).toBeVisible()

  await goToBooks(page)

  const bookCard = page.getByTestId(`book-card-${bookId}`)
  await expect(bookCard).not.toBeVisible()
}

export async function goToBooks(page: Page): Promise<void> {
  await page.goto('/books')
  await page.waitForFunction(() => document.title === 'Books — BymseRead')
}

export async function makeBookActive(page: Page, bookId: string): Promise<void> {
  await page.goto(`/books/${bookId}`)
  await ensureBookLoaded(page, bookId)
  await setPageInHeader(page, 3)
}
