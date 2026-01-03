import { openDB, type IDBPDatabase, DBSchema } from 'idb'

export const DB_NAME = 'bymse-read-offline'
export const DB_VERSION = 3
export const BOOKS_STORE = 'books'
export const POSTPONED_UPDATES_STORE = 'postponed-updates'

export interface BookMeta {
  fileUrl?: string
  coverUrl?: string
  title: string
  bookId: string
  pages?: number

  currentPage?: CurrentPage
  lastBookmark?: Bookmark
}

export interface Bookmark {
  page: number
  createdAt: Date
}

export interface CurrentPage {
  page: number
  createdAt: Date
}

export interface PostponedUpdates {
  bookId: string
  lastBookmark?: Bookmark
  currentPage?: CurrentPage
}

interface OfflineDB extends DBSchema {
  [BOOKS_STORE]: {
    key: string
    value: BookMeta
  }
  [POSTPONED_UPDATES_STORE]: {
    key: string
    value: PostponedUpdates
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
        if (!db.objectStoreNames.contains(POSTPONED_UPDATES_STORE)) {
          db.createObjectStore(POSTPONED_UPDATES_STORE, { keyPath: 'bookId' })
        }
      },
    })
  }
  return dbPromise
}
