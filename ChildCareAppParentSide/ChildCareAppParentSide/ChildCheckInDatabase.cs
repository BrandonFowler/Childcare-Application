using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace ChildCareAppParentSide {

    class ChildCheckInDatabase {

        private MySql.Data.MySqlClient.MySqlConnection conn;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;

        public ChildCheckInDatabase() {
            this.server = "146.187.135.22";
            this.port = "3306";
            this.database = "childcare_v5";
            this.UID = "ccdev";
            this.password = "devpw821";
            connectionString = "SERVER="+server+"; PORT="+port+"; DATABASE="+database+"; UID="+UID+"; PASSWORD="+password+";";
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionString;
        }//end Database(default constructor)

        public ChildCheckInDatabase(string server, string port, string database, string UID, string password){
            this.server = server;
            this.port = port;
            this.database = database;
            this.UID = UID;
            this.password = password;
            connectionString = server + "; PORT=" + port + "; DATABASE=" + database + "; UID=" + UID + "; PASSWORD=" + password + ";";
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionString;
        }//end Database

        public bool validateLogin(string ID, string PIN) {

            string sql = "select Guardian_ID " +
                         "from Guardian " + 
                         "where Guardian_ID = @ID and GuardianPIN = @PIN";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@ID", ID));
            command.Parameters.Add(new MySqlParameter("@PIN", PIN));

            conn.Open();
            object recordFound = command.ExecuteScalar();
            conn.Close();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            return false;
        }//end validateLogin

        public string[] getParentInfo(String guardianID) {

            string sql = "select * " +
                         "from Guardian " +
                         "where Guardian_ID = @guardianID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);

            int cCount = DS.Tables[0].Columns.Count;
            string[] data = new String[cCount];

            for (int x = 0; x < cCount; x++) {
                data[x] = DS.Tables[0].Rows[0][x].ToString();
            }

            conn.Close();

            return data;
        }// end getParentInfo

        public String[,] findChildren(string guardianID) {

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID "+
                  "where Guardian_ID = @guardianID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);

            if (DS.Tables == null) {
                return null;
            }

            int cCount = DS.Tables[0].Columns.Count;
            int rCount = DS.Tables[0].Rows.Count;
            String[,] data = new string[rCount,cCount];
           
            for(int x = 0; x < rCount; x++) {
                for(int y = 0; y < cCount; y++) {
                    data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                }
            }

            conn.Close();

            return data;
        }//end findChildren

        public string[,] getEvents() {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string month = Convert.ToDateTime(dateTime).ToString("MM");
            string day = Convert.ToDateTime(dateTime).ToString("dd");
            string dayOfWeek = dt.DayOfWeek.ToString();

            string sql = "select Event_ID, EventName " + 
                         "from EventData " + 
                         "where EventDay= @day and EventMonth= @month or EventWeekday= @dayOfWeek or Event_ID='2'";

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            command.Parameters.Add(new MySqlParameter("@day", day));
            command.Parameters.Add(new MySqlParameter("@month", month));
            command.Parameters.Add(new MySqlParameter("@dayOfWeek", dayOfWeek));
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);
            int cCount = DS.Tables[0].Columns.Count;
            int rCount = DS.Tables[0].Rows.Count;
            string[,] events = new string[rCount, cCount];
            for (int x = 0; x < rCount; x++) {
                for (int y = 0; y < cCount; y++) {
                    events[x, y] = DS.Tables[0].Rows[x][y].ToString();
                }
            }

            conn.Close();

            return events;
        }//end getEvents

        public bool checkIn(string childID, string eventID, string guardianID, string birthday) {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string date = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
            string time = Convert.ToDateTime(dateTime).ToString("HH:mm:ss");
            bool isInfant;
            if (Convert.ToInt32(eventID) == 2) {
                isInfant = checkInfant(birthday, date);
                if (isInfant) {
                    eventID = "3";
                }
            }

            string allowanceID = getAllowanceID(guardianID, childID);
            int maxTransactionID = getNextPrimary("ChildcareTransaction_ID","ChildcareTransaction");

            string sql = "insert into " +
                         "ChildcareTransaction (ChildcareTransaction_ID,Event_ID,Allowance_ID,transactionDate,CheckedIn) " + 
                         "values (@maxTransactionID, @eventID, @allowanceID, @date, @time)";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@maxTransactionID", maxTransactionID));
            command.Parameters.Add(new MySqlParameter("@eventID", eventID));
            command.Parameters.Add(new MySqlParameter("@allowanceID", allowanceID));
            command.Parameters.Add(new MySqlParameter("@date", date));
            command.Parameters.Add(new MySqlParameter("@time", time));

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

            return true;
        }//end checkIn

        private int getNextPrimary(string column, string table) {

            string sql = "Select Max(" + column + ") from " + table;
            
            MySqlCommand command = new MySqlCommand(sql, conn);

            conn.Open();
            object max = command.ExecuteScalar();
            conn.Close();

            if (max != DBNull.Value && max != null) {
                conn.Close();
                return Convert.ToInt32(max) + 1;
            }
            
            return 0;
        }//end getMaxPrimary

        public string getAllowanceID(string guardianID, string childID) {

            string sql = "select Allowance_ID " +
                         "from AllowedConnections " +
                         "where Guardian_ID = @guardianID and Child_ID = @childID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));
            command.Parameters.Add(new MySqlParameter("@childID", childID));

            conn.Open();
            string connectionID = Convert.ToString(command.ExecuteScalar());
            conn.Close();

            return connectionID;
        }//end getConnectionID

        public bool checkOut(string childID, string guardianID) {
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = DateTime.Now.ToString();
            string currentDateString = Convert.ToDateTime(dateTimeString).ToString("yyyy-MM-dd");
            string currentTimeString = Convert.ToDateTime(dateTimeString).ToString("HH:mm:ss");
            string allowanceID = getTransactionAllowanceID(guardianID, childID);
            string[] transaction = findTransaction(allowanceID);
            string transactionID = transaction[0];
            string eventID = transaction[1];
            string checkInTime = transaction[4];
            bool isLate = false;
            bool ishourly = checkIfHourly(eventID);
            double eventFee = findEventFee(guardianID, eventID);
            TimeSpan TimeSpanTime = TimeSpan.Parse(currentTimeString);
            TimeSpan TimeSpanCheckInTime = TimeSpan.Parse(checkInTime);
            double lateTime = checkIfPastClosing(currentDateTime.DayOfWeek.ToString(), TimeSpanTime);
            if(ishourly){
               double hourDifference = TimeSpanTime.Hours - TimeSpanCheckInTime.Hours;
               double minuteDifference = TimeSpanTime.Minutes - TimeSpanCheckInTime.Minutes;
               double totalCheckedInHours = hourDifference + (minuteDifference/60.0);
               if ((eventID.CompareTo("2") == 0 || eventID.CompareTo("3") == 0) && totalCheckedInHours > 3) {
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
      
            eventFee = eventFee - billingCapCalc(eventID, childID, transaction[3], eventFee);

            string sql = "update ChildcareTransaction " +
                  "set CheckedOut= @currentTimeString, TransactionTotal = @eventFee "+
                  "where Allowance_ID = @allowanceID and CheckedOut is null";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@currentTimeString", currentTimeString));
            command.Parameters.Add(new MySqlParameter("@eventFee", eventFee));
            command.Parameters.Add(new MySqlParameter("@allowanceID", allowanceID));

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

            addToBalance(guardianID, eventFee);

            if (isLate) {
                eventID = "1";
                double lateFee = getLateFee(eventID);
                lateFee = lateFee * lateTime;

                int maxTransactionID = getNextPrimary("ChildcareTransaction_ID", "ChildcareTransaction");
                    
                sql = "insert into " +
                      "ChildcareTransaction (ChildcareTransaction_ID,Event_ID,Allowance_ID,transactionDate,CheckedIn,CheckedOut,TransactionTotal) " +
                      "values (@maxTransactionID, @eventID, @allowanceID, @currentDateString, '00:00:00','00:00:00', @lateFee)";

                command = new MySqlCommand(sql, conn);
                command.Parameters.Add(new MySqlParameter("@maxTransactionID", maxTransactionID));
                command.Parameters.Add(new MySqlParameter("@eventID", eventID));
                command.Parameters.Add(new MySqlParameter("@allowanceID", allowanceID));
                command.Parameters.Add(new MySqlParameter("@currentDateString", currentDateString));
                command.Parameters.Add(new MySqlParameter("@lateFee", lateFee));

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                addToBalance(guardianID, lateFee);
            }
            
            return true;
        }//end checkOut

        private double billingCapCalc(string eventID, string childID, string transactionDate, double eventFee) {
            double cap = 100.0;
            DateTime DTStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime DTEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInMonth);
            string start = DTStart.ToString("yyyyMMdd");
            string end = DTEnd.ToString("yyyyMMdd");

            if(eventID.CompareTo("2") == 0 || eventID.CompareTo("3") == 0 ){

                string sql = "select sum(TransactionTotal) " +
                             "from AllowedConnections natural join ChildcareTransaction natural join Child " +
                             "where Child_ID = @childID and TransactionDate between " + start + " and " + end;

                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.Add(new MySqlParameter("@childID", childID));

                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();

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

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@fee", fee));
            command.Parameters.Add(new MySqlParameter("@familyID", familyID));

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();

        }//end addToBalance

        private string getTransactionAllowanceID(string guardianID, string childID) {
            string familyID = guardianID.Remove(guardianID.Length - 1);

            string sql = "select Allowance_ID " +
                         "from AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and Child_ID = @childID and CheckedOut is null " +
                         "Group By Allowance_ID " +
                         "HAVING COUNT(*) = 1 ";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@familyID", familyID));
            command.Parameters.Add(new MySqlParameter("@childID", childID));

            conn.Open();
            string allowanceID = Convert.ToString(command.ExecuteScalar());
            conn.Close();

            return allowanceID;
        }//end getTransactionAllowanceID

        public string getClosingTime(string dayOfWeek) {

            string sql = "select Closing " +
                         "from OperatingHours " +
                         "where OperatingWeekday = @dayOfWeek";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@dayOfWeek", dayOfWeek));

            conn.Open();
            string closingTime = Convert.ToString(command.ExecuteScalar());
            conn.Close();

            return closingTime;
        }// end getClosingTime

        public double getLateFee(string eventID) {

            string sql = "select HourlyPrice " +
                         "from EventData " +
                         "where Event_ID = @eventID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@eventID", eventID));

            conn.Open();
            string fee = Convert.ToString(command.ExecuteScalar());
            conn.Close();

            double lateFee = Convert.ToDouble(fee);
            return lateFee;
        }//end getLateFee

        public string[] getEvent(string eventID) {

            string sql = "select * " +
                  "from EventData " +
                  "where Event_ID = @eventID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@eventID", eventID));
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);

            int cCount = DS.Tables[0].Columns.Count;
            string[] eventData = new string[cCount];
            for (int x = 0; x < cCount; x++) {
                eventData[x] = DS.Tables[0].Rows[0][x].ToString();
            }
            conn.Close();

            return eventData;
        }//end getEvent

        public int numberOfCheckedIn(string guardianID) {

            string familyID = guardianID.Remove(guardianID.Length - 1);

            string sql = "select ChildcareTransaction_ID " +
                         "from AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and CheckedOut is null " +
                         "Group By ChildcareTransaction_ID " +
                         "HAVING COUNT(*) = 1 ";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@familyID", familyID));
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            conn.Close();

            return count;
        }//end numberOfCheckedIn

        public string[] findTransaction(string allowanceID) {

            string sql = "select * " +
                         "from ChildcareTransaction "+
                         "where Allowance_ID = @allowanceID and CheckedOut is null";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@allowanceID", allowanceID));
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);
            int cCount = DS.Tables[0].Columns.Count;
            string[] transaction = new string[cCount];
            for (int x = 0; x < cCount; x++) {
                transaction[x] = DS.Tables[0].Rows[0][x].ToString();
            }
            conn.Close();

            return transaction;
        }//end FindTransaction

        public bool isCheckedIn(string childID, string guardianID){
            string familyID = guardianID.Remove(guardianID.Length - 1);

            string sql = "select ChildcareTransaction_ID " +
                         "from Child natural join AllowedConnections natural join ChildcareTransaction " +
                         "where Family_ID = @familyID and Child_ID = @childID and CheckedOut is null ";
           
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@familyID", familyID));
            command.Parameters.Add(new MySqlParameter("@childID", childID));

            conn.Open();
            object recordFound = command.ExecuteScalar();
            conn.Close();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            return false;
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
