import { useEffect, useRef } from 'preact/hooks'
import styles from './Reader.module.scss'
import { PdfReader } from '@components/Reader/PdfReader.ts'
import { useExecuteWithLoader } from '@utils/useExecuteWithLoader'
import cn from 'classnames'
import { Loader } from '@components/Loader/Loader.tsx'
import { useErrorHandler } from '@hooks/useErrorHandler.ts'

export type ReaderProps = {
  pdfUrl: string
  bookId: string
  currentPage: number
  onCurrentPageChange?: (page: number) => void
}

export const Reader = ({ pdfUrl, currentPage, bookId, onCurrentPageChange }: ReaderProps) => {
  const containerRef = useRef<HTMLDivElement>(null)
  const pdfReader = useRef<PdfReader | null>(null)
  const currentPageRef = useRef<number>(currentPage)
  const { handleError } = useErrorHandler()

  const loadPdfReader = async () => {
    if (!containerRef.current) return

    if (pdfReader.current) {
      await pdfReader.current.close()
      pdfReader.current = null
    }

    pdfReader.current = new PdfReader(containerRef.current)
    return new Promise<void>((resolve, reject) => {
      void pdfReader.current!.load({
        bookId,
        pdfUrl,
        onInitialized: reader => {
          if (currentPageRef.current) {
            reader.page = currentPageRef.current
          }
          resolve()
        },
        onPageChange: page => {
          onCurrentPageChange?.(page)
        },
        onError: error => {
          handleError(error)
          reject(error)
        },
      })
    })
  }

  const { isLoading, showSpinner, execute } = useExecuteWithLoader(loadPdfReader, true)

  useEffect(() => {
    void execute()
  }, [pdfUrl])

  useEffect(() => {
    currentPageRef.current = currentPage
    if (pdfReader.current && currentPage && pdfReader.current.lastPage !== currentPage) {
      pdfReader.current.page = currentPage
    }
  }, [currentPage])

  return (
    <section className={styles.root}>
      <Loader showSpinner={showSpinner} text={<>We&lsquo;re loading the reader</>} />
      <div
        id="viewerContainer"
        ref={containerRef}
        className={cn({
          [styles.viewerContainer]: true,
          [styles.loading]: isLoading,
        })}
      >
        <div id="viewer" className="pdfViewer"></div>
      </div>
    </section>
  )
}
