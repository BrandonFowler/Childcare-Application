using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdminTools {
    class PDFCreator {
        private DataTable dataGrid;

        public PDFCreator(DataTable dataGrid) {
            this.dataGrid = dataGrid;
        }

        public PdfDocument CreatePDF() {
            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
                        new XRect(0, 0, page.Width, page.Height),
                        XStringFormats.Center);


            
            return pdf;
        }

        public void SavePDF(PdfDocument pdf) {
            string filename = @"..\HelloWorld.pdf";
            pdf.Save(filename);
            Process.Start(filename);
        }
    }
}
