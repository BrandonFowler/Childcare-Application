using System.Data.SQLite;

namespace ChildcareApplication.DatabaseController {
    class SettingsDB {

        private SQLiteConnection conn;

        public SettingsDB() {
            this.conn = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }


    }
}
