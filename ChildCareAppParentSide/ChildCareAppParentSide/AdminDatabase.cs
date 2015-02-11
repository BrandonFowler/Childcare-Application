using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Globalization;

namespace ChildCareAppParentSide {

    class AdminDatabase {

        private SQLiteConnection dbCon;

        public AdminDatabase() {
            dbCon = new SQLiteConnection("Data Source=../../ChildcareDB.s3db;Version=3;");
        }//end AdminDatabase

        public bool validateAdmin(string userName, string password) {

            dbCon.Open();

            string sql = "select rowid " +
                         "from Administrator " + 
                         "where AdministratorPW=" + password + " or AdministratorUN='" + userName + "'";

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            int recordFound = Convert.ToInt32(command.ExecuteScalar());

            if (recordFound > 0) {
                dbCon.Close();
                return true;
            }

            dbCon.Close();
            return false;
        }//end validateLogin
    }
}
