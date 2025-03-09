import { useEffect, useRef } from 'preact/hooks'
import styles from './Reader.module.scss'
import { PdfReader } from '@components/Reader/PdfReader.ts'

export type ReaderProps = {
  pdfUrl: string
  bookId: string
  currentPage: number
  onCurrentPageChange?: (page: number) => void
}

export const Reader = ({ pdfUrl, currentPage, bookId, onCurrentPageChange }: ReaderProps) => {
  const containerRef = useRef<HTMLDivElement>(null)
  const pdfReader = useRef<PdfReader | null>(null)

  useEffect(() => {
    if (!containerRef.current) return

    if (pdfReader.current) {
      void pdfReader.current.close().then(() => (pdfReader.current = null))
    }

    pdfReader.current = new PdfReader(containerRef.current)
    void pdfReader.current.load({
      bookId,
      startPage: currentPage,
      pdfUrl,
      onInitialized: reader => {
        reader.page = currentPage || 1
      },
      onPageChange: page => {
        onCurrentPageChange?.(page)
      },
    })
  }, [pdfUrl])

  useEffect(() => {
    if (pdfReader.current && currentPage) {
      pdfReader.current.page = currentPage
    }
  }, [currentPage])

  return (
    <section className={styles.root}>
      <div id="viewerContainer" ref={containerRef} className={styles.viewerContainer}>
        <div id="viewer" className="pdfViewer"></div>
      </div>
    </section>
  )
}
