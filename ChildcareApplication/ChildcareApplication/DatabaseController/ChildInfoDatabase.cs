using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;

namespace DatabaseController {
    class ChildInfoDatabase {

        private SQLiteConnection dbCon;

        public ChildInfoDatabase() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }


        public int GetMaxChildID() {

            int maxID = 0;
            try {
                dbCon.Open();
                string sql = "SELECT MAX(Child_ID) FROM Child;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();

                if (!reader.IsDBNull(0)) {
                    maxID = Convert.ToInt32(reader.GetString(0));
                } else {
                    maxID = 0;
                }
                reader.Close();
                dbCon.Close();
            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
            return maxID;
        }

        public int GetMaxConnectionID() {
            
            DataSet DS = new DataSet();
            int maxID = 0;

            try {
                dbCon.Open();
                string sql = "SELECT MAX(Allowance_ID) FROM AllowedConnections;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();

                if (!reader.IsDBNull(0)) {
                    maxID = Convert.ToInt32(reader.GetString(0));
                } else {
                    maxID = 0;
                }
                reader.Close();
                dbCon.Close();
            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
            return maxID;
        }

        public void AddNewChild(string cID, string fName, string lName, string birthday, string allergies, string medical, string photo) {

            
            try {
                dbCon.Open();
                string sql = "INSERT INTO Child(Child_ID, FirstName, LastName, Birthday, Allergies, Medical, PhotoLocation) "
                    + "VALUES ('" + cID + "', '" + fName + "', '" + lName + "', '" + birthday + "', '" + allergies + "', '" + medical + "', '" + photo + "');";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.ExecuteNonQuery();
            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
            dbCon.Close();
        }

        public void UpdateExistingChilderen(string conID, string pID, string cID, string famID) {

            try {
                dbCon.Open();
                String sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                + "VALUES(" + conID + ", " + pID + ", " + cID + ", " + famID + ");";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;

                command.ExecuteNonQuery();
                MessageBox.Show("Link Completed.");

            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
            dbCon.Close();
        }

        public void UpdateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies, string filePath) {
            try {
                dbCon.Open();
                string sql = @"UPDATE Child SET FirstName = @firstName, LastName = @lastName, Birthday = @birthday, Allergies = @allergies, Medical = @medical, PhotoLocation = @filePath WHERE Child_ID = @ID;";

                SQLiteCommand mycommand = new SQLiteCommand(sql, dbCon);
                mycommand.CommandText = sql;
                mycommand.Parameters.Add(new SQLiteParameter("@firstName", firstName));
                mycommand.Parameters.Add(new SQLiteParameter("@lastName", lastName));
                mycommand.Parameters.Add(new SQLiteParameter("@birthday", birthday));
                mycommand.Parameters.Add(new SQLiteParameter("@allergies", allergies));
                mycommand.Parameters.Add(new SQLiteParameter("@medical", medical));
                mycommand.Parameters.Add(new SQLiteParameter("@filePath", filePath));

                mycommand.Parameters.Add(new SQLiteParameter("@ID", ID));

                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
                dbCon.Close();
            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
        }

        public void DeleteChildInfo(string childID) {
            try {
                dbCon.Open();
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = @"UPDATE Child SET ChildDeletionDate = @today WHERE Child_ID = @childID;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@childID", childID));
                command.ExecuteNonQuery();

                MessageBox.Show("Completed");
                dbCon.Close();
            } catch (SQLiteException e) {
                MessageBox.Show(e.Message);
            }
        }//end GetFirstName

        public String[,] FindChildren(string guardianID) {
            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where Guardian_ID = @guardianID and ChildDeletionDate is null and ConnectionDeletionDate is NULL";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            DataSet DS = new DataSet();
            try {
                dbCon.Open();
                DB.Fill(DS);
                if (DS.Tables == null) {
                    return null;
                }
                int cCount = DS.Tables[0].Columns.Count;
                int rCount = DS.Tables[0].Rows.Count;
                String[,] data = new string[rCount, cCount];
                for (int x = 0; x < rCount; x++) {
                    for (int y = 0; y < cCount; y++) {
                        data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                    }
                }
                dbCon.Close();
                return data;
            } catch (Exception e) {
                MessageBox.Show(e.Message + "\n\n Database connection error: Unable to retrieve information for children");
                dbCon.Close();
                return null;
            }
        }

        public String[,] FindFamilyChildren(string fID, string ID) {



            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where  Family_ID = " + fID + " AND Guardian_ID != " + ID + " AND ConnectionDeletionDate IS null";

            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

            DataSet DS = new DataSet();
            try {
                dbCon.Open();
                DB.Fill(DS);
                dbCon.Close();
            }
            catch (Exception e) {
                MessageBox.Show(e.Message + "\n\n Database connection error: Unable to retrieve information for children");
                dbCon.Close();
            }

            if (DS.Tables == null) {
                return null;
            }

            int cCount = DS.Tables[0].Columns.Count;
            int rCount = DS.Tables[0].Rows.Count;
            String[,] data = new string[rCount, cCount];

            for (int x = 0; x < rCount; x++) {
                for (int y = 0; y < cCount; y++) {
                    data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                }
            }

            return data;
        }//end findChildren

        public String GetChildName(String transactionID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String result = "";

            try {
                connection.Open();

                String query = "SELECT FirstName, LastName FROM Child NATURAL JOIN AllowedConnections NATURAL JOIN ";
                query += "ChildcareTransaction WHERE ChildcareTransaction.ChildcareTransaction_ID = '" + transactionID + "';";
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

        public bool ChildNameExists(string fullName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            string[] nameAra = fullName.Split(' ');
            if (nameAra.Length != 2) {
                return false;
            }
            string firstName = nameAra[0];
            string lastName = nameAra[1];
            int count = 0;

            if (firstName == null || firstName.Length < 1 || lastName == null || lastName.Length < 1) {
                return false;
            }
            try {
                connection.Open();

                string query = "Select count(*) from Child where FirstName = '" + firstName + "' and LastName = '" + lastName + "';";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0) {
                    return true;
                }
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return false;
        }
    }
}
