import { useLocation, useRoute } from 'preact-iso'
import { ReaderHeader } from '@components/ReaderHeader/ReaderHeader.tsx'
import { NotFound } from '../NotFound/NotFound.tsx'
import { useBook } from '@hooks/useBook.ts'
import { Spinner } from '@components/Spinner/Spinner.tsx'
import { useShowHide } from '@hooks/useShowHide.ts'
import { BookmarksPanel } from '@components/BookmarksPanel/BookmarksPanel.tsx'
import { EditBookForm } from '@components/EditBookForm/EditBookForm.tsx'
import { useEditBook } from '@hooks/useEditBook.ts'
import { DeleteBookModal } from '@components/DeleteBookModal.tsx'
import { useCurrentPage } from '@hooks/useCurrentPage.ts'
import styles from './Book.module.scss'
import { Reader } from '@components/Reader/Reader.tsx'

export const Book = () => {
  const { params } = useRoute()
  const { route } = useLocation()
  const { id } = params
  const { book, isLoading, reload } = useBook(id)
  const { updateCurrentPage, currentPage } = useCurrentPage(book)
  const { handleEditBook, handleDeleteBook, handleMarkAsLastPage } = useEditBook(book?.bookId, reload, () =>
    route('/books'),
  )

  const bookmarksShowHide = useShowHide()
  const editShowHide = useShowHide()
  const deleteShowHide = useShowHide()

  if (!id || (!isLoading && !book)) {
    return <NotFound />
  }

  if (isLoading || !book) {
    return (
      <div>
        <div className={styles.loader}>
          <Spinner color="var(--color-base-primary-normal)" />
          <span>We&lsquo;re loading the book</span>
        </div>
      </div>
    )
  }

  return (
    <div>
      <ReaderHeader
        title={book.title as string}
        onBookmarkClick={bookmarksShowHide.open}
        onEditBook={editShowHide.open}
        onDeleteBook={deleteShowHide.open}
        onCurrentPageChange={updateCurrentPage}
        currentPage={currentPage}
        totalPages={book.pages as number}
      />

      <Reader
        pdfUrl={book.bookFile!.fileUrl as string}
        bookId={book.bookId as string}
        currentPage={currentPage}
        onCurrentPageChange={updateCurrentPage}
      />

      {bookmarksShowHide.visible && (
        <BookmarksPanel
          onClose={bookmarksShowHide.close}
          lastPage={book.lastBookmark?.page}
          lastPageDate={book?.lastBookmark?.createdAt}
          onLastPageClick={updateCurrentPage}
          currentPage={currentPage}
          onMarkAsLastPage={handleMarkAsLastPage}
          onReturnToPageClick={updateCurrentPage}
        />
      )}

      {editShowHide.visible && (
        <EditBookForm
          title={book.title as string}
          bookId={book.bookId as string}
          fileUrl={book.bookFile!.fileUrl as string}
          fileName={book.bookFile?.name as string}
          onSubmit={handleEditBook}
          onCancel={editShowHide.close}
        />
      )}

      {deleteShowHide.visible && (
        <DeleteBookModal bookId={book.bookId as string} onDelete={handleDeleteBook} onClose={deleteShowHide.close} />
      )}
    </div>
  )
}
