import { useWebApiClient } from '@hooks/useWebApiClient.ts'

type UploadResult = {
  fileUploadKey: string
}

export const useFileUpload = () => {
  const { client, onError } = useWebApiClient()

  const uploadFile = async (file: File): Promise<UploadResult | undefined> => {
    const prepareResponse = await client.webApi.files.prepareUpload
      .put({
        fileName: file.name,
        fileSize: file.size,
      })
      .catch(onError)

    if (!prepareResponse) return

    const response = await fetch(prepareResponse?.uploadUrl as string, {
      method: 'PUT',
      body: file,
      headers: {
        'x-amz-meta-originalFileName': file.name,
        'Content-Type': 'application/octet-stream',
      },
    }).catch(onError)

    return response ? { fileUploadKey: prepareResponse?.fileUploadKey as string } : undefined
  }

  return {
    uploadFile,
  }
}
