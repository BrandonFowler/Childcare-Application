using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Globalization;

namespace ChildCareAppParentSide {

    class Database {

        private SQLiteConnection dbCon;

        public Database() {
            dbCon = new SQLiteConnection("Data Source=../../ChildcareDB.s3db;Version=3;");
        }//end Database

        public bool validateLogin(string ID, string PIN) {
            dbCon.Open();
            string sql = "select rowid from Guardian where Guardian_ID = " + ID + " and GuardianPIN = " + PIN;
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
            return false;
        }//end validateLogin

        public bool validateAdmin(string userName, string password) {
            dbCon.Open();
            string sql = "select rowid from Administrator where AdministratorPW="+password+" or AdministratorUN='"+userName+"'";
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
            dbCon.Open();

            string sql = "select Child.* " +
                "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                "where Guardian_ID =" + id;
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if(recordFound == 0) {
              dbCon.Close();
              return null;
            }

            sql = "select Child.* "+
                "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID "+ 
                "where Guardian_ID ="+id;
            command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
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
            dbCon.Open();
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string month = Convert.ToDateTime(dateTime).ToString("MM");
            string day = Convert.ToDateTime(dateTime).ToString("dd");
            string dayOfWeek = dt.DayOfWeek.ToString();
            string sql = "select Event_ID, EventName from EventData where Day= '" + day + "' and Month='" + month + "' or Weekday='" + dayOfWeek + "' or EventName='Normal Care'";
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
        }

       
        public bool checkIn(string childID, string eventID, string guardianID) {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string date = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
            string time = Convert.ToDateTime(dateTime).ToString("HH:mm:ss");
            dbCon.Open();
            string sql = "select Connection_ID from AllowedConnections where Guardian_ID = '"+guardianID+"' and Child_ID = '"+childID+"'";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string connectionID = DS.Tables[0].Rows[0][0].ToString();
            sql = "insert into Transactions (Event_ID,Connection_ID,Date,CheckedIn) values ("+eventID+","+connectionID+",'"+date+"','"+time+"')";
            command = new SQLiteCommand(sql, this.dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }//end checkIn

        public bool checkOut(string childID, string guardianID) {
            dbCon.Open();
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string time = Convert.ToDateTime(dateTime).ToString("HH:mm:ss");
            string sql = "select Connection_ID from AllowedConnections where Guardian_ID = '" + guardianID + "' and Child_ID = '" + childID + "'";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string connectionID = DS.Tables[0].Rows[0][0].ToString();
            sql = "update Transactions set CheckedOut='"+time+"' where Connection_ID ="+connectionID +" and CheckedOut is null";
            command = new SQLiteCommand(sql, this.dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }//end checkOut
       
        public string[] getParentInfo(String ID) {
            dbCon.Open();
            string sql = "select * from Guardian where Guardian_ID = " + ID;
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

        public string[] getChildInfo(String ID) {
            dbCon.Open();
            string sql = "select * from Child where Child_ID = " + ID;
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
        }//end getChildInfo

        public bool isCheckedIn(string childID, string guardianID){
            dbCon.Open();
            string sql = "select Connection_ID from AllowedConnections where Guardian_ID = '" + guardianID + "' and Child_ID = '" + childID + "'";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            string connectionID = DS.Tables[0].Rows[0][0].ToString();
            sql = "select rowid from Transactions where Connection_ID =" + connectionID + " and CheckedOut is null";
            command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
            return false;
        }

    }//end Database(Class)
}
