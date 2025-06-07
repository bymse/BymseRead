import styles from './BookCard.module.scss'
import {useState} from 'preact/hooks'

export type BookCardProps = {
  title: string
  coverUrl?: string
  percentageFinished?: number
  bookId: string
}

export const BookCard = ({title, coverUrl, percentageFinished, bookId}: BookCardProps) => {
  const [imageLoaded, setImageLoaded] = useState(false)

  return (
    <div className={styles.cardWrapper}>
      <a style={{['--fill-percent']: `${percentageFinished || 0}%`}} className={styles.card} href={`/books/${bookId}`}>
        {coverUrl && (
          <>
            <img
              className={styles.cover}
              src={coverUrl}
              alt={title}
              style={{display: imageLoaded ? 'block' : 'none'}}
              onLoad={() => setImageLoaded(true)}
            />
            {!imageLoaded && <DefaultBookCover title={title}/>}
          </>
        )}
        {!coverUrl && <DefaultBookCover title={title}/>}
        <div className={styles.title}>{title}</div>
      </a>
    </div>
  )
}

const DefaultBookCover = ({title}: { title: string }) => {
  return (
    <div className={styles.defaultCover}>
      <span className={styles.coverLetter}>{title[0]}</span>
    </div>
  )
}
