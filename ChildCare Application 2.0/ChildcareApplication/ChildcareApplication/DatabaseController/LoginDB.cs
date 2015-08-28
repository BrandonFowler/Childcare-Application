﻿using MessageBoxUtils;
using System;
using System.Data;
using System.Data.SQLite;

namespace DatabaseController {

    class LoginDB {
        private SQLiteConnection dbCon;

        public LoginDB() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        internal bool validateAdminLogin(string ID, string PIN) {
            try {
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
            } catch (Exception e) {
                WPFMessageBox.Show(e.Message);
                dbCon.Close();
            }
            return false;
        }

        internal int GetAccessLevel(string ID) {
            int accessLevel = -1;
            String query = "Select AccessLevel from Administrator where AdministratorUN = '" + ID + "';";

            try {
                this.dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, this.dbCon);
                accessLevel = Convert.ToInt32(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception e) {
                WPFMessageBox.Show(e.Message);
                dbCon.Close();
            }
            return accessLevel;
        }

        public bool ValidateGuardianLogin(string ID, string PIN) {
            string sql = "select Guardian_ID " +
                         "from Guardian " +
                         "where Guardian_ID = @ID and GuardianPIN = @PIN and GuardianDeletionDate is NULL";
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
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
            } catch (Exception) {
                WPFMessageBox.Show("Unable to retrieve guardian information.");
                dbCon.Close();
            }
            return false;
        }
    }
}
