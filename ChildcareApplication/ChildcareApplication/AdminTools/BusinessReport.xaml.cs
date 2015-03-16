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
            String fromDate = "2015-03-01";
            String toDate = "2015-03-28";

            BuildQuery(fromDate, toDate);
        }

        private void btn_SpecificMonth_Click(object sender, RoutedEventArgs e) {
            GregorianCalendar cal = new GregorianCalendar();
            String month = ((ComboBoxItem)cmb_Month.SelectedItem).Content.ToString();
            String year = ((ComboBoxItem)cmb_Year.SelectedItem).Content.ToString();

            int monthNum = GetMonthNum(month);

            String fromDate = year + "-" + monthNum + "-1";
            String toDate = year + "-" + monthNum + "-";
            toDate += cal.GetDaysInMonth(Convert.ToInt32(year), monthNum);

            BuildQuery(fromDate, toDate);
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
    }
}
