using MessageBoxUtils;
using System;
using System.Data;
using System.Data.SQLite;

namespace DatabaseController {
    class TransactionDB {
        private SQLiteConnection dbCon;

        public TransactionDB() {
            this.dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }
        public bool TransactionExists(String transactionID) {
            int count = 0;
            try {
                dbCon.Open();
                String query = "SELECT count(*) FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            if (count == 0) {
                return false;
            } else {
                return true;
            }
        }

        public void DeleteTransaction(String transactionID) {
            try {
                dbCon.Open();
                String query = "DELETE FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
        }

        public String GetTransEventName(String transactionID) {
            String query = "SELECT EventName FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            String result = "";

            try {
                dbCon.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            return result;
        }

        public double GetTransactionTotal(String transactionID) {
            String query = "SELECT TransactionTotal FROM ChildcareTransaction WHERE ChildcareTransaction_ID = '" + transactionID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            double result = 0;

            try {
                dbCon.Open();
                result = Convert.ToDouble(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            return result;
        }

        public void UpdateTransaction(string transID, string eventName, string allowanceID, string transDate, string checkedIn, string checkedOut, string transTotal) {
            String query = "UPDATE ChildcareTransaction SET EventName = '" + eventName + "', Allowance_ID = '" + allowanceID + "', ";
            query += "TransactionDate = '" + transDate + "', CheckedIn = '" + checkedIn + "', CheckedOut = '" + checkedOut + "', ";
            query += "TransactionTotal = '" + transTotal + "' WHERE ChildcareTransaction_ID = '" + transID + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
        }

        public void NewTransaction(string transID, string eventName, string allowanceID, string transDate, string checkedIn, string checkedOut, string transTotal) {
            String query = "INSERT INTO ChildcareTransaction VALUES ('" + transID + "', '" + eventName + "', ";
            query += "'" + allowanceID + "', '" + transDate + "', '" + checkedIn + "', '" + checkedOut + "', ";
            query += "'" + transTotal + "')";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
        }

        public string GetNextTransID() {
            String query = "SELECT MAX(ChildcareTransaction_ID) FROM ChildcareTransaction;";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            string result = "";
            int num = 0;

            try {
                dbCon.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                if (!reader.IsDBNull(0)) {
                    num = Convert.ToInt32(reader.GetString(0));
                }
                num++;
                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }

            result = num.ToString("D10");
            return result;
        }

        //returns a time string in 12 hour clock form (AM/PM)
        public string GetTwelveHourTime(string transID, string fieldName) {
            String query = "SELECT " + fieldName + " FROM ChildcareTransaction where ChildcareTransaction_ID = '" + transID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            string time = "";

            try {
                dbCon.Open();
                time = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            time = AMPMFormat(time);
            return time;
        }

        //expects time to be of 'MM/DD/YYYY HH:MM:SS AM' format or 'MM/DD/YYYY HH:MM:SS'
        private string AMPMFormat(string time) {
            string[] splitDateTime = time.Split(' ');

            if (splitDateTime.Length == 3) {
                return splitDateTime[1] + " " + splitDateTime[2];
            } else {
                string[] splitTime = splitDateTime[1].Split(':');
                int hour = Convert.ToInt32(splitTime[0]);
                string formattedTime = "";
                if (hour > 12) {
                    formattedTime += (hour - 12) + ":" + splitTime[1] + ":" + splitTime[2] + " PM";
                } else {
                    formattedTime = splitDateTime[1] + " AM";
                }
                return formattedTime;
            }
        }

        public string GetTransactionDate(string transID) {
            String query = "SELECT TransactionDate FROM ChildcareTransaction where ChildcareTransaction_ID = '" + transID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            string date = "";

            try {
                dbCon.Open();
                date = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
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
            String query = BuildTransactionsQuery();
            DataTable table;
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                table = new DataTable("Transactions");
                adapter.Fill(table);

                dbCon.Close();
                return table;
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
                return null;
            }
        }

        private string BuildTransactionsQuery() {
            string query = "SELECT Guardian.Guardian_ID AS 'Guardian ID', ChildcareTransaction.ChildcareTransaction_ID AS 'Transaction ID', ";
            query += "strftime('%m/%d/%Y', ChildcareTransaction.TransactionDate) AS Date, Guardian.FirstName AS First, Guardian.LastName AS Last, ";
            query += "ChildcareTransaction.EventName AS 'Event', ";
            query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS 'Total Charges' ";
            query += "From Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ChildcareTransaction NATURAL JOIN Family ";
            query += "ORDER BY ChildcareTransaction.TransactionDate DESC;";

            return query;
        }

        public bool AddLateFee(string sMaxTransactionID, string eventName, string allowanceID, string currentDateString, double lateFee) {
            string sql = "insert into " +
                      "ChildcareTransaction (ChildcareTransaction_ID,EventName,Allowance_ID,transactionDate,CheckedIn,CheckedOut,TransactionTotal) " +
                      "values (@sMaxTransactionID, @eventName, @allowanceID, @currentDateString, '00:00:00','00:00:00', @lateFee)";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@sMaxTransactionID", sMaxTransactionID));
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            command.Parameters.Add(new SQLiteParameter("@currentDateString", currentDateString));
            command.Parameters.Add(new SQLiteParameter("@lateFee", lateFee));
            if (Convert.ToInt32(sMaxTransactionID) > -1) {
                try {
                    dbCon.Open();
                    command.ExecuteNonQuery();
                    dbCon.Close();
                } catch (System.Data.SQLite.SQLiteException) {
                    WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible. Did not record late fee.");
                    dbCon.Close();
                    return false;
                } catch (Exception) {
                    dbCon.Close();
                    WPFMessageBox.Show("Unable to record late fee");
                    return false;
                }
            } else {
                WPFMessageBox.Show("Database connection error: Unable to record late fee");
                return false;
            }
            return true;
        }

        public object SumRegularCare(string start, string end, string familyID) {
            string sql = "select sum(TransactionTotal) " +
                             "from AllowedConnections natural join ChildcareTransaction " +
                             "where Family_ID = @familyID and EventName IN ('Regular Childcare', 'Infant Childcare') and TransactionDate between '" + start + "' and '" + end + "'";//Add older child
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            object recordFound = null;
            try {
                dbCon.Open();
                recordFound = command.ExecuteScalar();
                dbCon.Close();
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to check if charge exceeds monthly maximum for normal care.");
            }
            return recordFound;
        }

        public void UpdateRegularBalance(string guardianID, double fee) {
            UpdateBalances(guardianID, fee, "RegularTotal");
        }

        public void UpdateCampBalance(string guardianID, double fee) {
            UpdateBalances(guardianID, fee, "CampTotal");
        }

        public void UpdateMiscBalance(string guardianID, double fee) {
            UpdateBalances(guardianID, fee, "MiscTotal");
        }

        public void UpdateBalances(string guardianID, double fee, string balanceType) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "update Family " +
                         "set " + balanceType + " = " + balanceType + " + @fee " +
                         "where Family_ID = @familyID;";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@fee", fee));
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible. Charge has not been added to balance.");
                dbCon.Close();
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to add charge to family balance.");
            }
        }

        public string GetIncompleteTransAllowanceID(string guardianID, string childID) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "select Allowance_ID " +
                         "from AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and Child_ID = @childID and CheckedOut is null " +
                         "Group By Allowance_ID " +
                         "HAVING COUNT(*) = 1 ";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            command.Parameters.Add(new SQLiteParameter("@childID", childID));
            try {
                dbCon.Open();
                string allowanceID = Convert.ToString(command.ExecuteScalar());
                dbCon.Close();
                return allowanceID;
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return null;
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve critical information.");
                return null;
            }
        }

        public string[] FindTransaction(string allowanceID) {
            string sql = "select * " +
                         "from ChildcareTransaction " +
                         "where Allowance_ID = @allowanceID and CheckedOut is null";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try {
                dbCon.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                string[] transaction = new string[cCount];
                for (int x = 0; x < cCount; x++) {
                    transaction[x] = DS.Tables[0].Rows[0][x].ToString();
                }
                dbCon.Close();
                return transaction;
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return null;
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve original transaction.");
                return null;
            }
        }

        public String GetParentNameFromTrans(String transactionID) {
            String result = "";

            try {
                dbCon.Open();

                String query = "SELECT FirstName, LastName FROM Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ";
                query += "ChildcareTransaction WHERE ChildcareTransaction.ChildcareTransaction_ID = '" + transactionID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                result = reader.GetString(0) + " " + reader.GetString(1);

                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            return result;
        }

        public String GetGuardianIDFromTrans(String transactionID) {
            String result = "";

            try {
                dbCon.Open();

                String query = "SELECT Guardian_ID FROM Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ";
                query += "ChildcareTransaction WHERE ChildcareTransaction.ChildcareTransaction_ID = '" + transactionID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                result = reader.GetString(0);

                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            return result;
        }

        public string[] GetUpdateTotalsDetails(string transactionID) {
            String query = "SELECT AllowedConnections.Guardian_ID, ChildcareTransaction.EventName, ";
            query += "ChildcareTransaction.TransactionTotal From ChildcareTransaction NATURAL JOIN AllowedConnections ";
            query += "Where ChildcareTransaction.ChildcareTransaction_ID = '" + transactionID + "';";
            string[] results = new string[3];

            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                SQLiteDataReader reader = cmd.ExecuteReader();

                reader.Read();
                results[0] = reader.GetString(0);
                results[1] = reader.GetString(1);
                double temp = reader.GetDouble(2);
                results[2] = temp.ToString();

                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                dbCon.Close();
            }
            return results;
        }
    }
}
