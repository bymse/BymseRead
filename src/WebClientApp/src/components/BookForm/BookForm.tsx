﻿import { PopupForm } from '@components/PopupForm/PopupForm.tsx'
import { Input } from '@components/Input/Input.tsx'
import { FileButton } from '@components/FileButton/FileButton.tsx'

export type BookFormProps = {
  onSubmit: (form: BookFormValues) => Promise<void> | void
  onCancel: () => void
}

export type BookFormValues = {
  title?: string
  bookFile?: File
}

export const BookForm = ({ onSubmit, onCancel }: BookFormProps) => {
  const handleSubmit = async (form: FormData) => {
    const title = form.get('title') as string
    const bookFileRaw = form.get('bookFile') as File
    const bookFile = bookFileRaw.name ? bookFileRaw : undefined
    await onSubmit({ title, bookFile })
  }

  return (
    <PopupForm title="Add new book" onCancel={onCancel} onSubmit={handleSubmit} submitTitle="Add book">
      <FileButton inputName="bookFile" />
      <Input type="text" name="title" label="Title" />
    </PopupForm>
  )
}
