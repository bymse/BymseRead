import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useFileUpload } from '@hooks/useFileUpload.tsx'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { EditBookFormValues } from '@components/EditBookForm/EditBookForm.tsx'

export const useEditBook = (bookId: string | undefined | null, onEdited: () => void, onDeleted: () => void) => {
  const { client } = useWebApiClient()
  const { uploadFile } = useFileUpload()
  const { handleError } = useErrorHandler()

  const handleEditBook = async (form: EditBookFormValues) => {
    if (!bookId) {
      return
    }

    let fileUploadKey: string | undefined
    if (form.bookFile) {
      const uploadResult = await uploadFile(form.bookFile)
      if (!uploadResult) return

      fileUploadKey = uploadResult.fileUploadKey
    }

    let coverUploadKey: string | undefined
    if (form.coverFile) {
      const uploadResult = await uploadFile(form.coverFile)
      if (!uploadResult) return

      coverUploadKey = uploadResult.fileUploadKey
    }

    client.webApi.books
      .byBookId(bookId)
      .update.post({
        title: form.title,
        removeCover: form.removeCover,
        uploadedBookFileKey: fileUploadKey,
        uploadedCoverFileKey: coverUploadKey,
      })
      .then(onEdited)
      .catch(handleError)
  }

  const handleDeleteBook = () => {
    if (!bookId) {
      return Promise.resolve()
    }

    return client.webApi.books.byBookId(bookId).delete().then(onDeleted).catch(handleError)
  }

  const handleMarkAsLastPage = (page: number) => {
    if (!bookId) {
      return
    }

    client.webApi.books
      .byBookId(bookId)
      .bookmarks.lastPage.post({
        page,
      })
      .catch(handleError)
  }

  return {
    handleEditBook,
    handleDeleteBook,
    handleMarkAsLastPage,
  }
}
