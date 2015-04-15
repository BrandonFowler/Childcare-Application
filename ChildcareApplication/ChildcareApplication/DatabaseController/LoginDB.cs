using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace DatabaseController {

    class LoginDB {
        private SQLiteConnection dbCon;

        public LoginDB() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }//end Database

        public bool validateLogin(string ID) {
            dbCon.Open();
            string sql = "select Guardian_ID from Guardian WHERE Guardian_ID = " + ID;
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);

            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            if (count > 0) {
                dbCon.Close();
                return true;
            }



            dbCon.Close();
            return false;
        }//end validateLogin

        public bool validateAdminLogin(string ID, string PIN) {
            dbCon.Open();
            string sql = "SELECT * FROM Administrator WHERE AdministratorUN = \"" + ID + "\" AND AdministratorPW = \"" + PIN + "\";";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);


            SQLiteDataAdapter db = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            db.Fill(DS);
            int count = DS.Tables[0].Rows.Count;
            if (count > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
            return false;
        }//end validateLogin

        public int GetAccessLevel(string ID) {
            int accessLevel = -1;
            String query = "Select AccessLevel from Administrator where AdministratorUN = '" + ID + "';";


            try {
                this.dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, this.dbCon);
                accessLevel = Convert.ToInt32(cmd.ExecuteScalar());
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
            return accessLevel;
        }

        public bool ValidateLogin(string ID, string PIN) {
            string sql = "select Guardian_ID " +
                         "from Guardian " +
                         "where Guardian_ID = @ID and GuardianPIN = @PIN";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
            command.Parameters.Add(new SQLiteParameter("@PIN", PIN));
            try {
                dbCon.Open();
                object recordFound = command.ExecuteScalar();
                dbCon.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    dbCon.Close();
                    return true;
                }
            } catch (Exception) {
                MessageBox.Show("Database Connection Failure");
                dbCon.Close();
            }
            return false;
        }
    }//end Database
}//end namespace
