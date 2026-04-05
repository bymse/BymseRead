import * as pdfjsLib from 'pdfjs-dist'
import 'pdfjs-dist/web/pdf_viewer.css'
import { ReaderOutlineItem } from '@components/Reader/readerOutline.ts'
import type { RefProxy } from 'pdfjs-dist/types/src/display/api'
import { EventBus, LinkTarget, PDFLinkService, PDFViewer } from 'pdfjs-dist/web/pdf_viewer.mjs'

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
  onOutlineReady?: (items: ReaderOutlineItem[]) => void
}

type RawOutlineItem = {
  title: string
  dest: string | Array<unknown> | null
  url: string | null
  items: RawOutlineItem[]
}

export class PdfReader {
  private readonly eventBus: EventBus
  private readonly pdfViewer: PDFViewer
  private readonly pdfLinkService: PDFLinkService
  private pdfDocument?: pdfjsLib.PDFDocumentProxy
  private loadingTask?: pdfjsLib.PDFDocumentLoadingTask
  private loadParams?: LoadParams
  private outlineLoadVersion = 0
  public lastPage?: number
  public totalPages: number = 0

  constructor(container: HTMLDivElement) {
    this.eventBus = new EventBus()
    this.pdfLinkService = new PDFLinkService({
      eventBus: this.eventBus,
      externalLinkTarget: LinkTarget.BLANK,
      externalLinkRel: 'noopener noreferrer',
    })

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
      this.totalPages = this.pdfViewer.pagesCount
      this.pdfViewer.currentScaleValue = this.getSavedScale() || DEFAULT_SCALE_VALUE

      void this.loadOutlineData()
      this.loadParams?.onInitialized?.(this)
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
      this.loadParams = params
      this.pdfLinkService.setDocument(this.pdfDocument)
      this.pdfViewer.setDocument(this.pdfDocument)
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
    this.outlineLoadVersion += 1

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

  private async loadOutlineData() {
    if (!this.pdfDocument) {
      return
    }

    const outlineLoadVersion = ++this.outlineLoadVersion
    try {
      const outline = this.toRawOutlineItems(await this.pdfDocument.getOutline())
      if (!this.isCurrentOutlineLoad(outlineLoadVersion)) {
        return
      }

      if (!outline.length) {
        this.loadParams?.onOutlineReady?.([])
        return
      }

      const destinationCache = new Map<string, Array<unknown> | null>()
      const pageNumberCache = new Map<string, number>()
      const normalizedItems = await this.normalizeOutlineItems(outline, 0, destinationCache, pageNumberCache)

      if (!this.isCurrentOutlineLoad(outlineLoadVersion)) {
        return
      }

      this.loadParams?.onOutlineReady?.(normalizedItems)
    } catch {
      // Outline loading failed — the contents button simply won't appear
    }
  }

  private async normalizeOutlineItems(
    outlineItems: RawOutlineItem[],
    depth: number,
    destinationCache: Map<string, Array<unknown> | null>,
    pageNumberCache: Map<string, number>,
    prefix: string = '',
  ): Promise<ReaderOutlineItem[]> {
    const normalizedItems: ReaderOutlineItem[] = []

    for (let index = 0; index < outlineItems.length; index++) {
      const outlineItem = outlineItems[index]
      const id = prefix ? `${prefix}-${index}` : `${index}`
      const pageNumber = await this.resolvePageNumber(outlineItem.dest, destinationCache, pageNumberCache)
      const url = outlineItem.url ?? null
      const children = await this.normalizeOutlineItems(
        outlineItem.items,
        depth + 1,
        destinationCache,
        pageNumberCache,
        id,
      )
      const isExternal = pageNumber === null && url !== null

      normalizedItems.push({
        id,
        title: outlineItem.title?.trim() || 'Untitled section',
        pageNumber,
        url,
        children,
        depth,
        isExternal,
        isNavigable: pageNumber !== null || isExternal,
      })
    }

    return normalizedItems
  }

  private async resolvePageNumber(
    destination: string | Array<unknown> | null,
    destinationCache: Map<string, Array<unknown> | null>,
    pageNumberCache: Map<string, number>,
  ): Promise<number | null> {
    if (!this.pdfDocument || destination === null) {
      return null
    }

    let resolvedDestination: Array<unknown> | null = null
    if (typeof destination === 'string') {
      if (destinationCache.has(destination)) {
        resolvedDestination = destinationCache.get(destination) ?? null
      } else {
        resolvedDestination = await this.pdfDocument.getDestination(destination)
        destinationCache.set(destination, resolvedDestination)
      }
    } else {
      resolvedDestination = destination
    }

    if (!Array.isArray(resolvedDestination) || resolvedDestination.length === 0) {
      return null
    }

    const destinationTarget = resolvedDestination[0]
    if (typeof destinationTarget === 'number') {
      return destinationTarget + 1
    }

    const pageRef = this.toPdfRef(destinationTarget)
    if (!pageRef) {
      return null
    }

    const refKey = `${pageRef.num}:${pageRef.gen}`
    if (pageNumberCache.has(refKey)) {
      return pageNumberCache.get(refKey) ?? null
    }

    try {
      const pageIndex = await this.pdfDocument.getPageIndex({ num: pageRef.num, gen: pageRef.gen })
      const pageNumber = pageIndex + 1
      pageNumberCache.set(refKey, pageNumber)
      return pageNumber
    } catch {
      return null
    }
  }

  private isCurrentOutlineLoad(outlineLoadVersion: number) {
    return this.outlineLoadVersion === outlineLoadVersion
  }

  private toRawOutlineItems(outline: unknown): RawOutlineItem[] {
    if (!Array.isArray(outline)) {
      return []
    }

    const normalizedItems: RawOutlineItem[] = []

    for (const outlineItem of outline) {
      const normalizedItem = this.toRawOutlineItem(outlineItem)
      if (normalizedItem) {
        normalizedItems.push(normalizedItem)
      }
    }

    return normalizedItems
  }

  private toRawOutlineItem(outlineItem: unknown): RawOutlineItem | null {
    if (!outlineItem || typeof outlineItem !== 'object') {
      return null
    }

    const item = outlineItem as { title?: unknown; dest?: unknown; url?: unknown; items?: unknown }
    const destination = this.toRawDestination(item.dest)

    return {
      title: typeof item.title === 'string' ? item.title : '',
      dest: destination,
      url: typeof item.url === 'string' ? item.url : null,
      items: this.toRawOutlineItems(item.items),
    }
  }

  private isPdfRef(value: unknown): value is RefProxy {
    if (!value || typeof value !== 'object') {
      return false
    }

    const maybeRef = value as { num?: unknown; gen?: unknown }
    return typeof maybeRef.num === 'number' && typeof maybeRef.gen === 'number'
  }

  private toPdfRef(value: unknown): RefProxy | null {
    if (!this.isPdfRef(value)) {
      return null
    }

    return value
  }

  private toRawDestination(value: unknown): string | Array<unknown> | null {
    if (typeof value === 'string' || value === null) {
      return value
    }

    if (!Array.isArray(value)) {
      return null
    }

    const destination: Array<unknown> = []
    for (const entry of value) {
      destination.push(entry)
    }

    return destination
  }
}
