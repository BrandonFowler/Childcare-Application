using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Globalization;

namespace ChildCareAppParentSide {

    class ChildCheckInDatabase {

        private SQLiteConnection dbCon;

        public ChildCheckInDatabase() {
            dbCon = new SQLiteConnection("Data Source=../../ChildcareDB.s3db;Version=3;");
        }//end Database

        public bool validateLogin(string ID, string PIN) {

            string sql = "select rowid " +
                         "from Guardian " + 
                         "where Guardian_ID = " + ID + " and GuardianPIN = " + PIN;

            dbCon.Open();
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
            return false;
        }//end validateLogin

        public String[,] findChildren(string id) {

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID "+ 
                  "where Guardian_ID = "+id;

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
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

            dbCon.Close();
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
                         "where Day= '" + day + "' and Month='" + month + "' or Weekday='" + dayOfWeek + "' or EventName='Normal Care'";

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
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
            dbCon.Close();
            return events;
        }//end getEvents
       
        public bool checkIn(string childID, string eventID, string guardianID, string birthday) {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string date = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
            string time = Convert.ToDateTime(dateTime).ToString("HH:mm:ss");
            bool isInfant;
            if (Convert.ToInt32(eventID) == 0) {
                isInfant = checkInfant(birthday, date);
                if (isInfant) {
                    eventID = "1";
                }
            }

            string connectionID = getConnectionID(guardianID, childID);

            string sql = "insert into " + 
                  "Transactions (Event_ID,Connection_ID,Date,CheckedIn) " + 
                  "values ("+eventID+","+connectionID+",'"+date+"','"+time+"')";

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }//end checkIn

        public string getConnectionID(string guardianID, string childID) {

            string sql = "select Connection_ID " +
                         "from AllowedConnections " +
                         "where Guardian_ID = '" + guardianID + "' and Child_ID = '" + childID + "'";

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string connectionID = DS.Tables[0].Rows[0][0].ToString();
            dbCon.Close();
            return connectionID;
        }//end getConnectionID

        public bool checkOut(string childID, string guardianID) {
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = DateTime.Now.ToString();
            string currentDateString = Convert.ToDateTime(dateTimeString).ToString("yyyy-MM-dd");
            string currentTimeString = Convert.ToDateTime(dateTimeString).ToString("HH:mm:ss");
            string connectionID = getConnectionID(guardianID, childID);
            string[] transaction = findTransaction(connectionID);
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
               double totalCheckedInHours = hourDifference + (minuteDifference/60);
               if ((eventID.CompareTo("0") == 0 || eventID.CompareTo("1") == 0) && totalCheckedInHours > 3) {
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

            dbCon.Open();

            string sql = "update Transactions " +
                  "set CheckedOut='" + currentTimeString + "' " + ", TransactionTotal = " + eventFee +" "+
                  "where Connection_ID ="+connectionID +" and CheckedOut is null";
            
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();

            if (isLate) {
                eventID = "2";
                double lateFee = getLateFee(eventID);
                lateFee = lateFee * lateTime;
                    
                sql = "insert into " +
                      "Transactions (Event_ID,Connection_ID,Date,CheckedIn,CheckedOut,TransactionTotal) " +
                      "values ("+eventID+","+connectionID+",'"+currentDateString+"','00:00:00','00:00:00',"+lateFee+") ";

                dbCon.Open();

                command = new SQLiteCommand(sql, this.dbCon);
                command.ExecuteNonQuery();
            }
            dbCon.Close();
            return true;
        }//end checkOut

        public string getClosingTime(string dayOfWeek) {
            string sql = "select Closing " +
                         "from OperationHours " +
                         "where Weekday = '" + dayOfWeek + "'";

            dbCon.Open();
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string closingTime = DS.Tables[0].Rows[0][0].ToString();
            dbCon.Close();

            return closingTime;
        }// end getClosingTime

        public double getLateFee(string eventID) {

            string sql = "select HourlyPrice " +
                         "from EventData " +
                         "where Event_ID = " + eventID;

            dbCon.Open();
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string fee = DS.Tables[0].Rows[0][0].ToString();
            dbCon.Close();

            double lateFee = Convert.ToDouble(fee);
            return lateFee;
        }//end getLateFee

        
        public string[] getEvent(string eventID) {

            string sql = "select * " +
                  "from EventData " +
                  "where Event_ID = " + eventID;

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);

            int cCount = DS.Tables[0].Columns.Count;
            string[] eventData = new string[cCount];
            for (int x = 0; x < cCount; x++) {
                eventData[x] = DS.Tables[0].Rows[0][x].ToString();
            }
            dbCon.Close();
            return eventData;
        }//end getEvent

        public int numberOfCheckedIn(string guardianID) {

            string sql = "select Transaction_ID " +
                         "from AllowedConnections natural join Transactions " +
                         "where Guardian_ID = '" + guardianID + "'" + " and CheckedOut is null";

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            
            if (DS.Tables == null) {
                dbCon.Close();
                return 0;
            }
            int count = DS.Tables[0].Rows.Count;
            dbCon.Close();
            return count;
        }//end numberOfCheckedIn

        public string[] findTransaction(string connectionID) {

            string sql = "select * " +
                         "from Transactions "+
                         "where Connection_ID =" + connectionID + " and CheckedOut is null";

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int cCount = DS.Tables[0].Columns.Count;
            string[] transaction = new string[cCount];
            for (int x = 0; x < cCount; x++) {
                transaction[x] = DS.Tables[0].Rows[0][x].ToString();
            }
            dbCon.Close();
            return transaction;
        }//end FindTransaction

       
       
        public string[] getParentInfo(String ID) {

            string sql = "select * " + 
                         "from Guardian " + 
                         "where Guardian_ID = " + ID;

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);

            int cCount = DS.Tables[0].Columns.Count;
            string[] data = new String[cCount];

            for (int x = 0; x < cCount; x++) {
                data[x] = DS.Tables[0].Rows[0][x].ToString();
            }
            dbCon.Close();
            return data;
        }// end getParentInfo

        public bool isCheckedIn(string childID, string guardianID){
            

            string sql = "select Connection_ID " + 
                         "from AllowedConnections " + 
                         "where Guardian_ID = '" + guardianID + "' and Child_ID = '" + childID + "'";

            dbCon.Open();

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string connectionID = DS.Tables[0].Rows[0][0].ToString();

            sql = "select rowid " + 
                  "from Transactions " + 
                  "where Connection_ID =" + connectionID + " and CheckedOut is null";

            command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
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
            double hours = hourDifference + (minuteDifference / 60);
            if (hours < 0) {
                return 0;
            }
            return hours;
        }//end checkIfPastClosing

        public bool checkIfHourly(string eventID) {
            string[] eventData = getEvent(eventID);
            if (String.IsNullOrWhiteSpace(eventData[2])) {
                return true;
            }
            else {
                return false;
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
