import * as pdfjsLib from "pdfjs-dist/webpack"
import * as pdfjsViewer from "pdfjs-dist/web/pdf_viewer"


class PdfViewer {

  async initialize() {
    const container = document.getElementById("pdf-viewer-container");
    
    const CMAP_URL = "../../node_modules/pdfjs-dist/cmaps/";
    const CMAP_PACKED = true;

    const DEFAULT_URL = "../../web/compressed.tracemonkey-pldi-09.pdf";
// To test the AcroForm and/or scripting functionality, try e.g. this file:
// "../../test/pdfs/160F-2019.pdf"

    const ENABLE_XFA = true;
    const SEARCH_FOR = ""; // try "Mozilla";

    const SANDBOX_BUNDLE_SRC = "../../node_modules/pdfjs-dist/build/pdf.sandbox.js";


    const eventBus = new pdfjsViewer.EventBus();

// (Optionally) enable hyperlinks within PDF files.
    const pdfLinkService = new pdfjsViewer.PDFLinkService({
      eventBus,
    });

// (Optionally) enable find controller.
    const pdfFindController = new pdfjsViewer.PDFFindController({
      eventBus,
      linkService: pdfLinkService,
    });

// (Optionally) enable scripting support.
    const pdfScriptingManager = new pdfjsViewer.PDFScriptingManager({
      eventBus,
      sandboxBundleSrc: SANDBOX_BUNDLE_SRC,
    });

    const pdfViewer = new pdfjsViewer.PDFViewer({
      container,
      eventBus,
      linkService: pdfLinkService,
      findController: pdfFindController,
      scriptingManager: pdfScriptingManager,
    });
    pdfLinkService.setViewer(pdfViewer);
    pdfScriptingManager.setViewer(pdfViewer);

    eventBus.on("pagesinit", function () {
      // We can use pdfViewer now, e.g. let's change default scale.
      
    });

// Loading document.
    const loadingTask = pdfjsLib.getDocument({
      url: container.dataset.pdfSrc,
    });
    await (async function () {
      const pdfDocument = await loadingTask.promise;
      // Document loaded, specifying document for the viewer and
      // the (optional) linkService.
      pdfViewer.setDocument(pdfDocument);

      pdfLinkService.setDocument(pdfDocument, null);
    })();
  }
}

window.PdfViewer = new PdfViewer();