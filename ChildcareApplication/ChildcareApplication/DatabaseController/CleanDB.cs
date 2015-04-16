using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using ChildcareApplication.Properties;


namespace ChildcareApplication.DatabaseController {
    class CleanDB {

        private SQLiteConnection dbCon;

        public CleanDB() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        public bool Clean() {
            bool success = true;
            int daysToKeepRecords;
            try {
                daysToKeepRecords = Convert.ToInt32(Settings.Default.HoldExpiredRecords) * (-1);
            }
            catch {
                MessageBox.Show("Error: Unable to retrieve settings data, database clean up routine failed.");
                return false;
            }
            if (daysToKeepRecords == 0) {
                 return false;
            }
            DateTime date = DateTime.Now.AddDays(daysToKeepRecords);
            string expirationDate = date.ToString("yyyy-MM-dd");
            success = DeleteTransactions(expirationDate);
            if (!success) {
                return false;
            }
            success = DeleteConnections(expirationDate);
            if (!success) {
                return false;
            }
            success = DeleteGuardians(expirationDate);
            if (!success) {
                return false;
            }
            success = DeleteChildren(expirationDate);
            return success;
        }

        public int GetRecordExpiration() {
            string settingName = "Days to Hold Expired and Deleted Records";
            String sql = "select SettingValue " +
                         "from ApplicationSettings " +
                         "where SettingName = @settingName";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@settingName", settingName));
            try {
                dbCon.Open();
                object recordFound = command.ExecuteScalar();
                dbCon.Close();
                return Convert.ToInt32(recordFound);
            } catch (Exception) {
                dbCon.Close();
                return 0;
            }
        }

        public bool DeleteTransactions(string expirationDate) {
            String sql = "delete " +
                         "from ChildcareTransaction " +
                         "where TransactionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            }
            catch (Exception) {
                dbCon.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool DeleteConnections(string expirationDate) {
            String sql = "delete " +
                         "from AllowedConnections " +
                         "where ConnectionDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            } catch (Exception) {
                dbCon.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool DeleteGuardians(string expirationDate) {
            String sql = "delete " +
                         "from Guardian " +
                         "where GuardianDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            } catch (Exception) {
                dbCon.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool DeleteChildren(string expirationDate) {
            String sql = "delete " +
                         "from Child " +
                         "where ChildDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            } catch (Exception) {
                dbCon.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

    }
}
