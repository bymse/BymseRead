import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo, BookCollectionItem } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader.ts'
import { getStoredBooks, ensureBooksStorage } from '@storage/index'

interface Books extends BooksCollectionInfo {
  offlineBooks?: BookCollectionItem[]
}

export const useBooksCollection = () => {
  const [collection, setCollection] = useState<Books>()
  const [isOffline, setIsOffline] = useState(false)
  const { client } = useWebApiClient()
  const { handleFetchError } = useErrorHandler()

  const load = async () => {
    try {
      const res = await client.webApi.books.get()
      setCollection(res)

      if (res) {
        await ensureBooksStorage(res.activeBooks || [])
      }
    } catch (e) {
      const { isBackendUnavailable } = handleFetchError(e as Error)

      if (isBackendUnavailable) {
        const storedBooks = await getStoredBooks()
        setCollection({
          offlineBooks: storedBooks,
        })
        setIsOffline(true)
        return
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
    isOffline,
  }
}
