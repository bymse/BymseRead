import { useToast } from '@components/Toast/ToastContext.tsx'
import { useWebApiClient } from '@hooks/useWebApiClient.ts'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'
import { useRef } from 'preact/hooks'

type UploadResult = {
  fileUploadKey: string
}

export const useFileUpload = () => {
  const { client } = useWebApiClient()
  const { handleFetchError } = useErrorHandler()
  const { showError } = useToast()
  const uploadedFiles = useRef(new Map<File, string>())

  const uploadFile = async (file?: File): Promise<UploadResult | undefined> => {
    if (!file) {
      showError('File is required', undefined, 5000)
      return
    }

    if (uploadedFiles.current.has(file)) {
      return { fileUploadKey: uploadedFiles.current.get(file) as string }
    }

    const prepareResponse = await client.webApi.files.prepareUpload
      .put({
        fileName: file.name,
        fileSize: file.size,
      })
      .catch(e => {
        handleFetchError(e as Error)
        return
      })

    if (!prepareResponse) return

    const response = await fetch(prepareResponse?.uploadUrl as string, {
      method: 'PUT',
      body: file,
      headers: {
        'x-amz-meta-originalFileName': prepareResponse.encodedFileName || file.name,
        'Content-Type': 'application/octet-stream',
      },
    }).catch(e => {
      handleFetchError(e as Error)
      return
    })

    if (response) {
      uploadedFiles.current.set(file, prepareResponse?.fileUploadKey as string)
      return { fileUploadKey: prepareResponse?.fileUploadKey as string }
    }
  }

  return {
    uploadFile,
  }
}
