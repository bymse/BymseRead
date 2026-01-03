import { type BookMeta, type CurrentPage, type Bookmark, BOOKS_STORE, getDB } from './db.ts'

export const writeBooksMeta = async (books: BookMeta[]): Promise<void> => {
  if (books.length === 0) return

  try {
    const db = await getDB()
    const transaction = db.transaction(BOOKS_STORE, 'readwrite')
    const store = transaction.objectStore(BOOKS_STORE)
    for (const book of books) {
      await store.put(book)
    }
    await transaction.done
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to save books meta:', error)
  }
}

export const deleteBooksMeta = async (bookIds: string[]): Promise<void> => {
  if (bookIds.length === 0) return

  try {
    const db = await getDB()
    const transaction = db.transaction(BOOKS_STORE, 'readwrite')
    const store = transaction.objectStore(BOOKS_STORE)
    for (const bookId of bookIds) {
      await store.delete(bookId)
    }
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to remove book meta:', error)
  }
}

export const readBookMeta = async (bookId: string): Promise<BookMeta | undefined> => {
  try {
    const db = await getDB()
    return await db.get(BOOKS_STORE, bookId)
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to get book meta:', error)
    return undefined
  }
}

export const readAllBooksMeta = async (): Promise<BookMeta[]> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(BOOKS_STORE, 'readonly')
    const store = transaction.objectStore(BOOKS_STORE)
    return await store.getAll()
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to get all books:', error)
    return []
  }
}

export const setBookFilesMeta = async (bookId: string, fileUrl?: string, coverUrl?: string): Promise<void> => {
  const db = await getDB()
  const transaction = db.transaction(BOOKS_STORE, 'readwrite')
  const store = transaction.objectStore(BOOKS_STORE)
  const book = await store.get(bookId)
  if (!book) {
    return
  }

  if (fileUrl) {
    book.fileUrl = fileUrl
  }

  if (coverUrl) {
    book.coverUrl = coverUrl
  }
  await store.put(book)
  await transaction.done
}

export const resetBookFilesMeta = async (bookId: string, fileUrl?: string, coverUrl?: string): Promise<void> => {
  const db = await getDB()
  const transaction = db.transaction(BOOKS_STORE, 'readwrite')
  const store = transaction.objectStore(BOOKS_STORE)
  const book = await store.get(bookId)
  if (!book) {
    return
  }

  if (fileUrl && book.fileUrl === fileUrl) {
    book.fileUrl = undefined
  }

  if (coverUrl && book.coverUrl === coverUrl) {
    book.coverUrl = undefined
  }

  await store.put(book)
  await transaction.done
}

export const updateBookCurrentPage = async (bookId: string, currentPage: CurrentPage): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(BOOKS_STORE, 'readwrite')
    const store = transaction.objectStore(BOOKS_STORE)
    const book = await store.get(bookId)
    if (!book) {
      return
    }

    book.currentPage = currentPage
    await store.put(book)
    await transaction.done
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to update current page:', error)
  }
}

export const updateBookLastBookmark = async (bookId: string, bookmark: Bookmark): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(BOOKS_STORE, 'readwrite')
    const store = transaction.objectStore(BOOKS_STORE)
    const book = await store.get(bookId)
    if (!book) {
      return
    }

    book.lastBookmark = bookmark
    await store.put(book)
    await transaction.done
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to update last bookmark:', error)
  }
}
