﻿using ChildcareApplication.AdminTools;
using ChildcareApplication.DatabaseController;
using ChildcareApplication.Properties;
using DatabaseController;
using MessageBoxUtils;
using PdfSharp.Pdf;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdminTools {
    public partial class ParentReport : Window {
        private DataTable table;
        private bool reportLoaded;

        public ParentReport() {
            InitializeComponent();
            this.txt_GuardianID.Focus();
            this.parentDataGrid.IsTabStop = false;
            this.reportLoaded = false;
            this.dte_fromDate.Loaded += delegate {
                var textBox = (TextBox)dte_fromDate.Template.FindName("PART_TextBox", dte_fromDate);
                textBox.Background = dte_fromDate.Background;
            };
            this.dte_toDate.Loaded += delegate {
                var textBox = (TextBox)dte_toDate.Template.FindName("PART_TextBox", dte_toDate);
                textBox.Background = dte_toDate.Background;
            };
            this.MouseDown += WindowMouseDown;
            this.btn_CurrentMonthReport.ToolTip = GetCurMonthToolTip(DateTime.Now);
        }

        private string GetCurMonthToolTip(DateTime now) {
            Settings settings = new Settings();
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

            return "From " + fromMonth + "/" + fromDay + "/" + fromYear + " to " + toMonth + "/" + toDay + "/" + toYear;
        }

        //Loads a report based on the passed in MySQL query
        private void LoadReport(params string[] dates) {
            ReportsDB reportDB = new ReportsDB();
            this.table = reportDB.GetParentReportTable(this.txt_GuardianID.Text, dates);
            parentDataGrid.ItemsSource = table.DefaultView;

            this.reportLoaded = true;
        }

        private void btn_LoadAll_Click(object sender, RoutedEventArgs e) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();
            if (txt_GuardianID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_GuardianID.Text)) {
                LoadReport();
                LoadParentData();
            } else {
                WPFMessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_GuardianID.Focus();
            }
        }

        private void btn_CurrentMonthReport_Click(object sender, RoutedEventArgs e) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();
            String fromDate, toDate;
            Settings settings = new Settings();
            int fromMonth, fromYear, fromDay, toMonth, toYear, toDay;

            if (txt_GuardianID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_GuardianID.Text)) {
                fromDay = Convert.ToInt32(settings.BillingStartDate);
                toDay = fromDay - 1;

                if (DateTime.Now.Day < 20) { //previous month and this month
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
                LoadParentData();
            } else {
                WPFMessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_GuardianID.Focus();
            }
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

        private void btn_DateRangeReport_Click(object sender, RoutedEventArgs e) {
            DateTime dt;
            if (DateTime.TryParse(dte_fromDate.Text, out dt) && DateTime.TryParse(dte_toDate.Text, out dt)) {
                DateRangeReport();
            } else {
                WPFMessageBox.Show("Please enter valid dates!");
            }
        }

        private void DateRangeReport() {
            GuardianInfoDB parentInfo = new GuardianInfoDB();
            String initialFrom = Convert.ToDateTime(dte_fromDate.Text).ToString("dd/MM/yyyy");
            String initialTo = Convert.ToDateTime(dte_toDate.Text).ToString("dd/MM/yyyy");
            if (initialFrom.Length >= 10 && initialTo.Length >= 10) {
                initialFrom = initialFrom.Substring(0, 10);
                initialTo = initialTo.Substring(0, 10);
            }

            if (initialFrom.Length == 10 && initialTo.Length == 10) {
                if (txt_GuardianID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_GuardianID.Text)) {
                    String[] fromParts = initialFrom.Split('/');
                    String[] toParts = initialTo.Split('/');

                    if (fromParts.Length == 3 && toParts.Length == 3) {
                        String fromDate = fromParts[2] + "-" + fromParts[1] + "-" + fromParts[0];
                        String toDate = toParts[2] + "-" + toParts[1] + "-" + toParts[0];

                        LoadReport(fromDate, toDate);
                        LoadParentData();
                    } else {
                        WPFMessageBox.Show("You must enter a valid date range!");
                        dte_fromDate.Focus();
                    }
                } else {
                    WPFMessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                    txt_GuardianID.Focus();
                }
            } else {
                WPFMessageBox.Show("You must enter a valid date range!");
                dte_fromDate.Focus();
            }
        }

        //Loads the information for the parent on to the side of the window
        private void LoadParentData() {
            GuardianInfoDB parentInfo = new GuardianInfoDB();

            if (File.Exists(parentInfo.GetPhotoPath(txt_GuardianID.Text))) {
                cnv_ParentIcon.Background = new ImageBrush(new BitmapImage(new Uri(parentInfo.GetPhotoPath(txt_GuardianID.Text), UriKind.Relative)));
            } else {
                cnv_ParentIcon.Background = new ImageBrush(new BitmapImage(new Uri(@"" + "C:/Users/Public/Documents" + "/Childcare Application/Pictures/default.jpg", UriKind.Relative)));
            }
            lbl_Name.Content = parentInfo.GetParentName(txt_GuardianID.Text);
            lbl_Address1.Content = parentInfo.GetAddress1(txt_GuardianID.Text);
            lbl_Address2.Content = parentInfo.GetAddress2(txt_GuardianID.Text);
            lbl_Address3.Content = parentInfo.GetAddress3(txt_GuardianID.Text);
            lbl_Phone.Content = parentInfo.GetPhoneNumber(txt_GuardianID.Text);
            UpdateRegularDue(txt_GuardianID.Text);
            UpdateCampDue(txt_GuardianID.Text);
            UpdateMiscDue(txt_GuardianID.Text);
        }

        private void btn_RegularPayment_Click(object sender, RoutedEventArgs e) {
            SubmitPayment("Regular");
        }

        private void btn_CampPayment_Click(object sender, RoutedEventArgs e) {
            SubmitPayment("Camp");
        }

        private void btn_MiscPayment_Click(object sender, RoutedEventArgs e) {
            SubmitPayment("Misc");
        }

        private void SubmitPayment(string type) {
            GuardianInfoDB parentinfo = new GuardianInfoDB();

            if (txt_GuardianID.Text.Length == 6 && parentinfo.GuardianIDExists(txt_GuardianID.Text)) {
                PaymentEntry paymentEntry = new PaymentEntry(txt_GuardianID.Text, this, type);
                paymentEntry.ShowDialog();
            } else {
                WPFMessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_GuardianID.Focus();
            }
        }

        public void UpdateRegularDue(String parentID) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();

            lbl_RegularDueValue.Content = parentInfo.GetCurrentDue(txt_GuardianID.Text, "Regular");
        }

        public void UpdateCampDue(String parentID) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();

            lbl_CampDueValue.Content = parentInfo.GetCurrentDue(txt_GuardianID.Text, "Camp");
        }

        public void UpdateMiscDue(String parentID) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();

            lbl_MiscDueValue.Content = parentInfo.GetCurrentDue(txt_GuardianID.Text, "Misc");
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void txt_FromDate_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                dte_toDate.Focus();
            }
        }

        private void txt_ToDate_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                DateRangeReport();
            }
        }

        private void txt_GuardianID_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_GuardianID.SelectAll);
        }

        private void btn_Print_Click(object sender, RoutedEventArgs e) {
            if (this.reportLoaded && this.table.Rows.Count > 0) {
                PrintDialog printDialog = new PrintDialog();
                printDialog.UserPageRangeEnabled = true;

                if (printDialog.ShowDialog() == true) {
                    var paginator = new ReportsPaginator(this.table.Rows.Count, this.table,
                      new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));

                    printDialog.PrintDocument(paginator, "Parent Report Data Table");
                }
            } else {
                WPFMessageBox.Show("You must load a report before you can print one!");
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            if (this.reportLoaded && this.table.Rows.Count > 0) {
                PDFCreator pdfCreator = new PDFCreator(this.table);
                PdfDocument pdf = pdfCreator.CreatePDF(this.parentDataGrid.Columns.Count);
                pdfCreator.SavePDF(pdf);
            } else {
                WPFMessageBox.Show("You must load a report before you can save one!");
            }
        }
    }
}