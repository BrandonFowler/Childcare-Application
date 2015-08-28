using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

namespace AdminTools {
    /// <summary>
    /// Interaction logic for BusinessReport.xaml
    /// </summary>
    public partial class BusinessReport : Window {
        public BusinessReport() {
            InitializeComponent();
            InitializeCMB_Year();
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

        private void btn_CurrentMonthReport_Click(object sender, RoutedEventArgs e) {
            ParentInfoDB parentInfo = new ParentInfoDB();
            String fromDate, toDate;
            int fromMonth, fromYear, fromDay, toMonth, toYear, toDay;

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

        private void btn_SpecificMonth_Click(object sender, RoutedEventArgs e) {
            GregorianCalendar cal = new GregorianCalendar();

            if (cmb_Month.SelectedIndex != -1 && cmb_Year.SelectedIndex != 1) {
                String fromDate, toDate;

                String month = ((ComboBoxItem)cmb_Month.SelectedItem).Content.ToString();
                int year = Convert.ToInt32(((ComboBoxItem)cmb_Year.SelectedItem).Content);

                int monthNum = GetMonthNum(month);
                int fromDay = 20;
                int toDay = 19;

                fromDate = BuildDateString(year, monthNum, fromDay);

                if (monthNum != 12) {
                    toDate = BuildDateString(year, monthNum + 1, toDay);
                } else {
                    toDate = BuildDateString(year + 1, monthNum + 1, toDay);
                }

                BuildQuery(fromDate, toDate);
            } else {
                MessageBox.Show("You must select a month and year from the drop down menus.");
            }
        }

        private int GetMonthNum(String month) { //TODO: Find a better way to handle something like this
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

        private void BuildQuery(String start, String end) {
            string query = "SELECT Guardian.Guardian_ID AS ID, Guardian.FirstName AS First, Guardian.LastName AS Last, ";
            query += "Guardian.Phone, Guardian.Address1, Guardian.Address2, Guardian.City, Guardian.StateAbrv AS State, Guardian.Zip, ";
            query += "'$' || case WHEN substr(ChildcareTransaction.TransactionTotal, -2, 1) = '.' THEN SUM(ChildcareTransaction.TransactionTotal) || ";
            query += "'0' ELSE SUM(ChildcareTransaction.TransactionTotal) END AS 'Total Charges', '$' || Family.FamilyTotal AS 'Current Due' ";
            query += "From Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ChildcareTransaction NATURAL JOIN Family ";
            query += "WHERE ChildcareTransaction.TransactionDate BETWEEN '" + start + "' AND '" + end + "' ";
            query += "GROUP BY Guardian.Guardian_ID";

            LoadReport(query);
        }

        private void LoadReport(String query) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);

                for (int i = 0; i < table.Rows.Count; i++) {
                    if (((String)table.Rows[i][9]).Split('.')[1].Length == 1) {
                        table.Rows[i][9] += "0";
                    }
                }
                for (int i = 0; i < table.Rows.Count; i++) {
                    if (((String)table.Rows[i][10]).Split('.')[1].Length == 1) {
                        table.Rows[i][10] += "0";
                    }
                }
                BusinessDataGrid.ItemsSource = table.DefaultView;

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
