using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBoxUtils;

namespace ChildcareApplication.DatabaseController {
    class ReportsDB {

        public DataTable GetBusinessReportTable(string startDate, string endDate) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            string query = BuildBusinessReportQuery(startDate, endDate);
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                DataTable table = new DataTable("Business Report");
                adapter.Fill(table);

                connection.Close();
                return table;
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                return null;
            }
        }

        private string BuildBusinessReportQuery(string startDate, string endDate) {

            string query = "SELECT Guardian.Guardian_ID AS ID, Guardian.FirstName AS 'First Name', Guardian.LastName AS 'Last Name', ";
            query += "ChildcareTransaction.EventName AS 'Event Name', ";
            query += "'$' || printf('%.2f', SUM(ChildcareTransaction.TransactionTotal)) AS 'Charges' ";
            query += "From Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ChildcareTransaction NATURAL JOIN Family ";
            query += "WHERE ChildcareTransaction.TransactionDate BETWEEN '" + startDate + "' AND '" + endDate + "' ";
            query += "GROUP BY Guardian.Guardian_ID, ChildcareTransaction.EventName ORDER BY Guardian.Guardian_ID;";

            return query;
        }

        public DataTable GetParentReportTable(string parentID, params string[] dates) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            DataTable table;
            string query = BuildParentReportQuery(parentID, dates);
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                table = new DataTable("Parent Report");
                adapter.Fill(table);

                connection.Close();
                return table;
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                return null;
            }
        }

        private string BuildParentReportQuery(string parentID, params string[] dates) {
            string query = "";
            if (dates != null && dates.Length == 2) {
                query += "SELECT strftime('%m-%d-%Y', ChildcareTransaction.TransactionDate) AS Date, Child.FirstName AS First, Child.LastName AS ";
                query += "Last, EventData.EventName AS 'Event Type', time(ChildcareTransaction.CheckedIn) AS 'Check In', ";
                query += "time(ChildcareTransaction.CheckedOut) AS 'Check Out', ";
                query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS Total FROM AllowedConnections NATURAL JOIN Child ";
                query += "NATURAL JOIN ChildcareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + parentID + " ";
                query += "AND ChildcareTransaction.TransactionDate BETWEEN '" + dates[0] + "' AND '" + dates[1] + "';";
            } else {
                query += "SELECT strftime('%m/%d/%Y', ChildcareTransaction.TransactionDate) AS 'Date', Child.FirstName AS First, Child.LastName AS ";
                query += "Last, EventData.EventName AS 'Event Type', time(ChildcareTransaction.CheckedIn) AS 'Check In', ";
                query += "time(ChildcareTransaction.CheckedOut) AS 'Check Out', ";
                query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS Total FROM AllowedConnections NATURAL JOIN Child ";
                query += "NATURAL JOIN ChildcareTransaction NATURAL JOIN EventData WHERE AllowedConnections.Guardian_ID = " + parentID + ";";
            }
            return query;
        }
    }
}
