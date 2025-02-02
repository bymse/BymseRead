﻿import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'

export const useBooksCollection = () => {
  const [collection, setCollection] = useState<BooksCollectionInfo>()
  const client = useWebApiClient()

  const load = () => {
    void client.webApi.books.get().then(e => setCollection(e))
  }

  useEffect(() => {
    load()
  }, [client])

  return {
    collection,
    reload: load,
  }
}
