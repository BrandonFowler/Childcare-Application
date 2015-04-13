using System.Data.SQLite;
using System;
using System.Data;
using System.Windows;
using ChildcareApplication.Properties;

namespace DatabaseController {

    class ParentToolsDB {

        private SQLiteConnection conn;
        private ParentTools.ParentToolsSettings settings;

        public ParentToolsDB() {
            conn = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            settings = new ParentTools.ParentToolsSettings();
        }

        public bool ValidateLogin(string ID, string PIN) {
            string sql = "select Guardian_ID " +
                         "from Guardian " + 
                         "where Guardian_ID = @ID and GuardianPIN = @PIN";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
            command.Parameters.Add(new SQLiteParameter("@PIN", PIN));
            try{
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception){
                MessageBox.Show("Database Connection Failure");
                conn.Close();
            }
            return false;
        }

        public string[] GetParentInfo(String guardianID) {
            string sql = "select * " +
                         "from Guardian " +
                         "where Guardian_ID = @guardianID";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try{
                conn.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                string[] data = new String[cCount];
                for (int x = 0; x < cCount; x++){
                    data[x] = DS.Tables[0].Rows[0][x].ToString();
                }
                conn.Close();
                return data;
            }
            catch(Exception){
                MessageBox.Show("Database connection error: Unable to retrieve information for guardian");
                conn.Close();
                return null;
            }
        }

        public String[,] FindChildren(string guardianID) {
            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID "+
                  "where Guardian_ID = @guardianID and ChildDeletionDate is null and ConnectionDeletionDate is NULL";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            DataSet DS = new DataSet();
            try{
                conn.Open();
                DB.Fill(DS);
                if (DS.Tables == null){
                    return null;
                }
                int cCount = DS.Tables[0].Columns.Count;
                int rCount = DS.Tables[0].Rows.Count;
                String[,] data = new string[rCount, cCount];
                for (int x = 0; x < rCount; x++){
                    for (int y = 0; y < cCount; y++){
                        data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                    }
                }
                conn.Close();
                return data;
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to retrieve information for children");
                conn.Close();
                return null;
            }
        }

        public string[] GetEvents() {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string month = Convert.ToDateTime(dateTime).ToString("MM");
            string day = Convert.ToDateTime(dateTime).ToString("dd");
            string dayOfWeek = dt.DayOfWeek.ToString();
            string sql = "select EventName " + 
                         "from EventData " + 
                         "where ((EventDay= @day and EventMonth= @month or " +
                         "EventWeekday= @dayOfWeek or EventName='Regular Childcare') " + 
                         "and EventDeletionDate is NULL) or (EventDay is NULL and EventMonth is NULL " + 
                         "and EventWeekday is NULL and EventDeletionDate is NULL and " +
                         "EventName != 'Late Fee' and EventName != 'Infant Childcare' and EventName != 'Adolescent Childcare')";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            command.Parameters.Add(new SQLiteParameter("@day", day));
            command.Parameters.Add(new SQLiteParameter("@month", month));
            command.Parameters.Add(new SQLiteParameter("@dayOfWeek", dayOfWeek));
            DataSet DS = new DataSet();
            try{
                conn.Open();
                DB.Fill(DS);
                int rCount = DS.Tables[0].Rows.Count;
                string[] events = new string[rCount];
                for (int x = 0; x < rCount; x++){      
                    events[x] = DS.Tables[0].Rows[x][0].ToString(); 
                }
                conn.Close();
                return events;
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to retrieve information for events");
                conn.Close();
                return null;
            }
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
            int maxTransactionID = GetNextPrimary("ChildcareTransaction_ID","ChildcareTransaction");
            string transactionID = Convert.ToString(maxTransactionID);
            transactionID = transactionID.ToString().PadLeft(10, '0');
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

        public int GetNextPrimary(string column, string table) {
            string sql = "Select max(cast("+column+" as unsigned)) from " + table;
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try{
                conn.Open();
                object max = command.ExecuteScalar();
                conn.Close();
                if (max != DBNull.Value && max != null){
                    conn.Close();
                    int maxID = Convert.ToInt32(max) + 1;
                    if(maxID == 2147483647){
                        maxID = 1;
                    }
                    return maxID;
                }
                return 1;
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to retrieve critical information");
                conn.Close();
                return -1;
            }
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

        public bool AddLateFee(string sMaxTransactionID, string eventName, string allowanceID, string currentDateString, double lateFee) {
            string sql = "insert into " +
                      "ChildcareTransaction (ChildcareTransaction_ID,EventName,Allowance_ID,transactionDate,CheckedIn,CheckedOut,TransactionTotal) " +
                      "values (@sMaxTransactionID, @eventName, @allowanceID, @currentDateString, '00:00:00','00:00:00', @lateFee)";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@sMaxTransactionID", sMaxTransactionID));
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            command.Parameters.Add(new SQLiteParameter("@currentDateString", currentDateString));
            command.Parameters.Add(new SQLiteParameter("@lateFee", lateFee));
            if (Convert.ToInt32(sMaxTransactionID) > -1){
                try{
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch(Exception){
                    conn.Close();
                    MessageBox.Show("Database connection error: Unable to record late fee");
                    return false;
                }
            }
            else{
                MessageBox.Show("Database connection error: Unable to record late fee");
                return false;
            }
            return true;
        }

        public object SumRegularCare(string start, string end, string familyID) {
            string sql = "select sum(TransactionTotal) " +
                             "from AllowedConnections natural join ChildcareTransaction " +
                             "where Family_ID = @familyID and EventName IN ('Regular Childcare', 'Infant Childcare') and TransactionDate between '" + start + "' and '" + end + "'";//Add older child
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            object recordFound = null;
            try {
                conn.Open();
                recordFound = command.ExecuteScalar();
                conn.Close();
            }
            catch (Exception) {
                conn.Close();
                MessageBox.Show("Database connection error: Unable to check if charge exceeds monthly maximum for normal care.");
            }
            return recordFound;
        }

        public void AddToBalance(string guardianID, double fee) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "update Family " +
                         "set FamilyTotal = FamilyTotal + @fee " +
                         "where Family_ID = @familyID";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@fee", fee));
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            try{
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to add charge to family balance.");
            }
        }

        public string GetTransactionAllowanceID(string guardianID, string childID) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            string sql = "select Allowance_ID " +
                         "from AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and Child_ID = @childID and CheckedOut is null " +
                         "Group By Allowance_ID " +
                         "HAVING COUNT(*) = 1 ";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
            command.Parameters.Add(new SQLiteParameter("@childID", childID));
            try{
                conn.Open();
                string allowanceID = Convert.ToString(command.ExecuteScalar());
                conn.Close();
                return allowanceID;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information.");
                return null;
            }
        }

        public double GetLateFee(string eventName) {
            string sql = "select HourlyPrice " +
                         "from EventData " +
                         "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            try{
                conn.Open();
                string fee = Convert.ToString(command.ExecuteScalar());
                conn.Close();

                double lateFee = Convert.ToDouble(fee);
                return lateFee;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information. Any late fees have not been recorded.");
                return 0;
            }
        }

        public string[] GetEvent(string eventName) {
            string sql = "select * " +
                  "from EventData " +
                  "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try{
                conn.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                string[] eventData = new string[cCount];
                for (int x = 0; x < cCount; x++){
                    eventData[x] = DS.Tables[0].Rows[0][x].ToString();
                }
                conn.Close();
                return eventData;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information. Please insure charge was calculated correctly");
                return null;
            }
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

        public string[] FindTransaction(string allowanceID) {
            string sql = "select * " +
                         "from ChildcareTransaction "+
                         "where Allowance_ID = @allowanceID and CheckedOut is null";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try{
                conn.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                string[] transaction = new string[cCount];
                for (int x = 0; x < cCount; x++){
                    transaction[x] = DS.Tables[0].Rows[0][x].ToString();
                }
                conn.Close();
                return transaction;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve original transaction.");
                return null;
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

        public double GetEventHourCap(string eventName) {
            String sql = "select EventMaximumHours " +
                         "from EventData " +
                         "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            double eventHasNoMax = 24;
            try {
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    return Convert.ToDouble(recordFound);
                }
                return eventHasNoMax;
            }
            catch (Exception) {
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve event specifications. Possible late fee calculations could be incorrect.");
                return eventHasNoMax;
            }
        }

        public string[,] RetieveGuardiansByLastName(string name) {
            string sql = "select FirstName, LastName, Guardian_ID " +
                         "from Guardian " +
                         "where LastName = @name";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@name", name));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try {
                conn.Open();
                DB.Fill(DS);
                if (DS.Tables == null) {
                    return null;
                }
                int cCount = DS.Tables[0].Columns.Count;
                int rCount = DS.Tables[0].Rows.Count;
                String[,] data = new string[rCount, cCount];
                for (int x = 0; x < rCount; x++) {
                    for (int y = 0; y < cCount; y++) {
                        data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                    }
                }
                conn.Close();
                return data;
            }
            catch (Exception) {
                MessageBox.Show("Database connection error: Unable to retrieve information for guardians");
                conn.Close();
                return null;
            }
        }

        public bool ValidateGuardianID(string ID) {
            String sql = "select Guardian_ID " +
                         "from Guardian " +
                         "where Guardian_ID = @ID";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
            try {
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    return true;
                }
                return false;
            }
            catch (Exception) {
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve settings data, child age group may be calculated incorrectly.");
                return false;
            }
        }

        public string GetGuardianImagePath(string ID) {
            String sql = "select PhotoLocation " +
                        "from Guardian " +
                        "where Guardian_ID = @ID";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
            try {
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    return (string)recordFound;
                }
                return null;
            }
            catch (Exception) {
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve guardian picture.");
                return null;
            }
        }

    }
}
