using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatabaseController {
    class TransactionDB {
        public bool TransactionExists(String transactionID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            int count = 0;
            try {
                connection.Open();
                String query = "SELECT count(*) FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            if (count == 0) {
                return false;
            } else {
                return true;
            }
        }

        public void DeleteTransaction(String transactionID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            
            try {
                connection.Open();
                String query = "DELETE FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public String GetEventName(String transactionID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT EventName FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            String result = "";

            try {
                connection.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public double GetTransactionTotal(String transactionID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT TransactionTotal FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            double result = 0;

            try {
                connection.Open();
                result = Convert.ToDouble(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public void UpdateTransaction(string transID) {
            /*SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, EventWeekday = '" + weekday + "', HourlyPrice = null,";
            query += " HourlyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }*/
        }

        public void NewTransaction() {

        }
    }
}
