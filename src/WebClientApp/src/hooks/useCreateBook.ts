import { BookFormValues } from '@components/BookForm/BookForm.tsx'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'

export const useCreateBook = (onCreated: (bookId: string) => void) => {
  const client = useWebApiClient()

  const handleCreateBook = async (form: BookFormValues) => {
    const response = await client.webApi.files.prepareUpload.put({
      fileName: form.bookFile.name,
      fileSize: form.bookFile.size,
    })

    await fetch(response?.uploadUrl as string, {
      method: 'PUT',
      body: form.bookFile,
      headers: {
        'x-amz-meta-originalFileName': form.bookFile.name,
        'Content-Type': 'application/octet-stream',
      },
    })

    const createResponse = await client.webApi.books.post({
      title: form.title,
      fileUploadKey: response?.fileUploadKey as string,
    })

    onCreated(createResponse?.bookId as string)
  }

  return {
    handleCreateBook,
  }
}
