import { PopupForm } from '@components/PopupForm/PopupForm.tsx'

type Props = {
  bookId: string
  onDelete: (bookId: string) => Promise<void>
  onClose: () => void
}

export const DeleteBookModal = ({ onDelete, bookId, onClose }: Props) => {
  const handleDelete = () => {
    void onDelete(bookId).then(() => onClose())
  }

  return (
    <PopupForm
      noClose
      title="Delete this book?"
      onCancel={onClose}
      onSubmit={handleDelete}
      submitTitle="Delete"
      data-testid="delete-book-modal"
    />
  )
}
