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

        public void UpdateTransaction(string transID, string eventName, string allowanceID, string transDate, string checkedIn, string checkedOut, string transTotal) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE ChildcareTransaction SET EventName = '" + eventName + "', Allowance_ID = '" + allowanceID + "', ";
            query += "TransactionDate = '" + transDate + "', CheckedIn = '" + checkedIn + "', CheckedOut = '" + checkedOut + "', ";
            query += "TransactionTotal = '" + transTotal + "' WHERE ChildcareTransaction_ID = '" + transID + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void NewTransaction(string transID, string eventName, string allowanceID, string transDate, string checkedIn, string checkedOut, string transTotal) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO ChildcareTransaction VALUES ('" + transID + "', '" + eventName + "', ";
            query += "'" + allowanceID + "', '" + transDate + "', '" + checkedIn + "', '" + checkedOut + "', ";
            query += "'" + transTotal + "')";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public string GetNextTransID() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT MAX(ChildcareTransaction_ID) FROM ChildcareTransaction;";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            string result = "";
            int num = 0;

            try {
                connection.Open();
                num = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            String.Format("{0:0000000000}", result);
            return result;
        }

        public string GetAllowanceID(string fullGuardianName, string fullChildName) {
            string[] splitGuardianName = fullGuardianName.Split(' ');
            string[] splitChildName = fullChildName.Split(' ');
            string guardianFirst = splitGuardianName[0];
            string guardianLast = splitGuardianName[1];
            string childFirst = splitChildName[0];
            string childLast = splitChildName[1];
            string allowanceID = "";

            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT Allowance_ID FROM Guardian Natural Join AllowedConnections Natural Join ";
            query += "Child Where Guardian.FirstName = '" + guardianFirst + "' and Guardian.LastName = '" + guardianLast;
            query += "' and Child.FirstName = '" + childFirst + "' and Child.LastName = '" + childLast + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            try {
                connection.Open();
                allowanceID = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }

            return allowanceID;
        }
    }
}
