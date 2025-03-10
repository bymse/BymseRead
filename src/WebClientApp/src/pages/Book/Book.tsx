import { useLocation, useRoute } from 'preact-iso'
import { ReaderHeader } from '@components/ReaderHeader/ReaderHeader.tsx'
import { NotFound } from '../NotFound/NotFound.tsx'
import { useBook } from '@hooks/useBook.ts'
import { useShowHide } from '@hooks/useShowHide.ts'
import { BookmarksPanel } from '@components/BookmarksPanel/BookmarksPanel.tsx'
import { EditBookForm, EditBookFormValues } from '@components/EditBookForm/EditBookForm.tsx'
import { useEditBook } from '@hooks/useEditBook.ts'
import { DeleteBookModal } from '@components/DeleteBookModal/DeleteBookModal.tsx'
import { useCurrentPage } from '@hooks/useCurrentPage.ts'
import styles from './Book.module.scss'
import { Reader } from '@components/Reader/Reader.tsx'
import { useBookmarks } from '@hooks/useBookmarks.ts'
import { Loader } from '@components/Loader/Loader.tsx'

export const Book = () => {
  const { params } = useRoute()
  const { route } = useLocation()
  const { id } = params
  const { book, isLoading, reload, showSpinner } = useBook(id)
  const { updateCurrentPage, currentPage } = useCurrentPage(book)
  const { handleMarkAsLastPage, lastPageBookmark } = useBookmarks(book)
  const { handleEditBook, handleDeleteBook } = useEditBook(book?.bookId, reload, () => route('/books'))

  const bookmarksShowHide = useShowHide()
  const editShowHide = useShowHide()
  const deleteShowHide = useShowHide()

  const handleEditBookFormSubmit = async (form: EditBookFormValues) => {
    await handleEditBook(form)
    return editShowHide.close()
  }

  if (!id || (!isLoading && !book)) {
    return <NotFound />
  }

  if (isLoading || !book) {
    return (
      <div className={styles.container}>
        <ReaderHeader />
        <Loader showSpinner={showSpinner} text={<>We&lsquo;re loading the book</>} />
      </div>
    )
  }

  return (
    <div className={styles.container}>
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
          lastPage={lastPageBookmark?.page}
          lastPageDate={lastPageBookmark?.date}
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
          onSubmit={handleEditBookFormSubmit}
          onCancel={editShowHide.close}
          coverUrl={book.coverUrl}
        />
      )}

      {deleteShowHide.visible && (
        <DeleteBookModal bookId={book.bookId as string} onDelete={handleDeleteBook} onClose={deleteShowHide.close} />
      )}
    </div>
  )
}
