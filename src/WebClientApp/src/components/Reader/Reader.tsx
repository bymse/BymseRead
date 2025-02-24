import { useEffect, useRef } from 'preact/hooks'
import * as pdfjsLib from 'pdfjs-dist'
import { PDFViewer, EventBus } from 'pdfjs-dist/web/pdf_viewer.mjs'
import styles from './Reader.module.scss'

pdfjsLib.GlobalWorkerOptions.workerSrc = new URL('pdfjs-dist/build/pdf.worker.min.mjs', import.meta.url).toString()

export type ReaderProps = {
  pdfUrl: string
  currentPage?: number
  onCurrentPageChange?: (page: number) => void
}

export const Reader = ({ pdfUrl, currentPage, onCurrentPageChange }: ReaderProps) => {
  const containerRef = useRef<HTMLDivElement>(null)
  const viewerRef = useRef<PDFViewer | null>(null)
  const lastPageRef = useRef<number | null>(null)

  useEffect(() => {
    if (!containerRef.current) return

    const eventBus = new EventBus()
    eventBus.on('pagechanging', ({ pageNumber }: { pageNumber: number }) => {
      if (pageNumber !== lastPageRef.current) {
        if (onCurrentPageChange) {
          onCurrentPageChange(pageNumber)
        }
        lastPageRef.current = pageNumber
      }
    })
    const pdfViewer = new PDFViewer({
      container: containerRef.current,
      eventBus,
      removePageBorders: true,
    })
    viewerRef.current = pdfViewer

    void pdfjsLib.getDocument(pdfUrl).promise.then(pdfDocument => {
      pdfViewer.setDocument(pdfDocument)
      if (currentPage) {
        pdfViewer.currentPageNumber = currentPage
      }
    })
  }, [pdfUrl])

  useEffect(() => {
    if (viewerRef.current && currentPage) {
      viewerRef.current.currentPageNumber = currentPage
    }
  }, [currentPage])

  return (
    <section className={styles.root}>
      <div className={styles.wrapper}>
        <div className={styles.pdfjsContainer} ref={containerRef}>
          <div id="pdf-viewer" className="pdfViewer"></div>
        </div>
      </div>
    </section>
  )
}
