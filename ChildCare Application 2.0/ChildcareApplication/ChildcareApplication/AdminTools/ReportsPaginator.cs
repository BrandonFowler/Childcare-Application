﻿//Example used to create this class found at: http://tech.pro/tutorial/888/wpf-printing-part-2-pagination

using System;
using System.Data;
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
                Width = this.pageSize.Width,
                Height = this.pageSize.Height,
            };

            page.Measure(this.pageSize);
            page.Arrange(new Rect(new Point(0, 0), this.pageSize));
            DocumentPage pages = new DocumentPage(page);

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
                this.rowsPerPage = PageElement.RowsPerPage(this.pageSize.Height, this.pageSize.Width);
            }
        }

        public override IDocumentPaginatorSource Source {
            get { return null; }
        }
    }
}
