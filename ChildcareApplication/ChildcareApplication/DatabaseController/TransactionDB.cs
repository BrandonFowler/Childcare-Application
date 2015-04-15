using System;
using System.Collections.Generic;
using System.Data;
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
            String query = "SELECT Allowance_ID FROM Guardian Natural Join AllowedConnections Join ";
            query += "Child ON AllowedConnections.Child_ID = Child.Child_ID Where Guardian.FirstName = '";
            query += guardianFirst + "' and Guardian.LastName = '" + guardianLast;
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

        //returns a time string in 12 hour clock form (AM/PM)
        public string GetTwelveHourTime(string transID, string fieldName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT " + fieldName + " FROM ChildcareTransaction where ChildcareTransaction_ID = '" + transID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            string time = "";

            try {
                connection.Open();
                time = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            time = AMPMFormat(time);
            return time;
        }

        //expects time to be of 'MM/DD/YYYY HH:MM:SS AM' format
        private string AMPMFormat(string time) {
            string[] splitTime = time.Split(' ');

            return splitTime[1] + " " + splitTime[2];
        }

        public string GetTransactionDate(string transID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT TransactionDate FROM ChildcareTransaction where ChildcareTransaction_ID = '" + transID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            string date = "";

            try {
                connection.Open();
                date = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            date = (date.Replace('-', '/')).Split(' ')[0];
            return ZeroFillDate(date);
        }

        private string ZeroFillDate(string date) {
            string[] splitDate = date.Split('/');

            if (splitDate[0].Length != 2) {
                splitDate[0] = "0" + splitDate[0];
            }
            if (splitDate[1].Length != 2) {
                splitDate[1] = "0" + splitDate[1];
            }
            return splitDate[0] + "/" + splitDate[1] + "/" + splitDate[2];
        }

        public DataTable GetTransactions() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = BuildTransactionsQuery();
            DataTable table;
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                table = new DataTable("Transactions");
                adapter.Fill(table);

                connection.Close();
                return table;
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
                return null;
            }
        }

        private string BuildTransactionsQuery() {
            string query = "SELECT Guardian.Guardian_ID AS 'Guardian ID', ChildcareTransaction.ChildcareTransaction_ID AS 'Transaction ID', ";
            query += "strftime('%m/%d/%Y', ChildcareTransaction.TransactionDate) AS Date, Guardian.FirstName AS First, Guardian.LastName AS Last, ";
            query += "Guardian.Phone, Guardian.Address1, Guardian.Address2, Guardian.City, Guardian.StateAbrv AS State, Guardian.Zip, ";
            query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS 'Total Charges' ";
            query += "From Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ChildcareTransaction NATURAL JOIN Family ";
            query += "ORDER BY ChildcareTransaction.TransactionDate;";

            return query;
        }
    }
}
