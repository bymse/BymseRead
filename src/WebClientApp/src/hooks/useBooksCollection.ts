import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader.ts'

export const useBooksCollection = () => {
  const [collection, setCollection] = useState<BooksCollectionInfo>()
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  const load = async () => {
    try {
      const res = await client.webApi.books.get()
      return setCollection(res)
    } catch (e) {
      handleError(e as Error)
      setTimeout(() => void load(), Math.random() * 4000 + 3000)
    }
  }

  const { isLoading, showSpinner, execute } = useExecuteWithLoader(load)

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
