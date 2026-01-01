import { BookCollectionItem } from '@api/models'
import { Header } from '@components/Header/Header'
import styles from './BooksList.module.scss'
import { BooksBlock } from '@components/BooksBlock/BooksBlock.tsx'
import { useShowHide } from '@hooks/useShowHide.ts'
import { AddBookForm } from '@components/AddBookForm/AddBookForm.tsx'
import { useBooksCollection } from '@hooks/useBooksCollection.ts'
import { useCreateBook } from '@hooks/useCreateBook.ts'
import { useToast } from '@components/Toast/ToastContext.tsx'
import noBooksIllustration from '@assets/no-books.svg?inline'
import { Loader } from '@components/Loader/Loader.tsx'
import { usePageTitle } from '@hooks/usePageTitle.ts'

export const BooksList = () => {
  const { showInfo } = useToast()
  const { open: openBookForm, close: closeBookFrom, visible: bookFromVisible } = useShowHide()
  const { collection, reload, isLoading, showSpinner, isOffline } = useBooksCollection()

  usePageTitle('Books')

  const onBookCreated = (bookId: string) => {
    void reload()
    closeBookFrom()
    showInfo('New book was added', `/books/${bookId}`, 7000)
  }

  const { handleCreateBook } = useCreateBook(onBookCreated)

  const isEmpty =
    collection &&
    collection.tlDrBooks?.length === 0 &&
    collection.newBooks?.length === 0 &&
    collection.activeBooks?.length === 0 &&
    collection.archivedBooks?.length === 0

  if (isLoading || !collection) {
    return (
      <div className={styles.container}>
        <Header />
        <Loader showSpinner={showSpinner} text={<>We&lsquo;re loading your books</>} />
      </div>
    )
  }

  return (
    <div className={styles.container}>
      <Header onAddBook={isOffline ? undefined : openBookForm} />
      {isEmpty ? (
        <div className={styles.noBooks}>
          <img src={noBooksIllustration} alt="No books" />
          <span>We haven’t found anything</span>
        </div>
      ) : (
        <div className={styles.list}>
          {<Block title="Active" books={collection.activeBooks} />}
          {<Block title="New" books={collection.newBooks} />}
          {<Block title="TL;DR" books={collection.tlDrBooks} />}
          {<Block title="Archived" books={collection.archivedBooks} />}
          {<Block title="Offline" books={collection.offlineBooks} />}
        </div>
      )}
      {bookFromVisible && <AddBookForm onSubmit={handleCreateBook} onCancel={closeBookFrom} />}
    </div>
  )
}

const Block = ({ title, books }: { title: string; books: BookCollectionItem[] | undefined | null }) => {
  if (books && books.length > 0) {
    return <BooksBlock title={title} books={books} />
  }

  return null
}
