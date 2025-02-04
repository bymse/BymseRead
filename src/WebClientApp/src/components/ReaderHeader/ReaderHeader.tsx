import styles from './ReaderHeader.module.scss'
import { Button } from '@components/Button/Button.tsx'
import { ArrowLeftIcon } from '@icons/ArrowLeftIcon.tsx'
import { Dropdown, DropdownItem } from '@components/Dropdown/Dropdown.tsx'
import { BookmarkIcon } from '@icons/BookmarkIcon.tsx'
import { PageInput } from '@components/PageInput/PageInput.tsx'

export type ReaderHeaderProps = {
  totalPages?: number
  currentPage?: number
  title: string
  onBookmarkClick: () => void
  onEditBook: () => void
  onDeleteBook: () => void
  onCurrentPageChange: (page: number) => void
}

export const ReaderHeader = ({
  title,
  onBookmarkClick,
  onEditBook,
  onDeleteBook,
  totalPages,
  currentPage,
  onCurrentPageChange,
}: ReaderHeaderProps) => {
  const handleCurrentPageChange = (value: number) => {
    onCurrentPageChange(value)
  }

  return (
    <header className={styles.header}>
      <div className={styles.left}>
        <Button href="/books" appearance="flat" icon={ArrowLeftIcon} />
        <span className={styles.title}>{title}</span>
      </div>
      <div className={styles.center}>
        <PageInput defaultValue={currentPage} onValueChange={handleCurrentPageChange} maxNumber={totalPages} />
        <span className={styles.totalPages}>/{totalPages}</span>
      </div>
      <div className={styles.right}>
        <Button icon={BookmarkIcon} appearance="flat" onClick={onBookmarkClick} />
        <Dropdown side="left">
          <DropdownItem onClick={onEditBook}>Edit book</DropdownItem>
          <DropdownItem onClick={onDeleteBook}>Delete</DropdownItem>
        </Dropdown>
      </div>
    </header>
  )
}
