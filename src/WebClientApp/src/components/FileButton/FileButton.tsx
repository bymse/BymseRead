import styles from './FileButton.module.css'
import { Button } from '@components/Button/Button.tsx'
import { useState } from 'preact/hooks'

export type FileButtonProps = {
  fileUrl?: string
  fileName?: string
  inputName: string
}

type FormFile = {
  fileUrl?: string
  fileName?: string
}

export const FileButton = ({ fileUrl, fileName, inputName }: FileButtonProps) => {
  const [file, setFile] = useState<FormFile | null>(fileUrl ? { fileUrl, fileName } : null)

  const handleFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement
    const file = target.files?.[0]
    if (file) {
      setFile({ fileName: file.name })
    }
  }

  return (
    <div className={styles.fileWrapper}>
      {file && (
        <a href={file.fileUrl} download className={styles.fileName} target="_blank" rel="noreferrer">
          {file.fileName}
        </a>
      )}
      <Button type="label" title={file ? 'Change file' : 'Select file'} appearance="outline">
        <input
          type="file"
          name={inputName}
          style={{ display: 'none' }}
          onChange={handleFileChange}
          accept=".pdf"
          data-testid="file-button-input"
        />
      </Button>
    </div>
  )
}
