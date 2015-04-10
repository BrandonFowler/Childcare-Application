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
        
        public ChildInfoDatabase()
        {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }


        public DataSet GetMaxID() {
            dbCon.Open();
            DataSet DS = new DataSet();
            try {
                string sql = "SELECT MAX(Child_ID) FROM Child;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

                DB.Fill(DS);
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
            return DS;
        }

        public DataSet GetMaxConnectionID() {
            dbCon.Open();
            DataSet DS = new DataSet();

            try {
                string sql = "SELECT MAX(Allowance_ID) FROM AllowedConnections;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

                DB.Fill(DS);
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
            return DS;
        }

        public void AddNewChild(string cID, string fName, string lName, string birthday, string allergies, string medical, string photo) {

            dbCon.Open();
              try
              {

                    string sql = "INSERT INTO Child(Child_ID, FirstName, LastName, Birthday, Allergies, Medical, PhotoLocation) "
                        + "VALUES ('" + cID + "', '" + fName + "', '" + lName + "', '" + birthday + "', '" + allergies + "', '" + medical + "', '" + photo + "');";

            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.CommandText = sql;
            command.ExecuteNonQuery();
            }
             catch (SQLiteException e)
             {
                 MessageBox.Show(e.ToString());
             }
            dbCon.Close();
        }
        public void UpdateAllowedConnections(string conID, string pID, string cID, string famID) {
            dbCon.Open();

            try {
                string sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                + "VALUES(" + conID + ", " + pID + ", " + cID + ", " + famID + ");";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;

                command.ExecuteNonQuery();
                MessageBox.Show("Link Completed.");
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }

        public void UpdateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies, string filePath) {

            dbCon.Open();



            try {
               
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
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }
        public void DeleteAllowedConnection(string childID, string pID) {

            dbCon.Open();

            try
            {

                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = @"UPDATE AllowedCOnnections SET ConnectionDeletionDate = @today WHERE Child_ID = @childID;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@childID", childID));
                command.ExecuteNonQuery();

                MessageBox.Show("Completed");
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Failed");
            }

            dbCon.Close();

        }//end GetFirstName
        public void DeleteChildInfo(string childID) {

            dbCon.Open();
            try {

                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = @"UPDATE Child SET ChildDeletionDate = @today WHERE Child_ID = @childID;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@childID", childID));
                command.ExecuteNonQuery();

                /*string sql2 = "DELETE from AllowedConnections where Child_ID = " + childID;
                SQLiteCommand command2 = new SQLiteCommand(sql2, dbCon);
                command2.CommandText = sql2;
                command2.ExecuteNonQuery();*/




                MessageBox.Show("Completed");
            } catch (SQLiteException e) {
                MessageBox.Show("Failed");
            }
            dbCon.Close();

        }//end GetFirstName

        public String[,] findChildren(string id) {
            dbCon.Open();

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where Guardian_ID = " + id + " AND ConnectionDeletionDate IS null";

            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

            DataSet DS = new DataSet();
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
    }
}
