using MessageBoxUtils;
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
        private DataTable table;
        private const int PageMargin = 30;
        private const int HeaderHeight = 25;
        private const int LineHeight = 17;
        private const int ColumnWidth = 125;
        private const int RowsPerPage = 44;

        public PDFCreator(DataTable dataGrid) {
            this.table = dataGrid;
        }

        public PdfDocument CreatePDF(int numCols) {
            PdfDocument pdf = new PdfDocument();
            XFont font = new XFont("Verdana", 10, XFontStyle.Regular);
            int currentRow = 0;
            int rows = this.table.Rows.Count;
            int[] columnWidth = InitWidths(numCols);

            while (currentRow < rows) {
                PdfPage page = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XPoint curPoint = new XPoint(PageMargin, PageMargin);

                for (int i = 0; i < this.table.Columns.Count; i++) {
                    gfx.DrawString(this.table.Columns[i].ColumnName, font, XBrushes.Black, curPoint);
                    curPoint.X += columnWidth[i];
                }

                curPoint.X = PageMargin;
                curPoint.Y += 5;

                gfx.DrawLine(XPens.Black, curPoint, new XPoint((page.Width - PageMargin), curPoint.Y));
                curPoint.Y += 15;

                for (int i = 0; i < RowsPerPage && currentRow < rows; i++, currentRow++) {
                    for (int j = 0; j < this.table.Columns.Count; j++) {
                        gfx.DrawString(this.table.Rows[currentRow].ItemArray[j].ToString(), font, XBrushes.Black, curPoint);
                        curPoint.X += columnWidth[j];
                    }
                    curPoint.Y += LineHeight;
                    curPoint.X = PageMargin;
                }
            }
            
            return pdf;
        }

        private int[] InitWidths(int numCols) {
            int[] colWidths = new int[numCols];
            if (numCols == 5) { //business report
                colWidths[0] = 50;
                colWidths[1] = 150;
                colWidths[2] = 150;
                colWidths[3] = 150;
                colWidths[4] = 0;
            } else {
                colWidths[0] = 125;
                colWidths[1] = 125;
                colWidths[2] = 125;
                colWidths[3] = 125;
                colWidths[4] = 125;
                colWidths[5] = 125;
                colWidths[6] = 0;
            }

            return colWidths;
        }

        public void SavePDF(PdfDocument pdf) {
            string filename = @"..\..\Saved Reports\HelloWorld.pdf";
            try {
                pdf.Save(filename); //try catches here
                Process.Start(filename);
            } catch (System.IO.IOException) {
                WPFMessageBox.Show("Unable to access file!  Ensure the file is not opened and that you have permission to edit files in the location specified.");
            } catch (Exception) {
                WPFMessageBox.Show("Error occurred while attempting to save the report. Report has not been saved.");
            }
        }
    }
}
