import styles from './BookmarksPanel.module.scss'
import { Button } from '@components/Button/Button.tsx'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'

export type BookmarksPanelProps = {
  onMarkAsLastPage: () => void
  lastPage?: number
  lastPageDate?: Date
  onClose: () => void
  onLastPageClick: (lastPage: number) => void
}

export const BookmarksPanel = ({
  onMarkAsLastPage,
  lastPageDate,
  lastPage,
  onClose,
  onLastPageClick,
}: BookmarksPanelProps) => {
  const date = lastPageDate ? lastPageDate.toISOString().split('T')[0].replace(/-/g, '.') : ''

  const handleLastPageClick = () => {
    if (lastPage) {
      onLastPageClick(lastPage)
    }
  }

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h2 className={styles.title}>Bookmarks</h2>
        <Button icon={RemoveIcon} onClick={onClose} appearance="flat" />
      </div>
      <Button appearance="primary" onClick={onMarkAsLastPage} title="Mark as last page" />
      {lastPage && (
        <div className={styles.card} onClick={handleLastPageClick}>
          <span className={styles.cardName}>Last page</span>
          <span className={styles.date}>{date}</span>
          <span className={styles.page}>{lastPage}</span>
        </div>
      )}
    </div>
  )
}
