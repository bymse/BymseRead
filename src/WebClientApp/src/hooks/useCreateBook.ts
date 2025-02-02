import { BookFormValues } from '@components/BookForm/BookForm.tsx'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useFileUpload } from '@hooks/useFileUpload.tsx'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'

export const useCreateBook = (onCreated: (bookId: string) => void) => {
  const { client } = useWebApiClient()
  const { uploadFile } = useFileUpload()
  const { handleError } = useErrorHandler()

  const handleCreateBook = async (form: BookFormValues) => {
    const uploadResponse = await uploadFile(form.bookFile)
    if (!uploadResponse) return

    const createResponse = await client.webApi.books
      .post({
        title: form.title,
        fileUploadKey: uploadResponse.fileUploadKey,
      })
      .catch(handleError)

    if (createResponse) {
      onCreated(createResponse?.bookId as string)
    }
  }

  return {
    handleCreateBook,
  }
}
