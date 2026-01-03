import { BookInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useEffect, useState } from 'preact/hooks'
import { updateBookLastBookmark } from '@storage/index.ts'

type LastPageBookmark = {
  page: number
  date: Date
}

export const useBookmarks = (book?: BookInfo) => {
  const { client } = useWebApiClient()
  const { handleFetchError } = useErrorHandler()
  const [lastPageBookmark, setLastPageBookmark] = useState<LastPageBookmark | undefined>(undefined)

  useEffect(() => {
    if (book?.lastBookmark) {
      setLastPageBookmark({
        page: book.lastBookmark.page!,
        date: book.lastBookmark.createdAt!,
      })
    }
  }, [book])

  const handleMarkAsLastPage = (page: number) => {
    if (!book?.bookId) {
      return
    }

    const prevLastPageBookmark = lastPageBookmark
    const changedAt = new Date(Date.now())

    setLastPageBookmark({
      page,
      date: changedAt,
    })

    void updateBookLastBookmark(book.bookId, { page, createdAt: changedAt })

    client.webApi.books
      .byBookId(book.bookId)
      .bookmarks.lastPage.post({
        page,
        changedAt,
      })
      .catch(e => {
        const { isBackendUnavailable } = handleFetchError(e as Error)
        if (!isBackendUnavailable && prevLastPageBookmark && book.bookId) {
          setLastPageBookmark(prevLastPageBookmark)
          void updateBookLastBookmark(book.bookId, {
            page: prevLastPageBookmark.page,
            createdAt: prevLastPageBookmark.date,
          })
        }
      })
  }

  return {
    handleMarkAsLastPage,
    lastPageBookmark,
  }
}
