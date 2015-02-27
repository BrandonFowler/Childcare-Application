using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using MySql.Data.MySqlClient;

namespace ChildCareApp {

    class Database {

        
        private MySql.Data.MySqlClient.MySqlConnection dbCon;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;

        public Database()
        {
            this.server = "146.187.135.22";
            this.port = "3306";
            this.database = "childcare_v5";
            this.UID = "ccdev";
            this.password = "devpw821";
            connectionString = "SERVER="+server+"; PORT="+port+"; DATABASE="+database+"; UID="+UID+"; PASSWORD="+password+";";
            dbCon = new MySql.Data.MySqlClient.MySqlConnection();
            dbCon.ConnectionString = connectionString;
        }//end Database(default constructor)

        /*private SQLiteConnection dbCon;

        public Database()
        {
            dbCon = new SQLiteConnection("Data Source=../../ChildCare_v3.s3db;Version=3;");
        }//end Database*/

        public bool validateLogin(string ID)
        {
            dbCon.Open();
            string sql = "select Guardian_ID from Guardian WHERE Guardian_ID = " + ID ;
            MySqlCommand command = new MySqlCommand(sql, dbCon);

            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            if (count > 0)
            {
                dbCon.Close();
                return true;
            }

            /*int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0)
            {
                dbCon.Close();
                return true;
            }*/

            dbCon.Close();
            return false;
        }//end validateLogin

        public bool validateAdminLogin(string ID, string PIN) 
        {
            dbCon.Open();
            string sql = "SELECT * FROM Administrator WHERE AdministratorUN = \"" + ID + "\" AND AdministratorPW = \"" + PIN + "\";";
            MySqlCommand command = new MySqlCommand(sql, this.dbCon);
            //SQLiteDataReader r = command.ExecuteReader();

            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            if (count > 0)
            {
                dbCon.Close();
                return true;
            }
           // if(DS.)
            
           /* try
            {
                int recordFound = Convert.ToInt32(command.ExecuteScalar());

                if (recordFound > 0)
                {
                    dbCon.Close();
                    return true;
                }
            }
            catch(SQLiteException){
                MessageBox.Show("hi from catch");
                
            }*/
            dbCon.Close();
            return false;
        }//end validateLogin

        public String[] findChildren(string id)
        {
            dbCon.Open();

            string sql = "select rowid from child where parentID = " + id;
            MySqlCommand command = new MySqlCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound == 0)
            {
                dbCon.Close();
                return null;
            }

            sql = "select name from child where parentID = " + id;
            command = new MySqlCommand(sql, this.dbCon);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            String[] names = new string[count];
            int x = 0;
            while (x < count)
            {
                names[x] = DS.Tables[0].Rows[x][0].ToString();
                x++;
            }
            dbCon.Close();
            return names;
        }//end findChildren

        public bool checkIn(string name)
        {
            dbCon.Open();
            string sql = "update child set checkedIn = 1 where name = '" + name + "'";
            MySqlCommand command = new MySqlCommand(sql, dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }

        public bool checkOut(string name)
        {
            dbCon.Open();
            string sql = "update child set checkedIn = 0 where name = '" + name + "'";
            MySqlCommand command = new MySqlCommand(sql, dbCon);
            command.ExecuteNonQuery();
            dbCon.Close();
            return true;
        }//end checkOut

    }//end Database
}//end namespace
