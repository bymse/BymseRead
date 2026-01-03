import { BookInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useEffect, useState } from 'preact/hooks'
import { updateBookLastBookmark, postponeBookmarkUpdate } from '@storage/index.ts'

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
    const createdAt = new Date(Date.now())

    setLastPageBookmark({
      page,
      date: createdAt,
    })

    void updateBookLastBookmark(book.bookId, { page, createdAt })

    client.webApi.books
      .byBookId(book.bookId)
      .bookmarks.lastPage.post({
        page,
        createdAt,
      })
      .catch(e => {
        const { isBackendUnavailable } = handleFetchError(e as Error)
        if (isBackendUnavailable) {
          void postponeBookmarkUpdate(book.bookId!, { page, createdAt })
        } else if (prevLastPageBookmark) {
          setLastPageBookmark(prevLastPageBookmark)

          void updateBookLastBookmark(book.bookId!, {
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
