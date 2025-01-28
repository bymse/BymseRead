import styles from './BookCard.module.scss'

export type BookCardProps = {
  title: string
  coverUrl?: string
  percentageFinished?: number
}

export const BookCard = ({ title, coverUrl, percentageFinished }: BookCardProps) => {
  return (
    <div style={{ ['--fill-percent']: `${percentageFinished || 0}%` }} className={styles.card}>
      {coverUrl && <img className={styles.cover} src={coverUrl} alt={title} />}
      {!coverUrl && <DefaultBookCover title={title} />}
      <div className={styles.title}>{title}</div>
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
