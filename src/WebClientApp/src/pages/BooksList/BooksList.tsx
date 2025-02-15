import { BookShortInfo } from '@api/models'
import { Header } from '@components/Header/Header'
import styles from './BooksList.module.scss'
import { BookInfo, BooksBlock } from '@components/BooksBlock/BooksBlock.tsx'
import { useShowHide } from '@hooks/useShowHide.ts'
import { AddBookForm } from '@components/AddBookForm/AddBookForm.tsx'
import { useBooksCollection } from '@hooks/useBooksCollection.ts'
import { useCreateBook } from '@hooks/useCreateBook.ts'
import { useToast } from '@components/Toast/ToastContext.tsx'
import { Spinner } from '@components/Spinner/Spinner.tsx'

export const BooksList = () => {
  const { showInfo } = useToast()
  const { open: openBookForm, close: closeBookFrom, visible: bookFromVisible } = useShowHide()
  const { collection, reload } = useBooksCollection()

  const onBookCreated = (bookId: string) => {
    void reload()
    closeBookFrom()
    showInfo('New book was added', `/books/${bookId}`, 7000)
  }

  const { handleCreateBook } = useCreateBook(onBookCreated)

  return (
    <div className={styles.container}>
      <Header onAddBook={openBookForm} />
      {collection ? (
        <div className={styles.list}>
          {<Block title="Active" books={collection.activeBooks} />}
          {<Block title="New" books={collection.newBooks} />}
          {<Block title="TL;DR" books={collection.tlDrBooks} />}
          {<Block title="Archived" books={collection.archivedBooks} />}
        </div>
      ) : (
        <div className={styles.loader}>
          <Spinner color="var(--color-base-primary-normal)" />
          <span>We&lsquo;re loading books</span>
        </div>
      )}
      {bookFromVisible && <AddBookForm onSubmit={handleCreateBook} onCancel={closeBookFrom} />}
    </div>
  )
}

const Block = ({ title, books }: { title: string; books: BookShortInfo[] | undefined | null }) => {
  if (books && books.length > 0) {
    return <BooksBlock title={title} books={books as BookInfo[]} />
  }

  return null
}
