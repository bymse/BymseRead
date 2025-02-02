import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'

export const useBooksCollection = () => {
  const [collection, setCollection] = useState<BooksCollectionInfo>()
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  const load = () => {
    void client.webApi.books
      .get()
      .then(e => setCollection(e))
      .catch(e => {
        handleError(e as Error)
        setTimeout(() => load(), Math.random() * (7000 - 3000) + 3000)
      })
  }

  useEffect(() => {
    load()
  }, [client])

  return {
    collection,
    reload: load,
  }
}
