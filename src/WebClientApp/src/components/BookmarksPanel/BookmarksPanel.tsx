import styles from './BookmarksPanel.module.scss'
import { Button } from '@components/Button/Button.tsx'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'
import { useRef, useState } from 'preact/hooks'
import { useEffect } from 'react'
import { Toast } from '@components/Toast/Toast.tsx'

export type BookmarksPanelProps = {
  onMarkAsLastPage: (page: number) => void
  lastPage?: number
  lastPageDate?: Date
  onClose: () => void
  onLastPageClick: (lastPage: number) => void
  currentPage: number
  onReturnToPageClick: (page: number) => void
}

export const BookmarksPanel = ({
  onMarkAsLastPage,
  lastPageDate,
  lastPage,
  onClose,
  onLastPageClick,
  currentPage,
  onReturnToPageClick,
}: BookmarksPanelProps) => {
  const [showReturnToPage, setShowReturnToPage] = useState(false)
  const pageToReturnRef = useRef<number | null>(null)

  useEffect(() => {
    if (currentPage === lastPage) {
      setShowReturnToPage(false)
    }
  }, [setShowReturnToPage, currentPage])

  const handleLastPageClick = () => {
    if (!lastPage) {
      return
    }

    onLastPageClick(lastPage)
    if (lastPage !== currentPage) {
      pageToReturnRef.current = currentPage
      setShowReturnToPage(true)
    }
  }

  const handleMarkAsLastPage = () => {
    setShowReturnToPage(false)
    pageToReturnRef.current = null
    onMarkAsLastPage(currentPage)
  }

  const handleReturnToPage = () => {
    onReturnToPageClick(pageToReturnRef.current as number)
    setShowReturnToPage(false)
  }

  const date = lastPageDate ? lastPageDate.toISOString().split('T')[0].replace(/-/g, '.') : ''
  return (
    <>
      <div className={styles.container}>
        <h2 className={styles.title}>Bookmarks</h2>
        <div className={styles.close}>
          <Button icon={RemoveIcon} onClick={onClose} appearance="flat" />
        </div>
        <div className={styles.button}>
          <Button appearance="primary" onClick={handleMarkAsLastPage} title="Mark as last page" />
        </div>
        {lastPage && (
          <div className={styles.card} onClick={handleLastPageClick}>
            <span className={styles.cardName}>Last page</span>
            <span className={styles.date}>{date}</span>
            <span className={styles.page}>{lastPage}</span>
          </div>
        )}
      </div>
      {showReturnToPage && (
        <div className={styles.returnToPageToast}>
          <Toast
            message={`Return to page ${pageToReturnRef.current}`}
            variant="info"
            position="relative"
            onClick={handleReturnToPage}
            onClose={() => setShowReturnToPage(false)}
          />
        </div>
      )}
    </>
  )
}
