import styles from './BookCard.module.scss'

export type BookCardProps = {
  title: string
  coverUrl?: string
  percentageFinished?: number
  bookId: string
}

export const BookCard = ({ title, coverUrl, percentageFinished, bookId }: BookCardProps) => {
  return (
    <a style={{ ['--fill-percent']: `${percentageFinished || 0}%` }} className={styles.card} href={`/books/${bookId}`}>
      {coverUrl && <img className={styles.cover} src={coverUrl} alt={title} />}
      {!coverUrl && <DefaultBookCover title={title} />}
      <div className={styles.title}>{title}</div>
    </a>
  )
}

const DefaultBookCover = ({ title }: { title: string }) => {
  return (
    <div className={styles.defaultCover}>
      <span className={styles.coverLetter}>{title[0]}</span>
    </div>
  )
}
