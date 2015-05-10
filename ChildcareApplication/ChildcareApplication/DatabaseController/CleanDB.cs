using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using ChildcareApplication.Properties;
using System.IO;
using MessageBoxUtils;

namespace ChildcareApplication.DatabaseController {
    class CleanDB {

        private SQLiteConnection dbCon;

        public CleanDB() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        public bool Clean() {
            var dirInfo = new DirectoryInfo("../../Database");
            dirInfo.Attributes &= ~FileAttributes.ReadOnly;
            bool success = true;
            int daysToKeepRecords;
            try {
                daysToKeepRecords = Convert.ToInt32(Settings.Default.HoldExpiredRecords) * (-1);
            }
            catch(Exception e){
                WPFMessageBox.Show(e.Message + "\n\n Error: Unable to retrieve settings data, database clean up routine failed.");
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
            catch (Exception e) {
                dbCon.Close();
                WPFMessageBox.Show(e.Message + "\n\n Database Connection Error: Unable to clean old records");
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
            } catch (Exception e) {
                dbCon.Close();
                WPFMessageBox.Show(e.Message + "\n\n Database Connection Error: Unable to clean old records");
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
            } catch (Exception e) {
                dbCon.Close();
                WPFMessageBox.Show(e.Message + "\n\n Database Connection Error: Unable to clean old records");
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
            } catch (Exception e) {
                dbCon.Close();
                WPFMessageBox.Show(e.Message + "\n\n Database Connection Error: Unable to clean old records");
                return false;
            }
            return true;
        }

    }
}
