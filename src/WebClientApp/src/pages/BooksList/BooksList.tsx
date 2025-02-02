import { useEffect, useState } from 'preact/hooks'
import { BooksCollectionInfo, BookShortInfo } from '@api/models'
import { useWebApiClient } from '@hooks/useWebApiClient'
import { Header } from '@components/Header/Header'
import styles from './BooksList.module.scss'
import { BookInfo, BooksBlock } from '@components/BooksBlock/BooksBlock.tsx'

export const BooksList = () => {
  const [collection, setCollection] = useState<BooksCollectionInfo>()
  const client = useWebApiClient()

  useEffect(() => {
    void client.webApi.books.get().then(e => setCollection(e))
  }, [client])

  return (
    <div className={styles.container}>
      <Header />
      {collection && (
        <div className={styles.list}>
          {<Block title="Active" books={collection.activeBooks} />}
          {<Block title="New" books={collection.newBooks} />}
          {<Block title="TL;DR" books={collection.tlDrBooks} />}
          {<Block title="Archived" books={collection.archivedBooks} />}
        </div>
      )}
    </div>
  )
}

const Block = ({ title, books }: { title: string; books: BookShortInfo[] | undefined | null }) => {
  if (books && books.length > 0) {
    return <BooksBlock title={title} books={books as BookInfo[]} />
  }

  return null
}
