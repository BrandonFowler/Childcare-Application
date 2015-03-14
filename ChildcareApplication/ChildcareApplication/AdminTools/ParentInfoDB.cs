using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminTools {
    class ParentInfoDB {
        private SQLiteConnection connection;

        public ParentInfoDB() {
            this.connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            try {
                connection.Open();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public String GetParentName(String parentID) {
            String query = "SELECT FirstName, LastName FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            SQLiteDataReader reader = cmd.ExecuteReader();
            reader.Read();

            String result = reader.GetString(0) + " " + reader.GetString(1);

            reader.Close();
            return result;
        }

        public String GetAddress1(String parentID) {
            String query = "SELECT Address1 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        public String GetAddress2(String parentID) {
            String query = "SELECT Address2 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        //returns a string for the state, zip, and city
        public String GetAddress3(String parentID) {
            String query = "SELECT City, StateAbrv, Zip FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            SQLiteDataReader reader = cmd.ExecuteReader();
            reader.Read();

            String result = reader.GetString(0) + ", " + reader.GetString(1) + " " + reader.GetString(2);

            reader.Close();
            return result;
        }

        public String GetPhoneNumber(String parentID) {
            String query = "SELECT Phone FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        public String GetPhotoPath(String parentID) {
            String query = "SELECT PhotoLocation FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        public String GetCurrentDue(String parentID) {
            String familyID = parentID.Remove(parentID.Length - 1);

            String query = "SELECT FamilyTotal FROM Family WHERE Family_ID = '" + familyID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            String curDue = "$" + Convert.ToString(cmd.ExecuteScalar());

            if (curDue.IndexOf('.') == curDue.Length - 1) {
                if (curDue.IndexOf('.') == curDue.Length - 1) {
                    curDue += "0";
                }
            } else if (!curDue.Contains('.')) {
                curDue += ".00";
            }

            return curDue;
        }

        public void UpdateCurBalance(String parentID, double paymentValue) {
            String familyID = parentID.Remove(parentID.Length - 1);

            String query = "SELECT FamilyTotal FROM Family WHERE Family_ID = '" + familyID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            Double currentBalance = Convert.ToDouble(cmd.ExecuteScalar());

            query = "Update Family SET FamilyTotal = '" + (currentBalance - paymentValue) + "' WHERE Family_ID = '";
            query += familyID + "';";

            cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery();
        }

        ~ParentInfoDB() {
            this.connection.Close();
        }
    }
}
