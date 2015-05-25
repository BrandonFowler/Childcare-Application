using ChildcareApplication.Properties;
using MessageBoxUtils;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace ChildcareApplication.DatabaseController {
    class MaintenanceDB {

        private SQLiteConnection dbCon;

        public MaintenanceDB() {
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
            catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Could not access the setting for the amount of days to hold records. Please insure the setting is valid.");
                return false;
            }
            catch(Exception){
                WPFMessageBox.Show("Unable to retrieve settings data, database clean up routine failed.");
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
            if (!success) {
                return false;
            }
            success = DeleteEvents(expirationDate);
            return success;
        }

        public bool Backup() {
            try {
                var dirInfo = new DirectoryInfo("../../Backup Records");
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;
                DateTime lastBackup = Properties.Settings.Default.LastBackup;
                DateTime now = DateTime.Now;
                TimeSpan difference = now - lastBackup;
                if (difference.Days > 6) {
                    File.Copy(@"..\..\Database\ChildcareDB.s3db", @"..\..\Backup Records\ChildcareDB_" + now.ToString("MM-dd-yyyy") + ".s3db", true);
                    Properties.Settings.Default.LastBackup = DateTime.Now;
                    Properties.Settings.Default.Save();
                }
            }
            catch (System.IO.IOException) {
                WPFMessageBox.Show("Unable to backup records. Please insure write permissions are set for the database backup location.");
                return false;
            }
            catch (Exception) {
                WPFMessageBox.Show("Unable to backup records");
                return false;
            }
            return DeleteBackups();
        }

        public bool DeleteBackups() {
            try {
                int fileCount = Directory.GetFiles(@"..\..\Backup Records").Length;
                if (fileCount > 10) {
                    var files = new DirectoryInfo(@"..\..\Backup Records").EnumerateFiles()
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(10)
                    .ToList();
                    files.ForEach(f => f.Delete());
                }
            }
            catch (System.IO.IOException) {
                WPFMessageBox.Show("Unable to delete old records. Please insure write permissions are set for the database backup location.");
                return false;
            }
            catch (Exception) {
                WPFMessageBox.Show("Unable to delete old records");
                return false;
            }
            return true;
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
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return false;
            }catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to clean old records");
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
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return false;
            }catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to clean old records");
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
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return false;
            }catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to clean old records");
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
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return false;
            }catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to clean old records");
                return false;
            }
            return true;
        }

        public bool DeleteEvents(string expirationDate) {
            String sql = "delete " +
                         "from EventData " +
                         "where EventDeletionDate <= '" + expirationDate + "'";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            }
            catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return false;
            }
            catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to clean old records");
                return false;
            }
            return true;
        }

    }
}
