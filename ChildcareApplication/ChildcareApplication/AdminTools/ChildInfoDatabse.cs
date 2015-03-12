using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;

namespace AdminTools {
    class ChildInfoDatabse {

        //private SQLite.Data.SQLiteClient.SQLiteConnection dbCon;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;

        /*public ChildInfoDatabse()
        {
            this.server = "146.187.135.22";
            this.port = "3306";
            this.database = "childcare_v5";
            this.UID = "ccdev";
            this.password = "devpw821";
            connectionString = "SERVER="+server+"; PORT="+port+"; DATABASE="+database+"; UID="+UID+"; PASSWORD="+password+";";
            dbCon = new SQLite.Data.SQLiteClient.SQLiteConnection();
            dbCon.ConnectionString = connectionString;
        }//end Database(default constructor)*/

        private SQLiteConnection dbCon;

        public ChildInfoDatabse() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildCare_v3.s3db;Version=3;");
        }//end Database*/

        public DataSet GetMaxID() {
            dbCon.Open();
            DataSet DS = new DataSet();
            try {
                string sql = "SELECT MAX(Child_ID) FROM Child;";

                //SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
                // SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
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

                // SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
                // SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
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
            //  try
            //  {

            //string sql = "INSERT INTO Child VALUES ("+ cID + ", " + fName + ", " + lName + ", " + birthday + ", " + allergies + ", " + medical + ", " + photo + ");";
            string sql = "INSERT INTO Child(Child_ID, FirstName, LastName, Birthday, Allergies, Medical, PhotoLocation) "
                        + "VALUES ('" + cID + "', '" + fName + "', '" + lName + "', '" + birthday + "', '" + allergies + "', '" + medical + "', '" + photo + "');";

            // SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.CommandText = sql;
            command.ExecuteNonQuery();
            // }
            /* catch (SQLiteException e)
             {
                 MessageBox.Show(e.ToString());
             }*/
            dbCon.Close();
        }
        public void UpdateAllowedConnections(string conID, string pID, string cID, string famID) {
            dbCon.Open();

            try {
                string sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                + "VALUES(" + conID + ", " + pID + ", " + cID + ", " + famID + ");";

                //string sql = "INSERT INTO AllowedConnections VALUES(" + conID + ", " + pID + ", " + cID + ");";
                // SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);

                //string sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID) VALUES(@conID, @pID, @cID);";
                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;

                command.ExecuteNonQuery();
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }

        public void UpdateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies) {

            dbCon.Open();



            try {
                MessageBox.Show(birthday);
                string sql = @"UPDATE Child SET FirstName = @firstName, LastName = @lastName, Birthday = @birthday, Allergies = @allergies, Medical = @medical WHERE Child_ID = @ID;";
                //SQLiteCommand mycommand = new SQLiteCommand(sql, this.dbCon);
                SQLiteCommand mycommand = new SQLiteCommand(sql, dbCon);
                mycommand.CommandText = sql;
                mycommand.Parameters.Add(new SQLiteParameter("@firstName", firstName));
                mycommand.Parameters.Add(new SQLiteParameter("@lastName", lastName));
                mycommand.Parameters.Add(new SQLiteParameter("@birthday", birthday));
                mycommand.Parameters.Add(new SQLiteParameter("@allergies", allergies));
                mycommand.Parameters.Add(new SQLiteParameter("@medical", medical));

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
            try {

                string sql = "DELETE from AllowedConnections where Child_ID = " + childID + " AND Guardian_ID = " + pID + ";";
                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.ExecuteNonQuery();

                MessageBox.Show("Completed");
            } catch (SQLiteException e) {
                MessageBox.Show("Failed");
            }
            dbCon.Close();

        }//end GetFirstName
        public void DeleteChildInfo(string childID) {

            dbCon.Open();
            try {

                string today = DateTime.Now.ToString("yyyy-MM-dd");
                //string sql = "DELETE from Guardian where Guardian_ID = " + parentID;
                string sql = @"UPDATE Child SET ChildDeletionDate = @today WHERE Child_ID = @childID;";
                //SQLiteCommand command = new SQLiteCommand(sql, this.dbConn);
                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@childID", childID));
                command.ExecuteNonQuery();

                string sql2 = "DELETE from AllowedConnections where Child_ID = " + childID;
                SQLiteCommand command2 = new SQLiteCommand(sql2, dbCon);
                command2.CommandText = sql2;
                command2.ExecuteNonQuery();




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
                  "where Guardian_ID = " + id;

            // SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            // SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
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
    }
}
