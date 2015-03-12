using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
            String fromDate = "2015-02-01";
            String toDate = "2015-02-28";

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
            query += "Guardian.Phone, Guardian.Address1, Guardian.Address2, Guardian.City, Guardian.State, Guardian.Zip, ";
            query += "INSERT(FORMAT(SUM(ChildCareTransaction.TransactionTotal), 2), 1, 0, '$') AS Total ";
            query += "From Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ChildCareTransaction ";
            query += "WHERE ChildCareTransaction.Date BETWEEN '" + start + "' AND '" + end + "' ";
            query += "GROUP BY Guardian.Guardian_ID";

            LoadReport(query);
        }

        private void LoadReport(String query) {
            MySqlConnection connection = new MySqlConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");

            try {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);
                BusinessDataGrid.ItemsSource = table.DefaultView;
                adapter.Update(table);

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
