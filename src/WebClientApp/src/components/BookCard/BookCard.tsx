import styles from './BookCard.module.scss'
import { useState } from 'preact/hooks'
import type { CurrentPageInfo } from '@api/models'

export type BookCardProps = {
  title: string
  coverUrl?: string
  currentPage?: CurrentPageInfo | null
  lastBookmark?: { page?: number | null } | null
  pages?: number | null
  bookId: string
}

const calculatePercentageFinished = (
  currentPage: CurrentPageInfo | null | undefined,
  lastBookmark: { page?: number | null } | null | undefined,
  pages: number | null | undefined,
): number => {
  const lastPage = lastBookmark?.page ?? currentPage?.page ?? 0
  return lastPage > 0 && pages != null ? Math.round((lastPage / pages) * 100) : 0
}

export const BookCard = ({ title, coverUrl, currentPage, lastBookmark, pages, bookId }: BookCardProps) => {
  const [imageLoaded, setImageLoaded] = useState(false)
  const percentageFinished = calculatePercentageFinished(currentPage, lastBookmark, pages)

  return (
    <div className={styles.cardWrapper}>
      <a style={{ ['--fill-percent']: `${percentageFinished}%` }} className={styles.card} href={`/books/${bookId}`}>
        {coverUrl && (
          <>
            <img
              className={styles.cover}
              src={coverUrl}
              alt={title}
              style={{ display: imageLoaded ? 'block' : 'none' }}
              onLoad={() => setImageLoaded(true)}
            />
            {!imageLoaded && <DefaultBookCover title={title} />}
          </>
        )}
        {!coverUrl && <DefaultBookCover title={title} />}
        <div className={styles.title}>{title}</div>
      </a>
    </div>
  )
}

const DefaultBookCover = ({ title }: { title: string }) => {
  return (
    <div className={styles.defaultCover}>
      <span className={styles.coverLetter}>{title[0]}</span>
    </div>
  )
}
