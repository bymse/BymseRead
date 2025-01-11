import styles from './BookCard.module.scss'

export type BookCardProps = {
  title: string
  coverUrl?: string
  progressPercent?: number
}

export const BookCard = ({ title, coverUrl, progressPercent }: BookCardProps) => {
  return (
    <div style={{ ['--fill-percent']: `${progressPercent || 0}%` }} className={styles.card}>
      <img src={coverUrl} alt={title} />
      <div className={styles.title}>{title}</div>
    </div>
  )
}
