import * as pdfjsLib from 'pdfjs-dist'
import { PDFViewer, EventBus, PDFLinkService } from 'pdfjs-dist/web/pdf_viewer.mjs'
import 'pdfjs-dist/web/pdf_viewer.css'

pdfjsLib.GlobalWorkerOptions.workerSrc = new URL('pdfjs-dist/build/pdf.worker.min.mjs', import.meta.url).toString()

const DEFAULT_SCALE_DELTA = 1.05
const MIN_SCALE = 0.25
const MAX_SCALE = 10.0
const DEFAULT_SCALE_VALUE = 'auto'
//const TEXT_LAYER_MODE = 0 // DISABLE

const CMAP_URL = '/cmaps/'
const CMAP_PACKED = true
const STANDARD_FONTS_URL = '/standard_fonts/'

type LoadParams = {
  pdfUrl: string
  onInitialized?: (p: PdfReader) => void
  bookId: string
  onPageChange?: (page: number) => void
  onError: (error: Error) => void
}

export class PdfReader {
  private readonly eventBus: EventBus
  private readonly pdfViewer: PDFViewer
  private readonly pdfLinkService: PDFLinkService
  private pdfDocument?: pdfjsLib.PDFDocumentProxy
  private loadingTask?: pdfjsLib.PDFDocumentLoadingTask
  private loadParams?: LoadParams
  public lastPage?: number

  constructor(container: HTMLDivElement) {
    this.eventBus = new EventBus()
    this.pdfLinkService = new PDFLinkService({ eventBus: this.eventBus })

    this.pdfViewer = new PDFViewer({
      container,
      eventBus: this.eventBus,
      linkService: this.pdfLinkService,
      removePageBorders: true,
      //textLayerMode: TEXT_LAYER_MODE,
    })

    this.pdfLinkService.setViewer(this.pdfViewer)

    this.eventBus.on('pagesinit', () => {
      if (!this.pdfViewer) return

      this.pdfViewer.currentScaleValue = this.getSavedScale() || DEFAULT_SCALE_VALUE
      if (this.loadParams?.onInitialized) {
        this.loadParams.onInitialized(this)
      }
    })

    this.eventBus.on(
      'pagechanging',
      (evt: { pageNumber: number }) => {
        const page = evt.pageNumber
        if (this.lastPage !== page) {
          this.lastPage = page
          this.loadParams?.onPageChange?.(page)
        }
      },
      true,
    )
  }

  public async load(params: LoadParams) {
    if (this.loadingTask) {
      await this.close()
      await this.load(params)
      return
    }

    this.loadingTask = pdfjsLib.getDocument({
      url: params.pdfUrl,
      cMapUrl: CMAP_URL,
      cMapPacked: CMAP_PACKED,
      standardFontDataUrl: STANDARD_FONTS_URL,
    })

    try {
      this.pdfDocument = await this.loadingTask.promise

      this.pdfLinkService.setDocument(this.pdfDocument)
      this.pdfViewer.setDocument(this.pdfDocument)
      this.loadParams = params
    } catch (error) {
      params.onError(error as Error)
    }
  }

  public close() {
    if (!this.loadingTask) {
      return Promise.resolve()
    }

    const promise = this.loadingTask.destroy()
    this.loadingTask = undefined

    if (this.pdfDocument) {
      this.pdfDocument = undefined

      this.pdfViewer.cleanup()
      this.pdfLinkService.setDocument(null, null)
    }

    return promise
  }

  public get page() {
    return this.pdfViewer.currentPageNumber
  }

  public set page(val) {
    this.lastPage = val
    this.pdfViewer.currentPageNumber = val
    this.pdfViewer.scrollPageIntoView({ pageNumber: val })
  }

  public zoomIn(ticks: number = 0) {
    if (!this.pdfViewer) return

    let newScale = this.pdfViewer.currentScale
    do {
      newScale = newScale * DEFAULT_SCALE_DELTA
      newScale = Math.ceil(newScale * 10) / 10
      newScale = Math.min(MAX_SCALE, newScale)
    } while (--ticks && newScale < MAX_SCALE)
    this.pdfViewer.currentScaleValue = newScale.toFixed(2)
    this.saveScale()
  }

  public zoomOut(ticks: number = 0) {
    let newScale = this.pdfViewer.currentScale
    do {
      newScale = newScale / DEFAULT_SCALE_DELTA
      newScale = Math.floor(newScale * 10) / 10
      newScale = Math.max(MIN_SCALE, newScale)
    } while (--ticks && newScale > MIN_SCALE)
    this.pdfViewer.currentScaleValue = newScale.toFixed(2)
    this.saveScale()
  }

  public resetZoom() {
    if (!this.pdfViewer) return

    this.pdfViewer.currentScaleValue = DEFAULT_SCALE_VALUE
    this.clearScale()
  }

  private saveScale() {
    const currentScaleValue = this.pdfViewer.currentScaleValue
    localStorage.setItem(`pdf-reader-${this.loadParams?.bookId}-scale`, currentScaleValue)
  }

  private clearScale() {
    localStorage.removeItem(`pdf-reader-${this.loadParams?.bookId}-scale`)
  }

  public getSavedScale() {
    return localStorage.getItem(`pdf-reader-${this.loadParams?.bookId}-scale`)
  }
}
