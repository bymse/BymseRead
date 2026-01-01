import type { BookCollectionItem, BookInfo } from '@api/models'
import { BookMeta } from '@storage/db.ts'
import { evaluateForStorageUpdate } from '@storage/utils.ts'
import { BookFiles, cacheBookFiles, removeBookFilesCache } from './filesCache.ts'
import { deleteBooksMeta, readAllBooksMeta, readBookMeta, writeBooksMeta } from './metaStore.ts'

export const ensureBookStorage = async (book: BookInfo): Promise<void> => {
  try {
    const storedBook = await readBookMeta(book.bookId!)
    if (book.status !== 'Active') {
      if (!storedBook) return
      const filesToRemove = {
        bookId: storedBook.bookId,
        fileUrl: storedBook.fileUrl,
        coverUrl: storedBook.coverUrl,
      }

      await Promise.allSettled([deleteBooksMeta([storedBook.bookId]), removeBookFilesCache([filesToRemove])])
      return
    }
    const { newBook, newFiles, oldFiles } = evaluateForStorageUpdate(book, storedBook)

    if (oldFiles) await removeBookFilesCache([oldFiles])
    if (newBook) await writeBooksMeta([newBook])
    if (newFiles) await cacheBookFiles([newFiles])
  } catch {
    /* empty */
  }
}

export const ensureBooksStorage = async (books: BookCollectionItem[]): Promise<void> => {
  try {
    const storedBooks = await readAllBooksMeta()

    const activeBookById = new Map(books.map(book => [book.bookId, book]))
    const storedBookById = new Map(storedBooks.map(book => [book.bookId, book]))

    const filesToAdd: BookFiles[] = []
    const booksToAdd: BookMeta[] = []
    const filesToRemove: BookFiles[] = []
    const booksToRemove: string[] = []

    for (const activeBook of books) {
      const storedBook = storedBookById.get(activeBook.bookId!)
      const { newBook, newFiles, oldFiles } = evaluateForStorageUpdate(activeBook, storedBook)
      if (newBook) booksToAdd.push(newBook)
      if (newFiles) filesToAdd.push(newFiles)
      if (oldFiles) filesToRemove.push(oldFiles)
    }

    for (const storedBook of storedBooks) {
      const activeBook = activeBookById.get(storedBook.bookId)
      if (!activeBook) {
        booksToRemove.push(storedBook.bookId)
        filesToRemove.push({ bookId: storedBook.bookId, fileUrl: storedBook.fileUrl, coverUrl: storedBook.coverUrl })
      }
    }

    await Promise.allSettled([deleteBooksMeta(booksToRemove), removeBookFilesCache(filesToRemove)])
    await writeBooksMeta(booksToAdd)
    await cacheBookFiles(filesToAdd)
  } catch {
    /* empty */
  }
}

export const getStoredBooks = async (): Promise<BookCollectionItem[]> => {
  const metas = await readAllBooksMeta()
  const books: BookCollectionItem[] = []
  for (const meta of metas) {
    books.push({
      bookId: meta.bookId,
      fileUrl: meta.fileUrl,
      coverUrl: meta.coverUrl,
      title: meta.title,
      lastBookmark: meta.lastBookmark,
      currentPage: meta.currentPage,
      pages: meta.pages,
    })
  }
  return books
}

export const getStoredBook = async (bookId: string): Promise<BookInfo | undefined> => {
  const bookMeta = await readBookMeta(bookId)
  if (!bookMeta?.fileUrl) {
    return undefined
  }

  return {
    bookId: bookMeta.bookId,
    currentPage: bookMeta.currentPage,
    title: bookMeta.title,
    coverUrl: bookMeta.coverUrl,
    status: 'Active',
    lastBookmark: bookMeta.lastBookmark,
    bookFile: {
      name: 'offline',
      fileUrl: bookMeta.fileUrl,
    },
    pages: bookMeta.pages,
  }
}
