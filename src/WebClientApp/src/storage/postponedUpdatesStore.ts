import { Bookmark, CurrentPage, getDB, POSTPONED_UPDATES_STORE, PostponedUpdates } from '@storage/db.ts'

export const getNextPostponedUpdate = async (): Promise<PostponedUpdates | undefined> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(POSTPONED_UPDATES_STORE, 'readonly')
    const store = transaction.objectStore(POSTPONED_UPDATES_STORE)
    const book = await store.openCursor()
    return book?.value
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to update current page:', error)
    return Promise.resolve(undefined)
  }
}

export const removePostponedUpdate = async (bookId: string): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(POSTPONED_UPDATES_STORE, 'readwrite')
    const store = transaction.objectStore(POSTPONED_UPDATES_STORE)
    await store.delete(bookId)
    await transaction.done
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to delete postponed update:', error)
    return Promise.resolve(undefined)
  }
}

export const postponeCurrentPageUpdate = async (bookId: string, currentPage: CurrentPage): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(POSTPONED_UPDATES_STORE, 'readwrite')
    const store = transaction.objectStore(POSTPONED_UPDATES_STORE)
    let book = await store.get(bookId)
    if (!book) {
      book = { bookId }
    }

    book.currentPage = currentPage
    await store.put(book)
    await transaction.done
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to update current page:', error)
  }
}

export const postponeBookmarkUpdate = async (bookId: string, bookmark: Bookmark): Promise<void> => {
  try {
    const db = await getDB()
    const transaction = db.transaction(POSTPONED_UPDATES_STORE, 'readwrite')
    const store = transaction.objectStore(POSTPONED_UPDATES_STORE)
    let book = await store.get(bookId)
    if (!book) {
      book = { bookId }
    }

    book.lastBookmark = bookmark
    await store.put(book)
    await transaction.done
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error('Failed to update last bookmark:', error)
  }
}
