import { PDFLinkService } from 'pdfjs-dist/web/pdf_viewer.mjs'

type PdfRef = {
  num: number
  gen: number
}

type LinkServicePdfDocument = {
  getDestination(dest: string): Promise<unknown>
  cachedPageNumber(ref: PdfRef): number | null | undefined
  getPageIndex(ref: PdfRef): Promise<number>
}

type LinkServicePdfViewer = {
  pageLabelToPageNumber(label: string): number
}

export class ReaderLinkService extends PDFLinkService {
  constructor(
    options: ConstructorParameters<typeof PDFLinkService>[0],
    private readonly onInternalLinkNavigation?: (page: number) => void,
  ) {
    super(options)
  }

  public override async goToDestination(dest: string | Array<unknown>) {
    this.notifyInternalLinkNavigation(await this.resolveDestinationPageNumber(dest))
    await super.goToDestination(dest)
  }

  public override goToPage(val: number | string) {
    this.notifyInternalLinkNavigation(this.resolvePageNumber(val))
    super.goToPage(val)
  }

  private async resolveDestinationPageNumber(dest: string | Array<unknown>): Promise<number | null> {
    const pdfDocument = this.getPdfDocument()
    if (!pdfDocument) {
      return null
    }

    const explicitDest = typeof dest === 'string' ? await pdfDocument.getDestination(dest) : dest
    if (!Array.isArray(explicitDest)) {
      return null
    }

    const destRef: unknown = explicitDest[0] as unknown
    const pdfRef = this.toPdfRef(destRef)
    if (pdfRef) {
      let pageNumber = pdfDocument.cachedPageNumber(pdfRef)
      if (!pageNumber) {
        try {
          pageNumber = (await pdfDocument.getPageIndex(pdfRef)) + 1
        } catch {
          return null
        }
      }
      return this.isValidPageNumber(pageNumber) ? pageNumber : null
    }

    if (typeof destRef === 'number' && Number.isInteger(destRef)) {
      const pageNumber = destRef + 1
      return this.isValidPageNumber(pageNumber) ? pageNumber : null
    }

    return null
  }

  private resolvePageNumber(val: number | string): number | null {
    const pageNumber = typeof val === 'string' ? this.getPdfViewer()?.pageLabelToPageNumber(val) : val
    return this.isValidPageNumber(pageNumber) ? pageNumber : null
  }

  private isValidPageNumber(pageNumber: number | undefined): pageNumber is number {
    return !!pageNumber && pageNumber > 0 && pageNumber <= this.pagesCount
  }

  private notifyInternalLinkNavigation(targetPage: number | null) {
    const currentPage = this.page
    if (targetPage === null || targetPage === currentPage) {
      return
    }

    this.onInternalLinkNavigation?.(currentPage)
  }

  private getPdfDocument(): LinkServicePdfDocument | null {
    return this.pdfDocument as LinkServicePdfDocument | null
  }

  private getPdfViewer(): LinkServicePdfViewer | null {
    return this.pdfViewer as LinkServicePdfViewer | null
  }

  private toPdfRef(value: unknown): PdfRef | null {
    if (!value || typeof value !== 'object') {
      return null
    }

    const maybeRef = value as { num?: unknown; gen?: unknown }
    if (typeof maybeRef.num !== 'number' || typeof maybeRef.gen !== 'number') {
      return null
    }

    return {
      num: maybeRef.num,
      gen: maybeRef.gen,
    }
  }
}
