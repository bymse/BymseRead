import { useEffect, useState } from 'preact/hooks'
import { BookInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader.ts'

export const useBook = (bookId?: string) => {
  const [book, setBook] = useState<BookInfo | undefined>()
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  const load = async () => {
    if (!bookId) {
      return Promise.resolve(undefined)
    }

    try {
      const book = await client.webApi.books.byBookId(bookId).get()
      return setBook(book)
    } catch (e) {
      return handleError(e as Error, false)
    }
  }

  const { isLoading, showSpinner, execute } = useExecuteWithLoader(load, true)

  useEffect(() => {
    void execute()
  }, [bookId, client, handleError])

  return {
    book,
    isLoading,
    reload: execute,
    showSpinner,
  }
}
