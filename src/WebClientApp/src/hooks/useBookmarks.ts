import { BookInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useEffect, useState } from 'preact/hooks'

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

    setLastPageBookmark({
      page,
      date: new Date(),
    })

    client.webApi.books
      .byBookId(book.bookId)
      .bookmarks.lastPage.post({
        page,
      })
      .catch(e => {
        if (prevLastPageBookmark) {
          setLastPageBookmark(prevLastPageBookmark)
        }
        handleFetchError(e as Error)
      })
  }

  return {
    handleMarkAsLastPage,
    lastPageBookmark,
  }
}
