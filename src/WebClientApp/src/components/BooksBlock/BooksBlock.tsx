import styles from './BooksBlock.module.scss'
import { BookCard } from '@components/BookCard/BookCard.tsx'
import type { BookCollectionItem } from '@api/models'

export type BooksBlockProps = {
  title: string
  books: BookCollectionItem[]
}

export const BooksBlock = ({ books, title }: BooksBlockProps) => {
  return (
    <section className={styles.block} data-testid={`books-block-${title.toLowerCase()}`}>
      <h2>{title}</h2>
      <div className={styles.list}>
        {books.map(book => (
          <div key={book.bookId} className={styles.card}>
            <BookCard
              bookId={book.bookId!}
              title={book.title!}
              coverUrl={book.coverUrl ?? undefined}
              currentPage={book.currentPage}
              lastBookmark={book.lastBookmark}
              pages={book.pages}
            />
          </div>
        ))}
      </div>
    </section>
  )
}
