import { PopupForm } from '@components/PopupForm/PopupForm.tsx'
import { Input } from '@components/Input/Input.tsx'
import { Button } from '@components/Button/Button.tsx'

export type BookFormProps = {
  onSubmit: (title: string, bookFile: File) => void
  onCancel: () => void
}

export const BookForm = ({ onSubmit, onCancel }: BookFormProps) => {
  const handleSubmit = (form: FormData) => {
    const title = form.get('title') as string
    const bookFile = form.get('book') as File
    onSubmit(title, bookFile)
  }

  return (
    <PopupForm title="Add new book" onCancel={onCancel} onSubmit={handleSubmit} submitTitle="Add book">
      <Button type="label" title="Select file" appearance="outline">
        <input type="file" name="bookFile" style={{ display: 'none' }} />
      </Button>
      <Input type="text" name="title" label="Title" />
    </PopupForm>
  )
}
