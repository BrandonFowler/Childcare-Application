using MySql.Data.MySqlClient;
using System;

namespace ChildCareAppParentSide {

    class AdminDatabase {

        //private SQLiteConnection dbCon;
        private MySql.Data.MySqlClient.MySqlConnection conn;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;

        public AdminDatabase() {
            //dbCon = new SQLiteConnection("Data Source=../../ChildcareDB.s3db;Version=3;");
            this.server = "146.187.135.22";
            this.port = "3306";
            this.database = "childcare_v4";
            this.UID = "ccdev";
            this.password = "devpw821";
            connectionString = "SERVER=" + server + "; PORT=" + port + "; DATABASE=" + database + "; UID=" + UID + "; PASSWORD=" + password + ";";
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionString;
        }//end AdminDatabase

        public bool validateAdmin(string userName, string password) {

            //dbCon.Open();

            string sql = "select AccessLevel " +
                         "from Administrator " + 
                         "where AdministratorPW='" + password + "' and AdministratorUN='" + userName + "'";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            object recordFound = command.ExecuteScalar();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            //dbCon.Close();
            conn.Close();
            return false;
        }//end validateLogin
    }
}
