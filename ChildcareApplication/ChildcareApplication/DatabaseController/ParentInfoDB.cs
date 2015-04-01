using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatabaseController {
    class ParentInfoDB {

        public ParentInfoDB() {
            
        }

        public String GetParentName(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String result = "";

            try {
                connection.Open();

                String query = "SELECT FirstName, LastName FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                result = reader.GetString(0) + " " + reader.GetString(1);

                reader.Close();
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetAddress1(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT Address1 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            String result = "";

            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            try {
                connection.Open();

                result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetAddress2(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT Address2 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            String result = "";

            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            try {
                connection.Open();

                result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        //returns a string for the state, zip, and city
        public String GetAddress3(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT City, StateAbrv, Zip FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            String result = "";

            try {
                connection.Open();

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                result = reader.GetString(0) + ", " + reader.GetString(1) + " " + reader.GetString(2);
                reader.Close();
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetPhoneNumber(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT Phone FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            String result = "";

            try {
                connection.Open();

                result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetPhotoPath(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT PhotoLocation FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            String result = "";

            try {
                connection.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetCurrentDue(String parentID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String familyID = parentID.Remove(parentID.Length - 1);

            String query = "SELECT FamilyTotal FROM Family WHERE Family_ID = '" + familyID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            String curDue = "$";

            try {
                connection.Open();
                curDue += Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }

            if (curDue.IndexOf('.') == curDue.Length - 2) {
                curDue += "0";
            } else if (!curDue.Contains('.')) {
                curDue += ".00";
            }
            return curDue;
        }

        public void UpdateCurBalance(String parentID, double paymentValue) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String familyID = parentID.Remove(parentID.Length - 1);

            String query = "SELECT FamilyTotal FROM Family WHERE Family_ID = '" + familyID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);

            try {
                connection.Open();
                Double currentBalance = Convert.ToDouble(cmd.ExecuteScalar());

                query = "Update Family SET FamilyTotal = '" + (currentBalance - paymentValue) + "' WHERE Family_ID = '";
                query += familyID + "';";

                cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public bool GuardianIDExists(string guardianID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT Guardian_ID FROM Guardian WHERE Guardian_ID = '" + guardianID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            String result = "";

            try {
                connection.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }

            if( result == guardianID) {
                return true;
            } else {
                return false;
            }
        }
    }
}
