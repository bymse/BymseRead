import styles from './BooksBlock.module.scss'
import { BookCard } from '@components/BookCard/BookCard.tsx'

export type BooksBlockProps = {
  title: string
  books: BookInfo[]
}

export type BookInfo = {
  bookId: string
  title: string
  percentageFinished?: number
  coverUrl?: string
}

export const BooksBlock = ({ books, title }: BooksBlockProps) => {
  return (
    <section className={styles.block}>
      <h2>{title}</h2>
      <div className={styles.list}>
        {books.map(book => (
          <div key={book.bookId} className={styles.card}>
            <BookCard {...book} />
          </div>
        ))}
      </div>
    </section>
  )
}
