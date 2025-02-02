import { PopupForm } from '@components/PopupForm/PopupForm.tsx'
import { Input } from '@components/Input/Input.tsx'
import { Button } from '@components/Button/Button.tsx'
import { useState } from 'preact/hooks'
import styles from './BookForm.module.css'

export type BookFormProps = {
  onSubmit: (form: BookFormValues) => Promise<void> | void
  onCancel: () => void
}

export type BookFormValues = {
  title: string
  bookFile: File
}

export const BookForm = ({ onSubmit, onCancel }: BookFormProps) => {
  const [file, setFile] = useState<string | null>(null)

  const handleSubmit = (form: FormData) => {
    const title = form.get('title') as string
    const bookFile = form.get('bookFile') as File
    void onSubmit({ title, bookFile })
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
