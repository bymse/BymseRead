import { useEffect, useState, useRef } from 'preact/hooks'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { BookInfo } from '@api/models'

export const useCurrentPage = (book?: BookInfo) => {
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()
  const debounceTimer = useRef<number | undefined>(undefined)
  const [currentPage, setCurrentPage] = useState<number>(0)

  const handleUpdateCurrentPage = (page: number) => {
    if (!book?.bookId) {
      return
    }

    client.webApi.books.byBookId(book?.bookId).progress.currentPage.put({ page }).catch(handleError)
  }

  const updateCurrentPage = (page: number) => {
    setCurrentPage(page)

    if (debounceTimer.current) {
      clearTimeout(debounceTimer.current)
    }

    debounceTimer.current = window.setTimeout(() => {
      handleUpdateCurrentPage(page)
    }, 300)
  }

  useEffect(() => {
    if (book?.currentPage) {
      setCurrentPage(book.currentPage)
    }
  }, [book])

  return {
    currentPage,
    updateCurrentPage,
  }
}
