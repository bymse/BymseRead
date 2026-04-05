import { Button } from '@components/Button/Button.tsx'
import { Dropdown, DropdownItem } from '@components/Dropdown/Dropdown.tsx'
import { PageInput } from '@components/PageInput/PageInput.tsx'
import { ArrowLeftIcon } from '@icons/ArrowLeftIcon.tsx'
import { BookmarkIcon } from '@icons/BookmarkIcon.tsx'
import { BookmarkSolidIcon } from '@icons/BookmarkSolidIcon.tsx'
import { ContentsIcon } from '@icons/ContentsIcon.tsx'
import styles from './ReaderHeader.module.scss'

export type ReaderHeaderProps = {
  totalPages?: number
  currentPage?: number
  lastPageBookmark?: number
  title?: string
  onBookmarkClick?: () => void
  onContentsClick?: () => void
  showContentsButton?: boolean
  isContentsOpen?: boolean
  onResetZoom?: () => void
  onEditBook?: () => void
  onDeleteBook?: () => void
  onCurrentPageChange?: (page: number) => void
  'data-testid'?: string
}

export const ReaderHeader = ({
  title,
  onBookmarkClick,
  onContentsClick,
  showContentsButton,
  isContentsOpen,
  onEditBook,
  onDeleteBook,
  onResetZoom,
  totalPages,
  currentPage,
  lastPageBookmark,
  onCurrentPageChange,
  'data-testid': dataTestId,
}: ReaderHeaderProps) => {
  const handleCurrentPageChange = (value: number) => {
    if (onCurrentPageChange) {
      onCurrentPageChange(value)
    }
  }

  const hasDropdown = Boolean(onEditBook || onDeleteBook || onResetZoom)

  return (
    <header className={styles.header} data-testid={dataTestId}>
      <div className={styles.left}>
        <Button href="/books" appearance="flat" icon={ArrowLeftIcon} />
        <span data-testid="reader-header-title" className={styles.title}>
          {title}
        </span>
      </div>
      <div className={styles.center}>
        {totalPages && (
          <>
            <PageInput defaultValue={currentPage} onValueChange={handleCurrentPageChange} maxNumber={totalPages} />
            <span className={styles.totalPages}>/{totalPages}</span>
          </>
        )}
      </div>
      <div className={styles.right}>
        {showContentsButton && onContentsClick && (
          <Button
            icon={ContentsIcon}
            appearance={isContentsOpen ? 'secondary' : 'flat'}
            onClick={onContentsClick}
            data-testid="reader-header-contents-button"
          />
        )}
        {onBookmarkClick && (
          <Button
            icon={currentPage === lastPageBookmark ? BookmarkSolidIcon : BookmarkIcon}
            appearance="flat"
            onClick={onBookmarkClick}
            data-testid="reader-header-bookmark-button"
          />
        )}
        {hasDropdown && (
          <Dropdown side="left" data-testid="reader-header-dropdown-button">
            <DropdownItem onClick={onResetZoom}>Reset zoom</DropdownItem>
            {onEditBook && <DropdownItem onClick={onEditBook}>Edit book</DropdownItem>}
            {onDeleteBook && (
              <DropdownItem onClick={onDeleteBook} data-testid="reader-header-delete-button">
                Delete
              </DropdownItem>
            )}
          </Dropdown>
        )}
      </div>
    </header>
  )
}
