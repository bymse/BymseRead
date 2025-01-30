import { PopupForm } from '@components/PopupForm/PopupForm.tsx'
import { Input } from '@components/Input/Input.tsx'
import { Button } from '@components/Button/Button.tsx'
import { useState } from 'preact/hooks'
import styles from './BookForm.module.css'

export type BookFormProps = {
  onSubmit: (title: string, bookFile: File) => void
  onCancel: () => void
}

export const BookForm = ({ onSubmit, onCancel }: BookFormProps) => {
  const [file, setFile] = useState<string | null>(null)

  const handleSubmit = (form: FormData) => {
    const title = form.get('title') as string
    const bookFile = form.get('bookFile') as File
    onSubmit(title, bookFile)
  }

  const handleFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement
    const file = target.files?.[0]
    if (file) {
      setFile(file?.name)
    }
  }

  return (
    <PopupForm title="Add new book" onCancel={onCancel} onSubmit={handleSubmit} submitTitle="Add book">
      <div className={styles.fileWrapper}>
        {file && <span className={styles.fileName}>{file}</span>}
        <Button type="label" title={file ? 'Change file' : 'Select file'} appearance="outline">
          <input type="file" name="bookFile" style={{ display: 'none' }} onChange={handleFileChange} accept=".pdf" />
        </Button>
      </div>
      <Input type="text" name="title" label="Title" />
    </PopupForm>
  )
}
