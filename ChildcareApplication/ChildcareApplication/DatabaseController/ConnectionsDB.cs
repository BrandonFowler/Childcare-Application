using System.Data.SQLite;
using System;
using System.Data;
using System.Windows;
using ChildcareApplication.Properties;

namespace DatabaseController {

    class ConnectionsDB {
        private SQLiteConnection conn;
        private GuardianTools.GuardianToolsSettings settings;
        private TransactionDB transDB = new TransactionDB();

        public ConnectionsDB() {
            conn = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
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
            string ageGroup;
            if (eventName.CompareTo("Regular Childcare") == 0) {
                ageGroup = settings.CheckAgeGroup(birthday, date);
                if (ageGroup.CompareTo("Infant") == 0) {
                    eventName = "Infant Childcare";
                }
                else if (ageGroup.CompareTo("Adolescent") == 0) {
                    eventName = "Adolescent Childcare";
                }
            }
            string allowanceID = GetAllowanceID(guardianID, childID);
            string transactionID = this.transDB.GetNextTransID();
            string sql = "insert into " +
                         "ChildcareTransaction (ChildcareTransaction_ID,EventName,Allowance_ID,transactionDate,CheckedIn) " + 
                         "values (@transactionID, @eventName, @allowanceID, @date, @time)";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@transactionID", transactionID));
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            command.Parameters.Add(new SQLiteParameter("@date", date));
            command.Parameters.Add(new SQLiteParameter("@time", time));
            try{
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception){
                MessageBox.Show("Database Connection Error: Unable Check In Child");
                conn.Close();
                return false;
            }
            return true;
        }

        public string GetAllowanceID(string guardianID, string childID) {
            string sql = "select Allowance_ID " +
                         "from AllowedConnections " +
                         "where Guardian_ID = @guardianID and Child_ID = @childID";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            command.Parameters.Add(new SQLiteParameter("@childID", childID));
            try{
                conn.Open();
                string connectionID = Convert.ToString(command.ExecuteScalar());
                conn.Close();

                return connectionID;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information");
                return null;
            }
        }

        public bool CheckOut(string currentTimeString, string eventFeeRounded, string allowanceID) {
            string sql = "update ChildcareTransaction " +
                  "set CheckedOut= @currentTimeString, TransactionTotal = @eventFee "+
                  "where Allowance_ID = @allowanceID and CheckedOut is null";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@currentTimeString", currentTimeString));
            command.Parameters.Add(new SQLiteParameter("@eventFee", eventFeeRounded));
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            try{
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception){
                MessageBox.Show("Database connection error: Unable to check out child");
                conn.Close();
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
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try{
                conn.Open();
                DB.Fill(DS);
                int count = DS.Tables[0].Rows.Count;
                conn.Close();

                return count;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information. Please insure charge was calculated correctly");
                return 0;
            }
        }

        public bool IsCheckedIn(string childID, string guardianID){
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "select ChildcareTransaction_ID " +
                         "from Child natural join AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and Child_ID = @childID and CheckedOut is null ";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            command.Parameters.Add(new SQLiteParameter("@childID", childID));
            try{
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();

                if (recordFound != DBNull.Value && recordFound != null)
                {
                    return true;
                }
                return false;
            }
            catch(Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to access find all children. Please log out, then try again.");
                return false;
            }
        }
    }
}
