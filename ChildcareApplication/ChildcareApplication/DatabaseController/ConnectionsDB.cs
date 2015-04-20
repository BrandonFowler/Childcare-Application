﻿using System.Data.SQLite;
using System;
using System.Data;
using System.Windows;
using ChildcareApplication.Properties;

namespace DatabaseController {

    class ConnectionsDB {
        private SQLiteConnection dbCon;
        private GuardianTools.GuardianToolsSettings settings;
        private TransactionDB transDB = new TransactionDB();

        public ConnectionsDB() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            settings = new GuardianTools.GuardianToolsSettings();
        }

        public bool CheckIn(string childID, string eventName, string guardianID, string birthday) {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string date = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
            string time = Convert.ToDateTime(dateTime).ToString("HH:mm:ss");
            TimeSpan TSTime = TimeSpan.Parse(time);
            if (settings.CheckIfPastClosing(dt.DayOfWeek.ToString(), TSTime) > 0){
                MessageBox.Show("Cannot check in a child after normal operating hours");
                return false;
            }
            eventName = GetAgeGroup(eventName, birthday, date);
            string allowanceID = GetAllowanceIDOnID(guardianID, childID);
            string transactionID = this.transDB.GetNextTransID();
            string sql = "insert into " +
                         "ChildcareTransaction (ChildcareTransaction_ID,EventName,Allowance_ID,transactionDate,CheckedIn) " + 
                         "values (@transactionID, @eventName, @allowanceID, @date, @time)";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@transactionID", transactionID));
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            command.Parameters.Add(new SQLiteParameter("@date", date));
            command.Parameters.Add(new SQLiteParameter("@time", time));
            try{
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            }
            catch (Exception){
                MessageBox.Show("Database Connection Error: Unable Check In Child");
                dbCon.Close();
                return false;
            }
            return true;
        }

        private String GetAgeGroup(string eventName, string birthday, string date) {
            if (eventName.CompareTo("Regular Childcare") == 0) {
                String ageGroup = settings.CheckAgeGroup(birthday, date);
                if (ageGroup.CompareTo("Infant") == 0) {
                    eventName = "Infant Childcare";
                } else if (ageGroup.CompareTo("Adolescent") == 0) {
                    eventName = "Adolescent Childcare";
                }
            }
            return eventName;
        }

        public string GetAllowanceIDOnID(string guardianID, string childID) {
            string sql = "select Allowance_ID " +
                         "from AllowedConnections " +
                         "where Guardian_ID = @guardianID and Child_ID = @childID";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            command.Parameters.Add(new SQLiteParameter("@childID", childID));
            try{
                dbCon.Open();
                string connectionID = Convert.ToString(command.ExecuteScalar());
                dbCon.Close();

                return connectionID;
            }
            catch (Exception){
                dbCon.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information");
                return null;
            }
        }

        public bool CheckOut(string currentTimeString, string eventFeeRounded, string allowanceID) {
            string sql = "update ChildcareTransaction " +
                  "set CheckedOut= @currentTimeString, TransactionTotal = @eventFee "+
                  "where Allowance_ID = @allowanceID and CheckedOut is null";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@currentTimeString", currentTimeString));
            command.Parameters.Add(new SQLiteParameter("@eventFee", eventFeeRounded));
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            try{
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            }
            catch(Exception){
                MessageBox.Show("Database connection error: Unable to check out child");
                dbCon.Close();
                return false;
            }
            return true;
        }

        public int NumberOfCheckedIn(string guardianID) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "select ChildcareTransaction_ID " +
                         "from AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and CheckedOut is null " +
                         "Group By ChildcareTransaction_ID " +
                         "HAVING COUNT(*) = 1 ";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try{
                dbCon.Open();
                DB.Fill(DS);
                int count = DS.Tables[0].Rows.Count;
                dbCon.Close();

                return count;
            }
            catch (Exception){
                dbCon.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information. Please insure charge was calculated correctly");
                return 0;
            }
        }

        public bool IsCheckedIn(string childID, string guardianID){
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "select ChildcareTransaction_ID " +
                         "from Child natural join AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and Child_ID = @childID and CheckedOut is null ";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            command.Parameters.Add(new SQLiteParameter("@childID", childID));
            try{
                dbCon.Open();
                object recordFound = command.ExecuteScalar();
                dbCon.Close();

                if (recordFound != DBNull.Value && recordFound != null)
                {
                    return true;
                }
                return false;
            }
            catch(Exception){
                dbCon.Close();
                MessageBox.Show("Database connection error: Unable to access find all children. Please log out, then try again.");
                return false;
            }
        }

        public void UpdateAllowedConnections(string conID, string pID, string cID, string famID) {

            try {
                dbCon.Open();
                if (!ConnectionExists(pID)) {
                    string sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                        + "VALUES(" + conID + ", " + pID + ", " + cID + ", " + famID + ");";

                    SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                    command.CommandText = sql;

                    command.ExecuteNonQuery();
                    MessageBox.Show("Link Completed.");
                } else {
                    MessageBox.Show("There is already a link to this child and the guardian.");
                }
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }

        private bool ConnectionExists(string pID) {
            try {
                dbCon.Open();
                string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where Guardian_ID = " + pID + " AND ConnectionDeletionDate IS null";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

                DataSet DS = new DataSet();
                DB.Fill(DS);
                int count = DS.Tables[0].Rows.Count;
                if (count > 0) {
                    return false;
                }
                return true;
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
            return false;
        }

        public void DeleteAllowedConnection(string childID, string pID) {
            try {
                dbCon.Open();
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = @"UPDATE AllowedCOnnections SET ConnectionDeletionDate = @today WHERE Child_ID = @childID AND Guardian_ID = @pID;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@childID", childID));
                command.Parameters.Add(new SQLiteParameter("@pID", pID));
                command.ExecuteNonQuery();

                MessageBox.Show("Completed");
                dbCon.Close();
            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
        }

        public string GetAllowanceIDOnNames(string fullGuardianName, string fullChildName) {
            string[] splitGuardianName = fullGuardianName.Split(' ');
            string[] splitChildName = fullChildName.Split(' ');
            string guardianFirst = splitGuardianName[0];
            string guardianLast = splitGuardianName[1];
            string childFirst = splitChildName[0];
            string childLast = splitChildName[1];
            string allowanceID = "";

            String query = "SELECT Allowance_ID FROM Guardian Natural Join AllowedConnections Join ";
            query += "Child ON AllowedConnections.Child_ID = Child.Child_ID Where Guardian.FirstName = '";
            query += guardianFirst + "' and Guardian.LastName = '" + guardianLast;
            query += "' and Child.FirstName = '" + childFirst + "' and Child.LastName = '" + childLast + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

            try {
                dbCon.Open();
                allowanceID = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }

            return allowanceID;
        }
    }
}