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
        private const int ColumnWidth = 80;

        private int currentRow;
        private int rows;

        public PageElement(int currentRow, int rows, DataTable table) {
            Margin = new Thickness(PageMargin);
            this.currentRow = currentRow;
            this.rows = rows;
            this.table = table;
        }

        public static int RowsPerPage(double height) {
            return ((int)Math.Floor((height - (2 * PageMargin) - HeaderHeight) / LineHeight));
        }

        private static FormattedText MakeText(string text) {
            return new FormattedText(text, CultureInfo.CurrentCulture,
              FlowDirection.LeftToRight, new Typeface("Tahoma"), 10, Brushes.Black);
        }

        protected override void OnRender(DrawingContext dc) {
            Point curPoint = new Point(0, 0);

            for (int i = 0; i < this.table.Columns.Count; i++) {
                dc.DrawText(MakeText(this.table.Columns[i].ColumnName), curPoint);
                curPoint.X += ColumnWidth;
            }

            curPoint.X = 0;
            curPoint.Y += LineHeight;

            dc.DrawRectangle(Brushes.Black, null, new Rect(curPoint, new Size(Width, 2)));
            curPoint.Y += HeaderHeight - LineHeight;

            for (int i = currentRow; i < currentRow + rows; i++) {
                for (int j = 0; j < this.table.Columns.Count; j++) {
                    dc.DrawText(MakeText(this.table.Rows[i].ItemArray[j].ToString()), curPoint);
                    curPoint.X += ColumnWidth;
                }
                curPoint.Y += LineHeight;
                curPoint.X = 0;
            }
        }
    }
}
