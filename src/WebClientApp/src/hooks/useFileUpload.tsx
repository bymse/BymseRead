import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'

type UploadResult = {
  fileUploadKey: string
}

export const useFileUpload = () => {
  const { client } = useWebApiClient()
  const { handleError } = useErrorHandler()

  const uploadFile = async (file?: File): Promise<UploadResult | undefined> => {
    if (!file) {
      handleError('File is required')
      return
    }

    const prepareResponse = await client.webApi.files.prepareUpload
      .put({
        fileName: file.name,
        fileSize: file.size,
      })
      .catch(handleError)

    if (!prepareResponse) return

    const response = await fetch(prepareResponse?.uploadUrl as string, {
      method: 'PUT',
      body: file,
      headers: {
        'x-amz-meta-originalFileName': file.name,
        'Content-Type': 'application/octet-stream',
      },
    }).catch(handleError)

    return response ? { fileUploadKey: prepareResponse?.fileUploadKey as string } : undefined
  }

  return {
    uploadFile,
  }
}
