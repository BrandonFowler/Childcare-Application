using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace MockUp
{
    class Database
    {
        private SQLiteConnection dbCon;

        public Database()
        {
            dbCon = new SQLiteConnection("Data Source=../../clients.db;Version=3;");
        }

        public String[] findChildren(string id) {
            dbCon.Open();
            string sql = "select name from child where parentID = " + id;
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            String[] names = new string[count];
            int x = 0;
            while (x < count) {
                names[x] = DS.Tables[0].Rows[x][0].ToString();
                x++;
            }
            dbCon.Close();
            return names;
        }

        public bool checkIn(string name) {
            dbCon.Open();
            string sql = "update child set checkedIn = 1 where name = '" + name +"'";
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
        }

    }
}



