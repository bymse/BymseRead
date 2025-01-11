import styles from './BookCard.module.scss'

export type BookCardProps = {
  title: string
  coverUrl?: string
  percentageFinished?: number
}

export const BookCard = ({ title, coverUrl, percentageFinished }: BookCardProps) => {
  return (
    <div style={{ ['--fill-percent']: `${percentageFinished || 0}%` }} className={styles.card}>
      <img src={coverUrl} alt={title} />
      <div className={styles.title}>{title}</div>
    </div>
  )
}
