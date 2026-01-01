import { openDB, type IDBPDatabase, DBSchema } from 'idb'

export const DB_NAME = 'bymse-read-offline'
export const DB_VERSION = 2
export const BOOKS_STORE = 'books'

export interface BookMeta {
  fileUrl?: string
  coverUrl?: string
  title: string
  bookId: string
  pages?: number

  currentPage?: number
  lastBookmark?: Bookmark
}

export interface Bookmark {
  page: number
  createdAt: Date
}

interface OfflineDB extends DBSchema {
  [BOOKS_STORE]: {
    key: string
    value: BookMeta
  }
}

let dbPromise: Promise<IDBPDatabase<OfflineDB>> | null = null

export const getDB = (): Promise<IDBPDatabase<OfflineDB>> => {
  if (!dbPromise) {
    dbPromise = openDB<OfflineDB>(DB_NAME, DB_VERSION, {
      upgrade(db) {
        if (!db.objectStoreNames.contains(BOOKS_STORE)) {
          db.createObjectStore(BOOKS_STORE, { keyPath: 'bookId' })
        }
      },
    })
  }
  return dbPromise
}
