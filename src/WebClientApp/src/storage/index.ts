import type { BookInfo, BooksCollectionInfo, BookShortInfo } from '@api/models'
import { BookFiles, cacheBookFiles, removeBookFilesCache } from './filesCache.ts'
import { deleteBooksMeta, readAllBooksMeta, readBookMeta, writeBooksMeta } from './metaStore.ts'

const isSameUrl = (url1?: string | null, url2?: string | null): boolean => {
  try {
    const url1Url = new URL(url1!)
    const url2Url = new URL(url2!)
    return url1Url.origin === url2Url.origin && url1Url.pathname === url2Url.pathname
  } catch {
    return false
  }
}

export const ensureBooksStored = async (books: BookShortInfo[]): Promise<void> => {
  try {
    const cachedBooks = await readAllBooksMeta()

    const activeBookById = new Map(books.map(book => [book.bookId, book]))
    const cachedBookById = new Map(cachedBooks.map(book => [book.bookId, book]))

    const filesToAdd: BookFiles[] = []
    const bookToAdd: BookShortInfo[] = []
    const filesToRemove: BookFiles[] = []

    for (let activeBook of books) {
      const cachedBook = cachedBookById.get(activeBook.bookId!)
      if (!cachedBook) {
        filesToAdd.push({ bookId: activeBook.bookId!, fileUrl: activeBook.fileUrl, coverUrl: activeBook.coverUrl })
        bookToAdd.push(activeBook)
        continue
      }

      const isSameFile = isSameUrl(cachedBook.fileUrl, activeBook.fileUrl)
      const isSameCover = isSameUrl(cachedBook.coverUrl, activeBook.coverUrl)

      if (!isSameFile || !isSameCover) {
        filesToRemove.push({
          bookId: activeBook.bookId!,
          fileUrl: isSameFile ? null : cachedBook.fileUrl,
          coverUrl: isSameCover ? null : cachedBook.coverUrl,
        })
        filesToAdd.push({
          bookId: activeBook.bookId!,
          fileUrl: isSameFile ? null : activeBook.fileUrl,
          coverUrl: isSameCover ? null : activeBook.coverUrl,
        })
        bookToAdd.push(activeBook)
      }
    }

    for (let cachedBook of cachedBooks) {
      const activeBook = activeBookById.get(cachedBook.bookId)
      if (!activeBook) {
        filesToRemove.push({ bookId: cachedBook.bookId!, fileUrl: cachedBook.fileUrl, coverUrl: cachedBook.coverUrl })
      }
    }

    await Promise.allSettled([deleteBooksMeta(filesToRemove.map(file => file.bookId!)), writeBooksMeta(bookToAdd)])
    await Promise.allSettled([cacheBookFiles(filesToAdd), removeBookFilesCache(filesToRemove)])
  } catch {}
}

export const ensureBookStored = async (book: BookInfo): Promise<void> => {
  try {
  } catch {}
}

export const getStoredBooks = async (): Promise<BookShortInfo[]> => {
  const books = await readAllBooksMeta()
  return books.filter(book => book.fileUrl)
}

export const getStoredBook = async (bookId: string): Promise<BookInfo | undefined> => {
  const book = await readBookMeta(bookId)
  return book?.fileUrl ? book : undefined
}
