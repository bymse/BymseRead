import type { BookInfo, BookShortInfo } from '@api/models'
import { openDB, type IDBPDatabase } from 'idb'

const DB_NAME = 'bymse-read-offline'
const DB_VERSION = 1
const STORE_NAME = 'books'

interface BookMeta {
  fileUrl?: string
  coverUrl?: string
  title: string
  bookId: string
}

interface BookMetaDB {
  [STORE_NAME]: {
    key: string
    value: BookMeta
  }
}

let dbPromise: Promise<IDBPDatabase<BookMetaDB>> | null = null

const getDB = (): Promise<IDBPDatabase<BookMetaDB>> => {
  if (!dbPromise) {
    dbPromise = openDB<BookMetaDB>(DB_NAME, DB_VERSION, {
      upgrade(db) {
        if (!db.objectStoreNames.contains(STORE_NAME)) {
          db.createObjectStore(STORE_NAME, { keyPath: 'bookId' })
        }
      },
    })
  }
  return dbPromise
}

export const writeBooksMeta = async (books: BookInfo[]): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(STORE_NAME, 'readwrite')
    books.forEach(book => transaction.objectStore(STORE_NAME).put({ filesCached: false, ...book }))
  } catch (error) {
    console.error('Failed to save books meta:', error)
  }
}

export const deleteBooksMeta = async (bookIds: string[]): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(STORE_NAME, 'readwrite')
    bookIds.forEach(bookId => transaction.objectStore(STORE_NAME).delete(bookId))
  } catch (error) {
    console.error('Failed to remove book meta:', error)
  }
}

export const readBookMeta = async (bookId: string): Promise<BookMeta | undefined> => {
  try {
    const db = await getDB()
    return await db.get(STORE_NAME, bookId)
  } catch (error) {
    console.error('Failed to get book meta:', error)
    return undefined
  }
}

export const readAllBooksMeta = async (): Promise<BookMeta[]> => {
  try {
    const db = await getDB()
    return await db.getAll(STORE_NAME)
  } catch (error) {
    console.error('Failed to get all books:', error)
    return []
  }
}

export const setBookFilesMeta = async (
  bookId: string,
  fileUrl?: string | null,
  coverUrl?: string | null,
): Promise<void> => {
  const db = await getDB()
  const transaction = db.transaction(STORE_NAME, 'readwrite')
  const book: BookMeta | undefined = await transaction.objectStore(STORE_NAME).get(bookId)
  if (!book) {
    return
  }

  if (fileUrl) {
    book.fileUrl = fileUrl
  }

  if (coverUrl) {
    book.coverUrl = coverUrl
  }
  transaction.objectStore(STORE_NAME).put(book)
}
