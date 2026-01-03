import { useEffect, useState, useRef } from 'preact/hooks'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { BookInfo } from '@api/models'
import { updateBookCurrentPage } from '@storage/index.ts'

export const useCurrentPage = (book?: BookInfo) => {
  const { client } = useWebApiClient()
  const { handleFetchError } = useErrorHandler()
  const debounceTimer = useRef<number | undefined>(undefined)
  const [currentPage, setCurrentPage] = useState<number>(1)

  const handleUpdateCurrentPage = (page: number) => {
    if (!book?.bookId) {
      return
    }

    const changedAt = new Date(Date.now())
    void updateBookCurrentPage(book.bookId, { page, changedAt })

    void client.webApi.books.byBookId(book.bookId).progress.currentPage.put({ page, changedAt }).catch(handleFetchError)
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
