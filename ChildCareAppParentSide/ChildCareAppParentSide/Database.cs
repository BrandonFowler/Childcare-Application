using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace ChildCareAppParentSide {

    class Database {

        private SQLiteConnection dbCon;

        public Database() {
            dbCon = new SQLiteConnection("Data Source=../../clients.db;Version=3;");
        }//end Database

        public bool validateLogin(string ID, string PIN) {
            dbCon.Open();
            string sql = "select rowid from client where ID = " + ID + " and PIN = " + PIN;
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
            return false;
        }//end validateLogin

        public bool validateAdmin(string ID, string PIN) {
            dbCon.Open();
            string sql = "select rowid from admins where ID = " + ID + " and PIN = " + PIN;
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

            string sql = "select rowid from child where parentID = " + id;
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if(recordFound == 0) {
              dbCon.Close();
              return null;
            }

            sql = "select childID, name from child where parentID = " + id;
            command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            String[,] data = new string[count,2];
           
            for(int x = 0; x < count; x++) {
                for(int y = 0; y < 2; y++) {
                    data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                
                }
            }

            dbCon.Close();
            return data;
        }//end findChildren

        public bool checkIn(string name) {
            dbCon.Open();
            string sql = "update child set checkedIn = 1 where name = '" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }

        public bool checkOut(string name) {
            dbCon.Open();
            string sql = "update child set checkedIn = 0 where name = '" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }//end checkOut

    }//end Database(Class)
}
