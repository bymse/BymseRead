import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader.ts'

const maxRetries = 10

export const useBooksCollection = () => {
  const [collection, setCollection] = useState<BooksCollectionInfo>()
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  let retryCount = 0

  const load = async () => {
    try {
      const res = await client.webApi.books.get()
      setCollection(res)
      retryCount = 0
    } catch (e) {
      handleError(e as Error)
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
