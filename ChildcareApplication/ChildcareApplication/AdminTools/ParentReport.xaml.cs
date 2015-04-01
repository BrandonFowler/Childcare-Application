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
using System.Data.SQLite;
using ChildcareApplication.AdminTools;
using DatabaseController;

namespace AdminTools {
    public partial class ParentReport : Window {
        public ParentReport() {
            InitializeComponent();
            cnv_ParentIcon.Background = new SolidColorBrush(Colors.Aqua);
            this.txt_ParentID.Focus();
            this.ParentDataGrid.IsTabStop = false;
        }

        //Loads a report based on the passed in MySQL query
        private void LoadReport(String query) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);
                ParentDataGrid.ItemsSource = table.DefaultView;

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_LoadAll_Click(object sender, RoutedEventArgs e) {
            ParentInfoDB parentInfo = new ParentInfoDB();
            if (txt_ParentID.Text.Length == 6 && parentInfo.GuardianIDExists(txt_ParentID.Text)) {
                BuildQuery();
                LoadParentData();
            } else {
                MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_ParentID.Focus();
            }
        }

        private void btn_CurrentMonthReport_Click(object sender, RoutedEventArgs e) { 
            ParentInfoDB parentInfo = new ParentInfoDB();
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

                BuildQuery(fromDate, toDate);
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
            DateRangeReport();
        }

        private void DateRangeReport() {
            ParentInfoDB parentInfo = new ParentInfoDB();
            String initialFrom = txt_FromDate.Text;
            String initialTo = txt_ToDate.Text;
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

                        BuildQuery(fromDate, toDate);
                        LoadParentData();
                    } else {
                        MessageBox.Show("You must enter a valid date range!");
                        txt_FromDate.Focus();
                    }
                } else {
                    MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                    txt_ParentID.Focus();
                }
            } else {
                MessageBox.Show("You must enter a valid date range!");
                txt_FromDate.Focus();
            }
        }

        //builds a query based on passed in values
        private void BuildQuery(String start, String end) { //idea for how to format the transaction price from: http://stackoverflow.com/questions/9149063/sqlite-format-number-with-2-decimal-places-always
            string query = "SELECT strftime('%m-%d-%Y', ChildcareTransaction.TransactionDate) AS Date, Child.FirstName AS First, Child.LastName AS ";
            query += "Last, EventData.EventName AS 'Event Type', time(ChildcareTransaction.CheckedIn) AS 'Check In', ";
            query += "time(ChildcareTransaction.CheckedOut) AS 'Check Out', ";
            query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS Total FROM AllowedConnections NATURAL JOIN Child ";
            query += "NATURAL JOIN ChildcareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + txt_ParentID.Text + " ";
            query += "AND ChildcareTransaction.TransactionDate BETWEEN '" + start + "' AND '" + end + "';";

            LoadReport(query);
        }

        //builds a query string to show all charges
        private void BuildQuery() {
            string query = "SELECT strftime('%m/%d/%Y', ChildcareTransaction.TransactionDate) AS 'Date', Child.FirstName AS First, Child.LastName AS ";
            query += "Last, EventData.EventName AS 'Event Type', time(ChildcareTransaction.CheckedIn) AS 'Check In', ";
            query += "time(ChildcareTransaction.CheckedOut) AS 'Check Out', ";
            query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS Total FROM AllowedConnections NATURAL JOIN Child ";
            query += "NATURAL JOIN ChildcareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + txt_ParentID.Text + ";";

            LoadReport(query);
        }

        //Loads the information for the parent on to the side of the window
        private void LoadParentData() {
            ParentInfoDB parentInfo = new ParentInfoDB();

            cnv_ParentIcon.Background = new ImageBrush(new BitmapImage(new Uri(parentInfo.GetPhotoPath(txt_ParentID.Text), UriKind.Relative)));
            lbl_Name.Content = parentInfo.GetParentName(txt_ParentID.Text);
            lbl_Address1.Content = parentInfo.GetAddress1(txt_ParentID.Text);
            lbl_Address2.Content = parentInfo.GetAddress2(txt_ParentID.Text);
            lbl_Address3.Content = parentInfo.GetAddress3(txt_ParentID.Text);
            lbl_Phone.Content = parentInfo.GetPhoneNumber(txt_ParentID.Text);
            UpdateCurDue(txt_ParentID.Text);
        }

        private void btn_MakePayment_Click(object sender, RoutedEventArgs e) {
            ParentInfoDB parentinfo = new ParentInfoDB();

            if (txt_ParentID.Text.Length == 6 && parentinfo.GuardianIDExists(txt_ParentID.Text)) {
                PaymentEntry paymentEntry = new PaymentEntry(txt_ParentID.Text, this);
                paymentEntry.Show();
            } else {
                MessageBox.Show("The Parent ID you entered does not exist in the database.  Please verify it is correct.");
                txt_ParentID.Focus();
            }
            
        }

        public void UpdateCurDue(String parentID) {
            ParentInfoDB parentInfo = new ParentInfoDB();

            lbl_CurrentDueValue.Content = parentInfo.GetCurrentDue(txt_ParentID.Text);
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void txt_FromDate_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                txt_ToDate.Focus();
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

        private void txt_FromDate_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_FromDate.SelectAll);
        }

        private void txt_ToDate_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_ToDate.SelectAll);
        }
    }
}