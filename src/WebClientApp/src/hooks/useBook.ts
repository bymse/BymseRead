import { ensureBookStorage, getStoredBook } from '@storage/index.ts'
import { useEffect, useState } from 'preact/hooks'
import { BookInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader.ts'

export const useBook = (bookId?: string) => {
  const [book, setBook] = useState<BookInfo | undefined>()
  const [isOffline, setIsOffline] = useState(false)
  const { client } = useWebApiClient()
  const { handleFetchError } = useErrorHandler()

  const load = async () => {
    if (!bookId) {
      return Promise.resolve(undefined)
    }

    try {
      const book = await client.webApi.books.byBookId(bookId).get()
      if (book) {
        await ensureBookStorage(book)
      }
      return setBook(book)
    } catch (e) {
      const { isBackendUnavailable } = handleFetchError(e as Error)
      if (!isBackendUnavailable) {
        return
      }

      const storedBook = await getStoredBook(bookId)
      if (storedBook) {
        setBook(storedBook)
        setIsOffline(true)
      }
    }
  }

  const { isLoading, showSpinner, execute } = useExecuteWithLoader(load, true)

  useEffect(() => {
    void execute()
  }, [bookId, client, handleFetchError])

  return {
    book,
    isLoading,
    reload: execute,
    showSpinner,
    isOffline,
  }
}
