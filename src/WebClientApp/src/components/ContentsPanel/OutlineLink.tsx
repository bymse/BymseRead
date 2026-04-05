import styles from './ContentsPanel.module.scss'
import { ReaderOutlineItem } from '@components/Reader/readerOutline.ts'

type OutlineLinkProps = {
  item: ReaderOutlineItem
  onNavigate: (item: ReaderOutlineItem) => void
}

export const OutlineLink = ({ item, onNavigate }: OutlineLinkProps) => {
  if (item.pageNumber !== null) {
    return (
      <button
        type="button"
        className={styles.linkArea}
        data-testid="contents-outline-link"
        data-outline-id={item.id}
        onClick={() => onNavigate(item)}
      >
        <span className={styles.rowTitle}>{item.title}</span>
        <span className={styles.dots}></span>
        <span className={styles.pageNumber}>{item.pageNumber}</span>
      </button>
    )
  }

  if (item.isExternal && item.url !== null) {
    return (
      <a
        href={item.url}
        target="_blank"
        rel="noopener noreferrer"
        className={styles.linkArea}
        data-testid="contents-outline-link"
        data-outline-id={item.id}
        onClick={() => onNavigate(item)}
      >
        <span className={styles.rowTitle}>{item.title}</span>
      </a>
    )
  }

  return (
    <div className={styles.linkArea}>
      <span className={styles.rowTitle}>{item.title}</span>
    </div>
  )
}
