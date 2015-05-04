using System;
using System.Collections.Generic;
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
using System.Data;
using ChildcareApplication.AdminTools;
using DatabaseController;
using ChildcareApplication.DatabaseController;

namespace AdminTools {
    public partial class ParentReport : Window {
        private DataTable table;
        private bool reportLoaded;

        public ParentReport() {
            InitializeComponent();
            cnv_ParentIcon.Background = new SolidColorBrush(Colors.Aqua);
            this.txt_ParentID.Focus();
            this.parentDataGrid.IsTabStop = false;
            this.reportLoaded = false;
            this.dte_fromDate.Loaded += delegate{
                var textBox = (TextBox)dte_fromDate.Template.FindName("PART_TextBox", dte_fromDate);
                textBox.Background = dte_fromDate.Background;
            };
            this.dte_toDate.Loaded += delegate{
                var textBox = (TextBox)dte_toDate.Template.FindName("PART_TextBox", dte_toDate);
                textBox.Background = dte_toDate.Background;
            };
            this.MouseDown += WindowMouseDown;
        }

        //Loads a report based on the passed in MySQL query
        private void LoadReport(params string[] dates) {
            ReportsDB reportDB = new ReportsDB();
            this.table = reportDB.GetParentReportTable(this.txt_ParentID.Text, dates);
            parentDataGrid.ItemsSource = table.DefaultView;
            
            this.reportLoaded = true;
            //parentDataGrid.CellStyle.
        }

        private void btn_LoadAll_Click(object sender, RoutedEventArgs e) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();
            if (txt_ParentID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_ParentID.Text)) {
                LoadReport();
                LoadParentData();
            } else {
                MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_ParentID.Focus();
            }
        }

        private void btn_CurrentMonthReport_Click(object sender, RoutedEventArgs e) { 
            GuardianInfoDB parentInfo = new GuardianInfoDB();
            String fromDate, toDate;
            int fromMonth, fromYear, fromDay, toMonth, toYear, toDay;

            if (txt_ParentID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_ParentID.Text)) {
                fromDay = 20;
                toDay = 19;

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
                        toMonth = DateTime.Now.Month;
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
                MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_ParentID.Focus();
            }
        }

        private string BuildDateString(int year, int month, int day) {
            String date;

            date = year + "-";

            if (month < 10) {
                date += "0" + month;
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
            }
            else {
                MessageBox.Show("Please enter valid dates!");
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
                if (txt_ParentID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_ParentID.Text)) {
                    String[] fromParts = initialFrom.Split('/');
                    String[] toParts = initialTo.Split('/');

                    if (fromParts.Length == 3 && toParts.Length == 3) {
                        String fromDate = fromParts[2] + "-" + fromParts[0] + "-" + fromParts[1];
                        String toDate = toParts[2] + "-" + toParts[0] + "-" + toParts[1];

                        LoadReport(fromDate, toDate);
                        LoadParentData();
                    } else {
                        MessageBox.Show("You must enter a valid date range!");
                        dte_fromDate.Focus();
                    }
                } else {
                    MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                    txt_ParentID.Focus();
                }
            } else {
                MessageBox.Show("You must enter a valid date range!");
                dte_fromDate.Focus();
            }
        }

        //Loads the information for the parent on to the side of the window
        private void LoadParentData() {
            GuardianInfoDB parentInfo = new GuardianInfoDB();

            cnv_ParentIcon.Background = new ImageBrush(new BitmapImage(new Uri(parentInfo.GetPhotoPath(txt_ParentID.Text), UriKind.Relative)));
            lbl_Name.Content = parentInfo.GetParentName(txt_ParentID.Text);
            lbl_Address1.Content = parentInfo.GetAddress1(txt_ParentID.Text);
            lbl_Address2.Content = parentInfo.GetAddress2(txt_ParentID.Text);
            lbl_Address3.Content = parentInfo.GetAddress3(txt_ParentID.Text);
            lbl_Phone.Content = parentInfo.GetPhoneNumber(txt_ParentID.Text);
            UpdateCurDue(txt_ParentID.Text);
        }

        private void btn_MakePayment_Click(object sender, RoutedEventArgs e) {
            GuardianInfoDB parentinfo = new GuardianInfoDB();

            if (txt_ParentID.Text.Length == 6 && parentinfo.GuardianIDExists(txt_ParentID.Text)) {
                PaymentEntry paymentEntry = new PaymentEntry(txt_ParentID.Text, this);
                paymentEntry.ShowDialog();
            } else {
                MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_ParentID.Focus();
            }
            
        }

        public void UpdateCurDue(String parentID) {
            GuardianInfoDB parentInfo = new GuardianInfoDB();

            lbl_CurrentDueValue.Content = parentInfo.GetCurrentDue(txt_ParentID.Text);
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

        private void txt_ParentID_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_ParentID.SelectAll);
        }

        /*private void txt_FromDate_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_FromDate.SelectAll);
        }This method is probably no longer necessary and doesn't currently compile*/

        /*private void txt_ToDate_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_ToDate.SelectAll);
        }This method is probably no longer necessary and doesn't currently compile*/

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
                MessageBox.Show("You must load a report before you can print one!");
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}