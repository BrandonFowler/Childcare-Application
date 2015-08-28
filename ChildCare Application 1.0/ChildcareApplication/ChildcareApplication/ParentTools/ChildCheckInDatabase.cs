using System.Data.SQLite;
using System;
using System.Data;
using System.Windows;

namespace ParentTools {

    class ChildCheckInDatabase {

        private SQLiteConnection conn;

        public ChildCheckInDatabase() {
            conn = new SQLiteConnection("Data Source=../../Database/ChildCare_v5.s3db;Version=3;");
        }//end Database(default constructor)

        public bool validateLogin(string ID, string PIN) {

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
        }//end validateLogin

        public string[] getParentInfo(String guardianID) {

            string sql = "select * " +
                         "from Guardian " +
                         "where Guardian_ID = @guardianID";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();

            try
            {
                conn.Open();
                DB.Fill(DS);

                int cCount = DS.Tables[0].Columns.Count;
                string[] data = new String[cCount];

                for (int x = 0; x < cCount; x++)
                {
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
        }// end getParentInfo

        public String[,] findChildren(string guardianID) {

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID "+
                  "where Guardian_ID = @guardianID and ChildDeletionDate is null";

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
        }//end findChildren

        public string[,] getEvents() {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string month = Convert.ToDateTime(dateTime).ToString("MM");
            string day = Convert.ToDateTime(dateTime).ToString("dd");
            string dayOfWeek = dt.DayOfWeek.ToString();

            string sql = "select Event_ID, EventName " + 
                         "from EventData " + 
                         "where EventDay= @day and EventMonth= @month or EventWeekday= @dayOfWeek or Event_ID='000002'";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            command.Parameters.Add(new SQLiteParameter("@day", day));
            command.Parameters.Add(new SQLiteParameter("@month", month));
            command.Parameters.Add(new SQLiteParameter("@dayOfWeek", dayOfWeek));
            DataSet DS = new DataSet();

            try{
                conn.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                int rCount = DS.Tables[0].Rows.Count;
                string[,] events = new string[rCount, cCount];
                for (int x = 0; x < rCount; x++){
                    for (int y = 0; y < cCount; y++){
                        events[x, y] = DS.Tables[0].Rows[x][y].ToString();
                    }
                }

                conn.Close();

                return events;
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to retrieve information for events");
                conn.Close();
                return null;
            }
        }//end getEvents

        public void checkIn(string childID, string eventID, string guardianID, string birthday) {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string date = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
            string time = Convert.ToDateTime(dateTime).ToString("HH:mm:ss");
            bool isInfant;
            if (Convert.ToInt32(eventID) == 2) {
                isInfant = checkInfant(birthday, date);
                if (isInfant) {
                    eventID = "000003";
                }
            }

            string allowanceID = getAllowanceID(guardianID, childID);
            int maxTransactionID = getNextPrimary("ChildcareTransaction_ID","ChildcareTransaction");

            string sql = "insert into " +
                         "ChildcareTransaction (ChildcareTransaction_ID,Event_ID,Allowance_ID,transactionDate,CheckedIn) " + 
                         "values (@maxTransactionID, @eventID, @allowanceID, @date, @time)";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@maxTransactionID", maxTransactionID));
            command.Parameters.Add(new SQLiteParameter("@eventID", eventID));
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
            }

        }//end checkIn

        private int getNextPrimary(string column, string table) {

            string sql = "Select max(cast("+column+" as unsigned)) from " + table;
            
            SQLiteCommand command = new SQLiteCommand(sql, conn);

            try
            {
                conn.Open();
                object max = command.ExecuteScalar();
                conn.Close();

                if (max != DBNull.Value && max != null){
                    conn.Close();
                    return Convert.ToInt32(max) + 1;
                }

                return 0;
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to retrieve critical information");
                conn.Close();
                return -1;
            }
        }//end getMaxPrimary

        public string getAllowanceID(string guardianID, string childID) {

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
        }//end getConnectionID

        public bool checkOut(string childID, string guardianID) {
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = DateTime.Now.ToString();
            string currentDateString = Convert.ToDateTime(dateTimeString).ToString("yyyy-MM-dd");
            string currentTimeString = Convert.ToDateTime(dateTimeString).ToString("HH:mm:ss");
            string allowanceID = getTransactionAllowanceID(guardianID, childID);
            string[] transaction = findTransaction(allowanceID);
            if (transaction == null){
                MessageBox.Show("Unable to check out child. Please log out then try again.");
                return false;
            }
            string transactionID = transaction[0];
            string eventID = transaction[1];
            string checkInTime = transaction[4];
            bool isLate = false;
            bool ishourly = checkIfHourly(eventID);
            double eventFee = findEventFee(guardianID, eventID);
            TimeSpan TimeSpanTime = TimeSpan.Parse(currentTimeString);
            checkInTime = Convert.ToDateTime(checkInTime).ToString("HH:mm:ss");
            TimeSpan TimeSpanCheckInTime = TimeSpan.Parse(checkInTime);
            double lateTime = checkIfPastClosing(currentDateTime.DayOfWeek.ToString(), TimeSpanTime);
            if(ishourly){
               double hourDifference = TimeSpanTime.Hours - TimeSpanCheckInTime.Hours;
               double minuteDifference = TimeSpanTime.Minutes - TimeSpanCheckInTime.Minutes;
               double totalCheckedInHours = hourDifference + (minuteDifference/60.0);
               if ((eventID.CompareTo("000002") == 0 || eventID.CompareTo("000003") == 0) && totalCheckedInHours > 3) {
                    double timeDifference = totalCheckedInHours - 3;
                    if (timeDifference > lateTime) {
                        lateTime = timeDifference;
                        totalCheckedInHours = 3;
                    }
                    else {
                        totalCheckedInHours = totalCheckedInHours - lateTime;
                    }
                    isLate = true;
               }
               else if (lateTime > 0) {
                   totalCheckedInHours = totalCheckedInHours - lateTime;
                   isLate = true;
               }
               
                eventFee = eventFee * totalCheckedInHours;
                eventFee = Math.Round(eventFee, 2, MidpointRounding.AwayFromZero);
            }
            else {
                if (lateTime > 0) {
                    isLate = true;
                }
            }
      
            eventFee = eventFee - billingCapCalc(eventID, guardianID, transaction[3], eventFee);
            string eventFeeRounded = eventFee.ToString("f2");

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

            addToBalance(guardianID, eventFee);

            if (isLate) {
                eventID = "000001";
                double lateFee = getLateFee(eventID);
                lateFee = lateFee * lateTime;

                int maxTransactionID = getNextPrimary("ChildcareTransaction_ID", "ChildcareTransaction");
                    
                sql = "insert into " +
                      "ChildcareTransaction (ChildcareTransaction_ID,Event_ID,Allowance_ID,transactionDate,CheckedIn,CheckedOut,TransactionTotal) " +
                      "values (@maxTransactionID, @eventID, @allowanceID, @currentDateString, '00:00:00','00:00:00', @lateFee)";

                command = new SQLiteCommand(sql, conn);
                command.Parameters.Add(new SQLiteParameter("@maxTransactionID", maxTransactionID));
                command.Parameters.Add(new SQLiteParameter("@eventID", eventID));
                command.Parameters.Add(new SQLiteParameter("@allowanceID", allowanceID));
                command.Parameters.Add(new SQLiteParameter("@currentDateString", currentDateString));
                command.Parameters.Add(new SQLiteParameter("@lateFee", lateFee));

                if (maxTransactionID > -1){
                    try{
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch(Exception){
                        conn.Close();
                        MessageBox.Show("Database connection error: Unable to record late fee");
                    }
                }
                else{
                    MessageBox.Show("Database connection error: Unable to record late fee");
                }

                addToBalance(guardianID, lateFee);
            }
            
            return true;
        }//end checkOut

        private double billingCapCalc(string eventID, string guardianID, string transactionDate, double eventFee) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            double cap = 100;
            int billingStart = 20;
            int billingEnd = 19;
            DateTime DTStart;
            DateTime DTEnd;
            if (DateTime.Now.Day > billingEnd) {
                DTStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingStart);
                int endMonth = DTStart.Month + 1;
                if (endMonth == 13) {
                    int endYear = DTStart.Year + 1;
                    DTEnd = new DateTime(endYear, 1, billingEnd);
                }
                else {
                    DTEnd = new DateTime(DateTime.Now.Year, endMonth, billingEnd);
                }
            }
            else {
                DTEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingEnd);
                int startMonth = DTEnd.Month - 1;
                if (startMonth == 0) {
                    int startYear = DTEnd.Year - 1;
                    DTStart = new DateTime(startYear, 12, billingStart);
                }
                else {
                    DTStart = new DateTime(DateTime.Now.Year, startMonth, billingStart);
                }
            }
           
            string start = DTStart.ToString("yyyy-MM-dd");
            string end = DTEnd.ToString("yyyy-MM-dd");

            if(eventID.CompareTo("000002") == 0 || eventID.CompareTo("000003") == 0 ){

                string sql = "select sum(TransactionTotal) " +
                             "from AllowedConnections natural join ChildcareTransaction " +
                             "where Family_ID = @familyID and Event_ID IN ('000002', '000003') and TransactionDate between '" + start + "' and '" + end + "'";

                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.Parameters.Add(new SQLiteParameter("@familyID", familyID));

                object recordFound = null;
                try{
                    conn.Open();
                    recordFound = command.ExecuteScalar();
                    conn.Close();
                }
                catch(Exception){
                    conn.Close();
                    MessageBox.Show("Database connection error: Unable to check if charge exceeds monthly maximum for normal care.");
                }

                double sum;
                if (recordFound == DBNull.Value || recordFound == null) {
                    return 0;
                }
                else {
                    sum = Convert.ToDouble(recordFound);
                }

                double total = sum + eventFee;

                double capdiff = total - cap;

                if (capdiff > 0 && capdiff < eventFee) {
                    return capdiff;
                }
                else if (capdiff >= eventFee) {
                    return eventFee;
                }
            }
            return 0.0;
        }//end billingCapReached

        private void addToBalance(string guardianID, double fee) {

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

        }//end addToBalance

        private string getTransactionAllowanceID(string guardianID, string childID) {
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
        }//end getTransactionAllowanceID

        public string getClosingTime(string dayOfWeek) {

            string sql = "select Closing " +
                         "from OperatingHours " +
                         "where OperatingWeekday = @dayOfWeek";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@dayOfWeek", dayOfWeek));

            try{
                conn.Open();
                string closingTime = Convert.ToString(command.ExecuteScalar());
                conn.Close();

                return closingTime;
            }
            catch (Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to retrieve critical information. Any late fees have not been recorded.");
                return null;
            }

        }// end getClosingTime

        public double getLateFee(string eventID) {

            string sql = "select HourlyPrice " +
                         "from EventData " +
                         "where Event_ID = @eventID";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@eventID", eventID));

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
        }//end getLateFee

        public string[] getEvent(string eventID) {

            string sql = "select * " +
                  "from EventData " +
                  "where Event_ID = @eventID";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@eventID", eventID));
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
        }//end getEvent

        public int numberOfCheckedIn(string guardianID) {

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
        }//end numberOfCheckedIn

        public string[] findTransaction(string allowanceID) {

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
                for (int x = 0; x < cCount; x++)
                {
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
        }//end FindTransaction

        public bool isCheckedIn(string childID, string guardianID){
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
                    conn.Close();
                    return true;
                }

                return false;
            }
            catch(Exception){
                conn.Close();
                MessageBox.Show("Database connection error: Unable to access find all children. Please log out, then try again.");
                return false;
            }
        }//end isCheckedIn

        public bool checkInfant(string birthday, string date) {
            DateTime DTBirthday = DateTime.Parse(birthday);
            DateTime DTDate = DateTime.Parse(date);
            TimeSpan difference = DTDate - DTBirthday;
            if (difference.Days < 1096) {
                return true;
            }
            return false;
        }//end checkInfant

        public double checkIfPastClosing(string dayOfWeek, TimeSpan time) {
            string closingTime = getClosingTime(dayOfWeek);
            closingTime = Convert.ToDateTime(closingTime).ToString("HH:mm:ss");
            TimeSpan TSClosingTime = TimeSpan.Parse(closingTime);

            double hourDifference = time.Hours - TSClosingTime.Hours;
            double minuteDifference = time.Minutes - TSClosingTime.Minutes;
            double hours = hourDifference + (minuteDifference / 60.0);
            if (hours < 0) {
                return 0;
            }
            return hours;
        }//end checkIfPastClosing

        public bool checkIfHourly(string eventID) {
            string[] eventData = getEvent(eventID);

            if (eventData == null){
                return false;
            }

            if (String.IsNullOrWhiteSpace(eventData[2])) {
                return false;
            }
            else {
                return true;
            }
        }//end checkIfHourly

        public double findEventFee(string guardianID, string eventID) {
            bool discount = false;
            int childrenCheckedIn = numberOfCheckedIn(guardianID);
            if (childrenCheckedIn > 1) {
                discount = true;
            }

            string[] eventData = getEvent(eventID);

            if (eventData == null)
            {
                return 0.0;
            }

            if (discount) {
                if (String.IsNullOrWhiteSpace(eventData[3])) {
                    return Convert.ToDouble(eventData[5]);
                }
                else {
                    return Convert.ToDouble(eventData[3]);
                }
            }
            else {
                if (String.IsNullOrWhiteSpace(eventData[2])) {
                    return Convert.ToDouble(eventData[4]);
                }
                else {
                    return Convert.ToDouble(eventData[2]);
                }
            }
        }//end findEventFee

    }//end Database(Class)
}
