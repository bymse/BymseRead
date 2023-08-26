import * as pdfjsLib from "pdfjs-dist/webpack"
import * as pdfjsViewer from "pdfjs-dist/web/pdf_viewer"


class PdfViewer {
  #pdfViewer;
  #dotNetHelper;
  #bookId;

  constructor() {
    this.#onWheel = this.#onWheel.bind(this);
  }
  
  async initialize(dotNetHelper, currentPage, bookId) {
    if (this.#pdfViewer) {
      this.#pdfViewer.cleanup();
    }
    this.#dotNetHelper = dotNetHelper;
    const container = document.getElementById("pdf-viewer-container");
    this.#bookId = bookId;

    const eventBus = new pdfjsViewer.EventBus();

    const pdfLinkService = new pdfjsViewer.PDFLinkService({
      eventBus,
    });

    const pdfFindController = new pdfjsViewer.PDFFindController({
      eventBus,
      linkService: pdfLinkService,
    });

    this.#pdfViewer = new pdfjsViewer.PDFViewer({
      container,
      eventBus,
      linkService: pdfLinkService,
      findController: pdfFindController,
    });
    pdfLinkService.setViewer(this.#pdfViewer);

    eventBus.on("pagesinit", () => {
      this.#pdfViewer.currentPageNumber = currentPage;
      this.#pdfViewer.currentScale = +localStorage.getItem('book-scale') || 1;
      this.#onInitialized();
    });

    eventBus.on("pagechanging", (e) => {
      if (e.pageNumber !== e.previous) {
        this.#onPageChanged(e.pageNumber);
      }
    });

    const loadingTask = pdfjsLib.getDocument({
      url: container.dataset.pdfSrc,
    });

    const pdfDocument = await loadingTask.promise;
    this.#pdfViewer.setDocument(pdfDocument);
    pdfLinkService.setDocument(pdfDocument, null);

    container.removeEventListener('wheel', this.#onWheel);
    container.addEventListener('wheel', this.#onWheel);
  }

  async #onInitialized() {
    await this.#dotNetHelper.invokeMethodAsync("OnPdfViewerInitialized", this.#pdfViewer.pagesCount);
  }

  async #onPageChanged(pageNumber) {
    await this.#dotNetHelper.invokeMethodAsync("OnCurrentPageChanged", pageNumber);
  }

  setCurrentPage(currentPage) {
    if (this.#pdfViewer) {
      this.#pdfViewer.currentPageNumber = +currentPage
    }
  }

  #onWheel = (e) => {
    if (e.ctrlKey) {
      e.preventDefault();
      if (e.deltaY < 0) {
        this.#pdfViewer.currentScale += 0.1;
      } else {
        this.#pdfViewer.currentScale -= 0.1;
      }
      localStorage.setItem('book-scale', this.#pdfViewer.currentScale);
    }
  }
}

window.PdfViewer = new PdfViewer();