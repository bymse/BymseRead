import { PopupForm } from '@components/PopupForm/PopupForm.tsx'
import { Input } from '@components/Input/Input.tsx'
import { FileButton } from '@components/FileButton/FileButton.tsx'
import { CoverFileButton } from '@components/CoverFileButton/CoverFileButton.tsx'
import { getFormFile } from '@utils/getFormFile.ts'

export type EditBookFormProps = {
  title: string
  bookId: string
  fileUrl: string
  fileName: string
  coverUrl?: string
  onSubmit: (form: EditBookFormValues) => Promise<void> | void
  onCancel: () => void
}

export type EditBookFormValues = {
  title: string
  bookFile?: File
  coverFile?: File
  removeCover: boolean
  bookId: string
}

export const EditBookForm = ({ title, bookId, fileUrl, fileName, coverUrl, onSubmit, onCancel }: EditBookFormProps) => {
  const handleSubmit = async (form: FormData) => {
    const title = form.get('title') as string
    const bookFile = getFormFile(form, 'bookFile')
    const coverFile = getFormFile(form, 'coverFile')
    const removeCover = form.get('removeCover') === 'on'

    await onSubmit({ title, bookFile, coverFile, removeCover, bookId })
  }

  return (
    <PopupForm title="Edit book" onCancel={onCancel} onSubmit={handleSubmit} submitTitle="Save changes">
      <FileButton inputName="bookFile" fileName={fileName} fileUrl={fileUrl} />
      <Input type="text" name="title" label="Title" defaultValue={title} />
      <div style={{ marginTop: '12px' }}>
        <CoverFileButton inputName="coverFile" removedCheckName="removeCover" coverUrl={coverUrl} />
      </div>
    </PopupForm>
  )
}
