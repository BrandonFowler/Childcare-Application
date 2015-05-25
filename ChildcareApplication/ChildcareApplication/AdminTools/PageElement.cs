//Example used to create this class found at: http://tech.pro/tutorial/888/wpf-printing-part-2-pagination

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdminTools {
    public class PageElement : UserControl {
        private DataTable table;
        private const int PageMargin = 75;
        private const int HeaderHeight = 25;
        private const int LineHeight = 20;
        private const int ColumnWidth = 93;

        private int currentRow;
        private int rows;

        public PageElement(int currentRow, int rows, DataTable table) {
            Margin = new Thickness(PageMargin);
            this.currentRow = currentRow;
            this.rows = rows;
            this.table = table;
        }

        public static int RowsPerPage(double height, double width) {  //32 rows in landscape
            return ((int)Math.Floor((height - (2 * PageMargin) - HeaderHeight) / LineHeight));
        }

        private static FormattedText MakeText(string text) {
            return new FormattedText(text, CultureInfo.CurrentCulture,
              FlowDirection.LeftToRight, new Typeface("Tahoma"), 10, Brushes.Black);
        }

        protected override void OnRender(DrawingContext dc) {
            Point curPoint = new Point(0, 0);
            int numCols = this.table.Columns.Count;
            int[] columnWidth = InitWidths(numCols);

            for (int i = 0; i < this.table.Columns.Count; i++) {
                dc.DrawText(MakeText(this.table.Columns[i].ColumnName), curPoint);
                curPoint.X += columnWidth[i];
            }

            curPoint.X = 0;
            curPoint.Y += LineHeight;

            dc.DrawRectangle(Brushes.Black, null, new Rect(curPoint, new Size(Width, 2)));
            curPoint.Y += HeaderHeight - LineHeight;

            for (int i = currentRow; i < currentRow + rows; i++) {
                for (int j = 0; j < this.table.Columns.Count; j++) {
                    string colValue = TrimColEntry(i, j, this.table.Columns.Count);
                    dc.DrawText(MakeText(colValue), curPoint);
                    curPoint.X += columnWidth[j];
                }
                curPoint.Y += LineHeight;
                curPoint.X = 0;
            }
        }

        private string TrimColEntry(int currentRow, int colNum, int colCount) {
            if (colCount == 6) { //Business Report
                if (colNum == 1) {
                    return TruncateString(this.table.Rows[currentRow].ItemArray[colNum].ToString(), 22);
                } else if (colNum == 2) {
                    return TruncateString(this.table.Rows[currentRow].ItemArray[colNum].ToString(), 20);
                } else if (colNum == 3) {
                    return TruncateString(this.table.Rows[currentRow].ItemArray[colNum].ToString(), 25);
                } else {
                    return this.table.Rows[currentRow].ItemArray[colNum].ToString();
                }
            } else {
                if (colNum == 1) {
                    return TruncateString(this.table.Rows[currentRow].ItemArray[colNum].ToString(), 15);
                } else if (colNum == 2) {
                    return TruncateString(this.table.Rows[currentRow].ItemArray[colNum].ToString(), 15);
                } else if (colNum == 3) {
                    return TruncateString(this.table.Rows[currentRow].ItemArray[colNum].ToString(), 20);
                } else {
                    return this.table.Rows[currentRow].ItemArray[colNum].ToString();
                }
            }
        }

        private string TruncateString(string val, int length) {
            if (!string.IsNullOrEmpty(val)) {
                if (val.Length < length) {
                    return val;
                } else {
                    return val.Substring(0, length);
                }
            } else {
                return val;
            }
        }

        private int[] InitWidths(int numCols) {
            int[] colWidths = new int[numCols];
            if (numCols == 6) { //business report
                colWidths[0] = 50;
                colWidths[1] = 125;
                colWidths[2] = 125;
                colWidths[3] = 150;
                colWidths[4] = 55;
                colWidths[5] = 0;
            } else {
                colWidths[0] = 70;
                colWidths[1] = 90;
                colWidths[2] = 90;
                colWidths[3] = 125;
                colWidths[4] = 60;
                colWidths[5] = 70;
                colWidths[6] = 0;
            }

            return colWidths;
        }
    }
}
