//Example used to create this class found at: http://tech.pro/tutorial/888/wpf-printing-part-2-pagination

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace AdminTools {
    class ReportsPaginator : DocumentPaginator {
        private int rowsPerPage;
        private Size pageSize;
        private int rows;
        private DataTable table;

        public ReportsPaginator(int rows, DataTable table, Size pageSize) {
            this.rows = rows;
            this.table = table;
            PageSize = pageSize;
        }

        public override DocumentPage GetPage(int pageNumber) {
            int currentRow = this.rowsPerPage * pageNumber;

            var page = new PageElement(currentRow, Math.Min(this.rowsPerPage, this.rows - currentRow), this.table) {
                Width = PageSize.Width,
                Height = PageSize.Height,
            };

            page.Measure(PageSize);
            page.Arrange(new Rect(new Point(0, 0), PageSize));

            return new DocumentPage(page);
        }

        public override bool IsPageCountValid {
            get { return true; }
        }

        public override int PageCount {
            get { return (int)Math.Ceiling(rows / (double)rowsPerPage); }
        }

        public override Size PageSize {
            get {
                return this.pageSize;
            }
            set {
                this.pageSize = value;
                if (this.table.TableName == "Business Report") {
                    this.rowsPerPage = PageElement.RowsPerPage(this.pageSize.Width);
                } else {
                    this.rowsPerPage = PageElement.RowsPerPage(this.pageSize.Height);
                }

                //Can't print anything if you can't fit a row on a page
                Debug.Assert(this.rowsPerPage > 0);
            }
        }

        public override IDocumentPaginatorSource Source {
            get { return null; }
        }
    }
}
