using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;
//using System.DateTime; 

namespace AdminTools {

    class LoadParentInfoDatabase {

        private SQLiteConnection dbCon;

        public LoadParentInfoDatabase() {
            dbCon = new SQLiteConnection("Data Source=../../Database/ChildCare_v3.s3db;Version=3;");
        }//end Database


        /*private MySql.Data.MySqlClient.MySqlConnection dbConn;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;

        public LoadParentInfoDatabase() {
            this.server = "146.187.135.22";
            this.port = "3306";
            this.database = "childcare_v5";
            this.UID = "ccdev";
            this.password = "devpw821";
            connectionString = "SERVER="+server+"; PORT="+port+"; DATABASE="+database+"; UID="+UID+"; PASSWORD="+password+";";
            dbConn = new MySql.Data.MySqlClient.MySqlConnection();
            dbConn.ConnectionString = connectionString;
        }//end Database(default constructor)*/

        public LoadParentInfoDatabase(string server, string port, string database, string UID, string password) {
            /*this.server = server;
            this.port = port;
            this.database = database;
            this.UID = UID;
            this.password = password;
            connectionString = server + "; PORT=" + port + "; DATABASE=" + database + "; UID=" + UID + "; PASSWORD=" + password + ";";
            dbConn = new MySql.Data.MySqlClient.MySqlConnection();
            dbConn.ConnectionString = connectionString;*/
        }//end Database

        public DataSet GetParentInfo(string parentID) {

            dbCon.Open();
            string sql = "select * from Guardian where Guardian_ID = " + parentID;
            //SQLiteCommand command = new SQLiteCommand(sql, this.conn);
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);

            //SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);


            dbCon.Close();
            return DS;
        }//end GetFirstName

        public void DeleteParentInfo(string parentID) {

            dbCon.Open();
            try {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                //string sql = "DELETE from Guardian where Guardian_ID = " + parentID;
                string sql = @"UPDATE Guardian SET GuardianDeletionDate = @today WHERE Guardian_ID = @parentID;";
                //SQLiteCommand command = new SQLiteCommand(sql, this.dbConn);
                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@parentID", parentID));

                command.ExecuteNonQuery();

                MessageBox.Show("Completed");
            } catch (SQLiteException e) {
                MessageBox.Show("Failed");
            }
            dbCon.Close();

        }//end GetFirstName

        public void AddNewParent(string ID, string PIN, string firstName, string lastName, string phone, string email, string address, string address2, string city, string state, string zip, string photo) {
            dbCon.Open();

            try {

                string sql = @"INSERT INTO Guardian(Guardian_ID, GuardianPIN, FirstName, LastName, Phone, Email, Address1, Address2, City, StateAbrv, Zip, PhotoLocation) " +
                "VALUES(" + ID + ", " + PIN + ", " + firstName + ", " + lastName + ", " + phone + ", " + email + ", " + address + ", " + address2 + ", " + city + ", " + state + ", " + zip + ", " + photo + ");";
                SQLiteCommand mycommand = new SQLiteCommand(sql, dbCon);
                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }


        public void UpdateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string address2, string city, string state, string zip) {
            dbCon.Open();

            try {

                string sql = @"UPDATE Guardian SET FirstName = @firstName, LastName = @lastName, Phone = @phone, Email = @email," +
                                    "Address1 = @address, Address2 = @address2, City = @city, StateAbrv = @state, Zip  = @zip WHERE Guardian_ID = @ID;";
                //SQLiteCommand mycommand = new SQLiteCommand(sql, this.dbConn);
                SQLiteCommand mycommand = new SQLiteCommand(sql, dbCon);
                mycommand.CommandText = sql;
                mycommand.Parameters.Add(new SQLiteParameter("@firstName", firstName));
                mycommand.Parameters.Add(new SQLiteParameter("@lastName", lastName));
                mycommand.Parameters.Add(new SQLiteParameter("@phone", phone));
                mycommand.Parameters.Add(new SQLiteParameter("@email", email));
                mycommand.Parameters.Add(new SQLiteParameter("@address", address));
                mycommand.Parameters.Add(new SQLiteParameter("@address2", address2));
                mycommand.Parameters.Add(new SQLiteParameter("@city", city));
                mycommand.Parameters.Add(new SQLiteParameter("@state", state));
                mycommand.Parameters.Add(new SQLiteParameter("@zip", zip));
                mycommand.Parameters.Add(new SQLiteParameter("@ID", ID));

                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }
        public DataSet checkIfFamilyExists(string familyID) {
            dbCon.Open();
            string sql = "select * from Family where Family_ID = " + familyID;
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);

            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);


            dbCon.Close();
            return DS;
        }
        public void AddNewFamily(string familyID, double ballance) {

            dbCon.Open();

            try {

                string sql = @"INSERT INTO Family(Family_ID, FamilyTotal) " +
                "VALUES(" + familyID + ", " + ballance + ");";
                SQLiteCommand mycommand = new SQLiteCommand(sql, dbCon);
                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
            } catch (SQLiteException e) {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }

    }//end LoadParentInfoDatabase

}//end namespace
