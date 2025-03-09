import { useEffect, useState } from 'preact/hooks'
import { BookInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'

export const useBook = (bookId?: string) => {
  const [isLoading, setIsLoading] = useState(true)
  const [book, setBook] = useState<BookInfo | undefined>()
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  const load = () => {
    if (!bookId) {
      setIsLoading(false)
      return
    }

    setIsLoading(true)
    client.webApi.books
      .byBookId(bookId)
      .get()
      .then(e => {
        setBook(e)
        setIsLoading(false)
      })
      .catch(e => {
        handleError(e as Error)
        setIsLoading(false)
      })
  }

  useEffect(() => {
    load()
  }, [bookId, client, handleError])

  return {
    book,
    isLoading,
    reload: load,
  }
}
