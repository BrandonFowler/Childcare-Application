using ChildcareApplication.DatabaseController;
using DatabaseController;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBoxUtils;

namespace AdminTools {
    public partial class BusinessReport : Window {
        private DataTable table;
        private bool reportLoaded;
        private ChildcareApplication.Properties.Settings settings;

        public BusinessReport() {
            InitializeComponent();
            InitializeCMB_Year();
            reportLoaded = false;
            settings = new ChildcareApplication.Properties.Settings();
            this.MouseDown += WindowMouseDown;
            this.btn_CurrentMonthReport.ToolTip = GetCurMonthToolTip(DateTime.Now);
        }

        private void InitializeCMB_Year() {
            GregorianCalendar cal = new GregorianCalendar();

            int curYear = cal.GetYear(DateTime.Now);

            ComboBoxItem cmbPrev = new ComboBoxItem();
            ComboBoxItem cmbCur = new ComboBoxItem();

            cmbPrev.Content = curYear - 1;
            cmbCur.Content = curYear;

            cmb_Year.Items.Add(cmbPrev);
            cmb_Year.Items.Add(cmbCur);
        }

        private string GetCurMonthToolTip(DateTime now) {
            int fromMonth, fromYear, fromDay, toMonth, toYear, toDay;

            fromDay = Convert.ToInt32(settings.BillingStartDate);
            toDay = fromDay - 1;

            if (DateTime.Now.Day < fromDay) { //previous month and this month
                if (DateTime.Now.Month != 1) {
                    fromYear = DateTime.Now.Year;
                    fromMonth = DateTime.Now.Month - 1;
                } else {
                    fromYear = DateTime.Now.Year - 1;
                    fromMonth = 12;
                }
                toYear = DateTime.Now.Year;
                toMonth = DateTime.Now.Month;
            } else { //this month and next month
                fromYear = DateTime.Now.Year;
                fromMonth = DateTime.Now.Month;
                if (DateTime.Now.Month != 12) {
                    toYear = DateTime.Now.Year;
                    toMonth = DateTime.Now.Month;
                } else {
                    toYear = DateTime.Now.Year + 1;
                    toMonth = 1;
                }
            }

            return "From " + fromMonth + "/" + fromDay + "/" + fromYear + " to " + toMonth + "/" + toDay + "/" + toYear;
        }

        private void btn_CurrentMonthReport_Click(object sender, RoutedEventArgs e) {
            string fromDate, toDate;
            int fromMonth, fromYear, fromDay, toMonth, toYear, toDay;

            fromDay = Convert.ToInt32(settings.BillingStartDate);
            toDay = fromDay - 1;

            if (DateTime.Now.Day < fromDay) { //previous month and this month
                if (DateTime.Now.Month != 1) {
                    fromYear = DateTime.Now.Year;
                    fromMonth = DateTime.Now.Month - 1;
                } else {
                    fromYear = DateTime.Now.Year - 1;
                    fromMonth = 12;
                }
                toYear = DateTime.Now.Year;
                toMonth = DateTime.Now.Month;
            } else { //this month and next month
                fromYear = DateTime.Now.Year;
                fromMonth = DateTime.Now.Month;
                if (DateTime.Now.Month != 12) {
                    toYear = DateTime.Now.Year;
                    toMonth = DateTime.Now.Month + 1;
                } else {
                    toYear = DateTime.Now.Year + 1;
                    toMonth = 1;
                }
            }

            fromDate = BuildDateString(fromYear, fromMonth, fromDay);
            toDate = BuildDateString(toYear, toMonth, toDay);

            LoadReport(fromDate, toDate);
        }

        private string BuildDateString(int year, int month, int day) {
            String date;

            date = year + "-";

            if (month < 10) {
                date += "0" + month + "-";
            } else {
                date += month + "-";
            }
            if (day < 10) {
                date += "0" + day;
            } else {
                date += day;
            }
            return date;
        }

        private void btn_SpecificMonth_Click(object sender, RoutedEventArgs e) {
            if (cmb_Month.SelectedIndex != -1 && cmb_Year.SelectedIndex != -1) {
                String fromDate, toDate;

                String month = ((ComboBoxItem)cmb_Month.SelectedItem).Content.ToString();
                int year = Convert.ToInt32(((ComboBoxItem)cmb_Year.SelectedItem).Content);

                int monthNum = GetMonthNum(month);
                int fromDay = Convert.ToInt32(settings.BillingStartDate);
                int toDay = fromDay - 1;

                fromDate = BuildDateString(year, monthNum, fromDay);

                if (monthNum != 12) {
                    toDate = BuildDateString(year, monthNum + 1, toDay);
                } else {
                    toDate = BuildDateString(year + 1, 1, toDay);
                }

                LoadReport(fromDate, toDate);
            } else {
                WPFMessageBox.Show("You must select a month and year from the drop down menus.");
            }
        }

        private int GetMonthNum(String month) {
            if (month == "January")
                return 1;
            if (month == "February")
                return 2;
            if (month == "March")
                return 3;
            if (month == "April")
                return 4;
            if (month == "May")
                return 5;
            if (month == "June")
                return 6;
            if (month == "July")
                return 7;
            if (month == "August")
                return 8;
            if (month == "September")
                return 9;
            if (month == "October")
                return 10;
            if (month == "November")
                return 11;
            return 12;
        }

        private void LoadReport(string startDate, string endDate) {
            ReportsDB reportsDB = new ReportsDB();
            DataTable table = reportsDB.GetBusinessReportTable(startDate, endDate);
            AddTotalsColumn(table);
            this.table = table;

            businessDataGrid.ItemsSource = table.DefaultView;

            this.reportLoaded = true;
        }

        private void AddTotalsColumn(DataTable table) {
            GuardianInfoDB gDB = new GuardianInfoDB();
            table.Columns.Add("Totals", typeof(string));
            if (table.Rows.Count > 1) {
                string id = "";
                bool campTotalDisplayed = false;
                bool regTotalDisplayed = false;

                for (int i = 0; i < table.Rows.Count; i++) {
                    if (id != table.Rows[i][0].ToString().Remove(5)) {
                        campTotalDisplayed = false;
                        regTotalDisplayed = false;
                        id = table.Rows[i][0].ToString().Remove(5);
                    }
                    if (IsRegular(table.Rows[i][3].ToString())) {
                        if (!regTotalDisplayed) {
                            table.Rows[i][5] = String.Format("{0:0.00}", gDB.GetCurrentDue(table.Rows[i][0].ToString(), "Regular"));
                            regTotalDisplayed = true;
                        }
                    } else if (table.Rows[i][3].ToString().Contains("Camp") || table.Rows[i][3].ToString().Contains("camp")) {
                        if (!campTotalDisplayed) {
                            campTotalDisplayed = true;
                            table.Rows[i][5] = String.Format("{0:0.00}", gDB.GetCurrentDue(table.Rows[i][0].ToString(), "Camp"));
                        }
                    }
                    if (table.Rows[i][5] == "$0.00") {
                        table.Rows[i][5] = "";
                    }

                }
            }
        }

        private bool IsRegular(string eventName) {
            return (eventName == "Regular Childcare" || eventName == "Infant Childcare" || eventName == "Adolescent Childcare");
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e) { //height = 1056, width = 816
            if (this.reportLoaded && this.table.Rows.Count > 0) {
                PrintDialog printDialog = new PrintDialog();
                printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
                printDialog.UserPageRangeEnabled = true;

                if (printDialog.ShowDialog() == true) {
                    var paginator = new ReportsPaginator(this.table.Rows.Count, this.table,
                      new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));

                    printDialog.PrintDocument(paginator, "Business Report Data Table");
                }
            } else {
                WPFMessageBox.Show("You must load a report before you can print one!");
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            if (this.reportLoaded && this.table.Rows.Count > 0) {
                PDFCreator pdfCreator = new PDFCreator(this.table);
                PdfDocument pdf = pdfCreator.CreatePDF(this.businessDataGrid.Columns.Count);
                pdfCreator.SavePDF(pdf);
            } else {
                WPFMessageBox.Show("You must load a report before you can save one!");
            }
        }
    }
}
