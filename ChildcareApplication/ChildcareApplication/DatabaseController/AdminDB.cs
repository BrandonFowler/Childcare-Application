using System;
using System.Data;
using System.Data.SQLite;

namespace ChildcareApplication.DatabaseController {
    class AdminDB {
        private SQLiteConnection dbCon;

        public AdminDB() {
            this.dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        public string[] FindAdmins() {
            dbCon.Open();

            string sql = "SELECT AdministratorUN from Administrator";

            SQLiteCommand comm = new SQLiteCommand(sql, dbCon);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(comm);
            DataSet adminlist = new DataSet();
            adapter.Fill(adminlist);

            int count = adminlist.Tables[0].Rows.Count;
            String[] names = new string[count];
            int x = 0;
            while (x < count) {
                names[x] = adminlist.Tables[0].Rows[x][0].ToString();
                x++;
            }
            dbCon.Close();

            return names;
        }

        internal string GetAccessLevel(string tempUN) {
            dbCon.Open();

            string sql = @"SELECT AccessLevel FROM Administrator WHERE AdministratorUN = @adminUN;";

            SQLiteCommand comm = new SQLiteCommand(sql, dbCon);

            comm.Parameters.Add(new SQLiteParameter("@adminUN", tempUN));

            SQLiteDataReader reader = comm.ExecuteReader();

            reader.Read();

            string result = reader.GetString(0);

            reader.Close();
            dbCon.Close();

            return result;
        }

        internal string GetAdminEmail(string tempUN) {
            dbCon.Open();

            string sql = @"SELECT AdministratorEmail FROM Administrator WHERE AdministratorUN = @adminUN;";

            SQLiteCommand comm = new SQLiteCommand(sql, dbCon);

            comm.Parameters.Add(new SQLiteParameter("@adminUN", tempUN));

            SQLiteDataReader reader = comm.ExecuteReader();

            reader.Read();

            string result = reader.GetString(0);

            reader.Close();
            dbCon.Close();

            return result;
        }


        internal void AddNewAdmin() {
            dbCon.Open();

            string sql = @"INSERT INTO Administrator VALUES (@newname, @newpw, 2, @newemail, null);";

            SQLiteCommand comm = new SQLiteCommand(sql, dbCon);

            string defaultdata = "default";
            string newPass = ChildcareApplication.AdminTools.Hashing.HashPass("default");
            comm.Parameters.Add(new SQLiteParameter("@newname", defaultdata));
            comm.Parameters.Add(new SQLiteParameter("@newpw", newPass));
            comm.Parameters.Add(new SQLiteParameter("@newemail", defaultdata));

            comm.ExecuteNonQuery();

            dbCon.Close();
        }

        internal void UpdateAdmin(string oldlogin, string newlogin, string newpw, string email, string access) {
            dbCon.Open();

            string sql = @"UPDATE Administrator SET AdministratorUN = @newUN, AdministratorPW = @newPW, AdministratorEmail = @em, AccessLevel = @al WHERE AdministratorUN = @oldUN;";
            SQLiteCommand comm = new SQLiteCommand(sql, dbCon);
            comm.Parameters.Add(new SQLiteParameter("@newUN", newlogin));
            comm.Parameters.Add(new SQLiteParameter("@newPW", newpw));
            comm.Parameters.Add(new SQLiteParameter("@em", email));
            comm.Parameters.Add(new SQLiteParameter("@al", access));
            comm.Parameters.Add(new SQLiteParameter("@oldUN", oldlogin));

            comm.ExecuteNonQuery();

            dbCon.Close();
        }

        internal void DeleteAdmin(string user) {
            dbCon.Open();

            string sql = @"DELETE FROM Administrator WHERE AdministratorUN = @delme;";
            SQLiteCommand comm = new SQLiteCommand(sql, dbCon);
            comm.Parameters.Add(new SQLiteParameter("@delMe", user));

            comm.ExecuteNonQuery();

            dbCon.Close();
        }
    }
}