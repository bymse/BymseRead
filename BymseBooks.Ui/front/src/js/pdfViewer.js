import * as pdfjsLib from "pdfjs-dist/webpack"
import * as pdfjsViewer from "pdfjs-dist/web/pdf_viewer"


class PdfViewer {
  #pdfViewer;
  #dotNetHelper;

  async initialize(dotNetHelper, currentPage) {
    console.log("PdfViewer.initialize");
    this.#dotNetHelper = dotNetHelper;
    const container = document.getElementById("pdf-viewer-container");

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
  }

  async #onInitialized() {
    await this.#dotNetHelper.invokeMethodAsync("OnPdfViewerInitialized", this.#pdfViewer.pagesCount);
  }

  async #onPageChanged(pageNumber) {
    await this.#dotNetHelper.invokeMethodAsync("OnCurrentPageChanged", pageNumber);
  }

  setCurrentPage(currentPage) {
    this.#pdfViewer.currentPageNumber = currentPage
  }
}

window.PdfViewer = new PdfViewer();