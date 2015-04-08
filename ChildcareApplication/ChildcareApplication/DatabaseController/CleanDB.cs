using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;


namespace ChildcareApplication.DatabaseController {
    class CleanDB {

        private SQLiteConnection conn;

        public CleanDB() {
            conn = new SQLiteConnection("Data Source=../../Database/ChildCareDB.s3db;Version=3;");
        }

        public bool clean() {
            bool success = true;
            int daysToKeepRecords = getRecordExpiration()*(-1);
            if (daysToKeepRecords == 0) {
                 return false;
            }
            DateTime date = DateTime.Now.AddDays(daysToKeepRecords);
            string expirationDate = date.ToString("yyyy-MM-dd");
            success = deleteTransactions(expirationDate);
            if (!success) {
                return false;
            }
            success = deleteConnections(expirationDate);
            if (!success) {
                return false;
            }
            success = deleteGuardians(expirationDate);
            if (!success) {
                return false;
            }
            success = deleteChildren(expirationDate);
            return success;
        }

        public int getRecordExpiration() {
            string settingName = "Days to Hold Expired and Deleted Records";
            String sql = "select SettingValue " +
                         "from ApplicationSettings " +
                         "where SettingName = @settingName";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.Parameters.Add(new SQLiteParameter("@settingName", settingName));
            try {
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();
                return Convert.ToInt32(recordFound);
            } catch (Exception) {
                conn.Close();
                return 0;
            }
        }

        public bool deleteTransactions(string expirationDate) {
            String sql = "delete " +
                         "from ChildcareTransaction " +
                         "where TransactionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception) {
                conn.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool deleteConnections(string expirationDate) {
            String sql = "delete " +
                         "from AllowedConnections " +
                         "where ConnectionDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            } catch (Exception) {
                conn.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool deleteGuardians(string expirationDate) {
            String sql = "delete " +
                         "from Guardian " +
                         "where GuardianDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            } catch (Exception) {
                conn.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool deleteChildren(string expirationDate) {
            String sql = "delete " +
                         "from Child " +
                         "where ChildDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            } catch (Exception) {
                conn.Close();
                MessageBox.Show("Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

    }
}
