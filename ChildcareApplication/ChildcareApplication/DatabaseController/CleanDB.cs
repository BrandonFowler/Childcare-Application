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
            conn = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        public bool Clean() {
            bool success = true;
            int daysToKeepRecords = GetRecordExpiration()*(-1);
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

        public bool DeleteTransactions(string expirationDate) {
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

        public bool DeleteConnections(string expirationDate) {
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

        public bool DeleteGuardians(string expirationDate) {
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

        public bool DeleteChildren(string expirationDate) {
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
