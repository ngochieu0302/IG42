
function LoadPdf(urlpdf, urlnode) {
    var socket = io(urlnode);
    socket.emit("client-load-main", { pagenum: pageNum });
        socket.on("server-load-main", function (data) {
            pdfjsLib.getDocument(urlpdf).promise.then(function (pdfDoc_) {
                pdfDoc = pdfDoc_;
                document.getElementById('page_count').textContent = pdfDoc.numPages;
                // Initial/first page rendering
                pageNum = data.pagenum;
                renderPage(data.pagenum);
            });
        });
    //    setInterval(function () {
    //        socket.on("server-load-main", function (data) {
    //            pdfjsLib.getDocument(urlpdf).promise.then(function (pdfDoc_) {
    //                pdfDoc = pdfDoc_;
    //                document.getElementById('page_count').textContent = pdfDoc.numPages;
    //                // Initial/first page rendering
    //                pageNum = data.pagenum;
    //                renderPage(data.pagenum);
    //            });
    //        });
    //}, 1000);
        socket.on("server-click-prev", function (data) {
            pageNum = data.pagenum;
            queueRenderPage(data.pagenum);
        });
        socket.on("server-click-next", function (data) {
            pageNum = data.pagenum;
            queueRenderPage(data.pagenum);
        });
    document.body.onkeydown = function (e) {
        var x = e.which || e.keyCode;
        if (x == 39) {
            socket.emit("client-click-next", { pagenum: pageNum, totalpage: pdfDoc.numPages });
        }
        if (x == 37) {
            socket.emit("client-click-prev", { pagenum: pageNum });
            
        }
    };
    document.getElementById("next").addEventListener("click", function () {
            socket.emit("client-click-next", { pagenum: pageNum, totalpage: pdfDoc.numPages });
            
    });
    document.getElementById("fullscreen").addEventListener("click", function () {
        if (canvas.requestFullscreen) {
            canvas.requestFullscreen();
        } else if (canvas.mozRequestFullScreen) { /* Firefox */
            canvas.mozRequestFullScreen();
        } else if (canvas.webkitRequestFullscreen) { /* Chrome, Safari & Opera */
            canvas.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) { /* IE/Edge */
            canvas.msRequestFullscreen();
        }
    });
    document.getElementById("prev").addEventListener("click", function () {
        socket.emit("client-click-prev", { pagenum: pageNum, totalpage: pdfDoc.numPages });
        
    });
}

function renderPage(num) {
    pageRendering = true;
    // Using promise to fetch the page
    pdfDoc.getPage(num).then(function (page) {
        var viewport = page.getViewport({ scale: scale });
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRendering = false;
            if (pageNumPending !== null) {
                // New page rendering is pending
                renderPage(pageNumPending);
                pageNumPending = null;
            }
        });
    });

    // Update page counters
    document.getElementById('page_num').textContent = num;
}

function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}
