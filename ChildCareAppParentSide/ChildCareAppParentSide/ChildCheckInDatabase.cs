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
            this.database = "childcare_v4";
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
                         "where Guardian_ID = " + ID + " and GuardianPIN = " + PIN;

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            object recordFound = command.ExecuteScalar();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            conn.Close();
            return false;
        }//end validateLogin

        public string[] getParentInfo(String guardianID) {

            string sql = "select * " +
                         "from Guardian " +
                         "where Guardian_ID = " + guardianID;

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
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
                  "where Guardian_ID = " + guardianID;

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
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
                         "where EventDay= '" + day + "' and EventMonth='" + month + "' or EventWeekday='" + dayOfWeek + "' or Event_ID='2'";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
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

            string allowanceID = getConnectionID(guardianID, childID);
            int maxTransactionID = getNextPrimary("ChildCareTransaction_ID","ChildCareTransaction");

            string sql = "insert into " +
                         "ChildCareTransaction (ChildCareTransaction_ID,Event_ID,Allowance_ID,Date,CheckedIn) " + 
                         "values ("+maxTransactionID+","+eventID+","+allowanceID+",'"+date+"','"+time+"')";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            conn.Close();
            return true;
        }//end checkIn

        private int getNextPrimary(string column, string table) {
            string sql = "Select Max(" + column + ") from " + table;
            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            object max = command.ExecuteScalar();
            if (max != DBNull.Value && max != null) {
                conn.Close();
                return Convert.ToInt32(max) + 1;
            }
            conn.Close();
            return 0;
        }//end getMaxPrimary

        public string getConnectionID(string guardianID, string childID) {

            string sql = "select Allowance_ID " +
                         "from AllowedConnections " +
                         "where Guardian_ID = '" + guardianID + "' and Child_ID = '" + childID + "'";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string connectionID = DS.Tables[0].Rows[0][0].ToString();

            conn.Close();
            return connectionID;
        }//end getConnectionID

        public bool checkOut(string childID, string guardianID) {
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = DateTime.Now.ToString();
            string currentDateString = Convert.ToDateTime(dateTimeString).ToString("yyyy-MM-dd");
            string currentTimeString = Convert.ToDateTime(dateTimeString).ToString("HH:mm:ss");
            string allowanceID = getConnectionID(guardianID, childID);
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

            string sql = "update ChildCareTransaction " +
                  "set CheckedOut='" + currentTimeString + "' " + ", TransactionTotal = " + eventFee +" "+
                  "where Allowance_ID ="+ allowanceID +" and CheckedOut is null";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();

            if (isLate) {
                eventID = "1";
                double lateFee = getLateFee(eventID);
                lateFee = lateFee * lateTime;

                int maxTransactionID = getNextPrimary("ChildCareTransaction_ID", "ChildCareTransaction");
                    
                sql = "insert into " +
                      "ChildCareTransaction (ChildCareTransaction_ID,Event_ID,Allowance_ID,Date,CheckedIn,CheckedOut,TransactionTotal) " +
                      "values ("+maxTransactionID+","+eventID+","+allowanceID+",'"+currentDateString+"','00:00:00','00:00:00',"+lateFee+") ";

                conn.Open();
                command = new MySqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
            
            return true;
        }//end checkOut

        public string getClosingTime(string dayOfWeek) {
            string sql = "select Closing " +
                         "from OperatingHours " +
                         "where OperatingWeekday = '" + dayOfWeek + "'";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string closingTime = DS.Tables[0].Rows[0][0].ToString();
            conn.Close();

            return closingTime;
        }// end getClosingTime

        public double getLateFee(string eventID) {

            string sql = "select HourlyPrice " +
                         "from EventData " +
                         "where Event_ID = " + eventID;

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string fee = DS.Tables[0].Rows[0][0].ToString();
            conn.Close();

            double lateFee = Convert.ToDouble(fee);
            return lateFee;
        }//end getLateFee

        public string[] getEvent(string eventID) {

            string sql = "select * " +
                  "from EventData " +
                  "where Event_ID = " + eventID;

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
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

            string sql = "select ChildCareTransaction_ID " +
                         "from AllowedConnections natural join ChildCareTransaction " +
                         "where Guardian_ID = '" + guardianID + "'" + " and CheckedOut is null";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);

            int count = DS.Tables[0].Rows.Count;
            conn.Close();

            return count;
        }//end numberOfCheckedIn

        public string[] findTransaction(string allowanceID) {

            string sql = "select * " +
                         "from ChildCareTransaction "+
                         "where Allowance_ID =" + allowanceID + " and CheckedOut is null";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
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
            

            string sql = "select Allowance_ID " + 
                         "from AllowedConnections " + 
                         "where Guardian_ID = '" + guardianID + "' and Child_ID = '" + childID + "'";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string allowanceID = DS.Tables[0].Rows[0][0].ToString();

            sql = "select ChildCareTransaction_ID " + 
                  "from ChildCareTransaction " + 
                  "where Allowance_ID =" + allowanceID + " and CheckedOut is null";

            command = new MySqlCommand(sql, conn);
            object recordFound = command.ExecuteScalar();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }
            conn.Close();

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
