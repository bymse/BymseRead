import { expect, Page } from '@playwright/test'
import { fileURLToPath } from 'node:url'

const getFilePath = (fileName: string) => {
  return fileURLToPath(new URL(`../testData/${fileName}`, import.meta.url))
}

export async function addBook(page: Page, title: string, fileName: string): Promise<string> {
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

  return bookId
}

export async function clickToastLink(page: Page): Promise<void> {
  await page.getByTestId('toast-link').click()
  await page.getByTestId('book-page-header').waitFor()
}

export async function clickBookCard(page: Page, bookId: string): Promise<void> {
  await page.getByTestId(`book-card-${bookId}`).click()
  await page.getByTestId('book-page-header').waitFor()
}

export async function verifyBookInSection(page: Page, sectionName: string, bookId: string): Promise<void> {
  const section = page.getByTestId(`books-block-${sectionName.toLowerCase()}`)
  await section.waitFor()

  const bookCard = section.getByTestId(`book-card-${bookId}`)
  await expect(bookCard).toBeVisible()
}

export async function deleteBook(page: Page, bookId: string): Promise<void> {
  await page.goto(`/books/${bookId}`)
  await page.getByTestId('book-page-header').waitFor()

  await page.getByTestId('reader-header-dropdown-button').click()

  await page.getByTestId('reader-header-delete-button').click()

  await page.getByTestId('delete-book-modal').waitFor()

  await page.getByTestId('popup-form-submit').click()

  await page.waitForURL(url => url.pathname === '/books')
}

export async function verifyBookNotExists(page: Page, bookId: string): Promise<void> {
  await page.goto(`/books/${bookId}`)

  const notFoundPage = page.getByTestId('not-found-page')
  await expect(notFoundPage).toBeVisible()

  await page.goto('/books')

  const bookCard = page.getByTestId(`book-card-${bookId}`)
  await expect(bookCard).not.toBeVisible()
}

export function generateBookTitle(): string {
  return `Test Book ${Date.now()}`
}
