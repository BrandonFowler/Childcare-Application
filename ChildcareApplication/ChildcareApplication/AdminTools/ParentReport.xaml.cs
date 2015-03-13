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

namespace AdminTools {
    /// <summary>
    /// Interaction logic for ParentReport.xaml
    /// </summary>
    public partial class ParentReport : Window {
        public ParentReport() {
            InitializeComponent();
            cnv_ParentIcon.Background = new SolidColorBrush(Colors.Aqua);
        }

        //Loads a report based on the passed in MySQL query
        private void LoadReport(String query) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildCare_v5.s3db;Version=3;");

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);
                ParentDataGrid.ItemsSource = table.DefaultView;
                adapter.Update(table);

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_LoadAll_Click(object sender, RoutedEventArgs e) {
            BuildQuery();
            LoadParentData();
        }

        private void btn_CurrentMonthReport_Click(object sender, RoutedEventArgs e) { //TODO: load for actual month
            String fromDate = "2015-02-01";
            String toDate = "2015-02-28";

            BuildQuery(fromDate, toDate);
            LoadParentData();
        }

        private void btn_DateRangeReport_Click(object sender, RoutedEventArgs e) { //TODO: error checking!
            String initialFrom = txt_FromDate.Text;
            String initalTo = txt_ToDate.Text;

            String[] fromParts = initialFrom.Split('/');
            String[] toParts = initalTo.Split('/');
            
            String fromDate = fromParts[2] + "-" + fromParts[0] + "-" + fromParts[1];
            String toDate = toParts[2] + "-" + toParts[0] + "-" + toParts[1];

            BuildQuery(fromDate, toDate);
            LoadParentData();
        }

        //builds a query based on passed in values
        private void BuildQuery(String start, String end) {
            string query = "SELECT (DATE_FORMAT(ChildCareTransaction.Date, '%m/%d/%Y')) AS Date, Child.FirstName AS First, Child.LastName AS ";
            query += "Last, EventData.EventName AS 'Event Type', DATE_FORMAT(ChildCareTransaction.CheckedIn, '%h:%i %p') AS 'Check In', ";
            query += "DATE_FORMAT(ChildCareTransaction.CheckedOut, '%h:%i %p') AS 'Check Out', ";
            query += "INSERT(FORMAT(ChildCareTransaction.TransactionTotal, 2), 1, 0, '$') AS Total FROM AllowedConnections NATURAL JOIN Child ";
            query += "NATURAL JOIN ChildCareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + txt_ParentID.Text + " ";
            query += "AND ChildCareTransaction.Date BETWEEN '" + start + "' AND '" + end + "';";

            LoadReport(query);
        }

        //builds a query string to show all charges
        private void BuildQuery() {
            string query = "SELECT (DATE_FORMAT(ChildCareTransaction.Date, '%m/%d/%Y')) AS Date, Child.FirstName AS First, Child.LastName AS ";
            query += "Last, EventData.EventName AS 'Event Type', DATE_FORMAT(ChildCareTransaction.CheckedIn, '%h:%i %p') AS 'Check In', ";
            query += "DATE_FORMAT(ChildCareTransaction.CheckedOut, '%h:%i %p') AS 'Check Out', ";
            query += "INSERT(FORMAT(ChildCareTransaction.TransactionTotal, 2), 1, 0, '$') AS Total FROM AllowedConnections NATURAL JOIN Child ";
            query += "NATURAL JOIN ChildCareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + txt_ParentID.Text + ";";

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
        }
    }
}
//going to need this query later so ill stash it here for now
//string query = "SELECT Child.FirstName, Child.LastName, EventData.EventName, ChildCareTransaction.CheckedIn, ChildCareTransaction.CheckedOut, ";
//                    query += "ChildCareTransaction.TransactionTotal FROM ((AllowedConnections NATURAL JOIN Child) JOIN Guardian ON Guardian.Guardian_ID = AllowedConnections.Guardian_ID) ";
//                    query += "NATURAL JOIN ChildCareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + txt_ParentID.Text + ";";