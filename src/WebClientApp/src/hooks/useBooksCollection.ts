import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo, BookShortInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader.ts'
import { getStoredBooks, ensureBooksStored } from '@storage/index'

const maxRetries = 10

interface Books extends BooksCollectionInfo {
  offlineBooks?: BookShortInfo[]
}

export const useBooksCollection = () => {
  const [collection, setCollection] = useState<Books>()
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  let retryCount = 0

  const load = async () => {
    try {
      const res = await client.webApi.books.get()
      setCollection(res)
      retryCount = 0

      if (res) {
        void ensureBooksStored(res.activeBooks || [])
      }
    } catch (e) {
      const { isBackendUnavailable } = handleError(e as Error, false)

      if (isBackendUnavailable) {
        const storedBooks = await getStoredBooks()
        setCollection({
          offlineBooks: storedBooks,
        })
        return
      }

      if (retryCount < maxRetries) {
        retryCount++
        setTimeout(() => void load(), Math.random() * 4000 + 3000)
      }
    }
  }

  const { isLoading, showSpinner, execute } = useExecuteWithLoader(load, true)

  useEffect(() => {
    void execute()
  }, [client])

  return {
    collection,
    reload: execute,
    isLoading,
    showSpinner,
  }
}
