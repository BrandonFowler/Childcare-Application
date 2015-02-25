using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace ChildCareAppParentSide {

    class AdminDatabase {
        private MySql.Data.MySqlClient.MySqlConnection conn;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;

        public AdminDatabase() {
            this.server = "146.187.135.22";
            this.port = "3306";
            this.database = "childcare_v5";
            this.UID = "ccdev";
            this.password = "devpw821";
            connectionString = "SERVER=" + server + "; PORT=" + port + "; DATABASE=" + database + "; UID=" + UID + "; PASSWORD=" + password + ";";
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionString;
        }//end AdminDatabase

        public bool validateAdmin(string userName, string password) {

            string sql = "select AccessLevel " +
                         "from Administrator " + 
                         "where AdministratorPW='" + password + "' and AdministratorUN='" + userName + "'";

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            object recordFound = command.ExecuteScalar();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            conn.Close();
            return false;
        }//end validateLogin

        public DataSet getParentInfo(string parentID) {

            conn.Open();
            string sql = "select * from Guardian where Guardian_ID = " + parentID;
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command); 
            DataSet DS = new DataSet();
            DB.Fill(DS);
            conn.Close();        
            return DS; 
        }//end GetParentInfo

        public void deleteParentInfo(string parentID){

            conn.Open();
            try{
                string sql = "DELETE from Guardian where Guardian_ID = " + parentID;
                
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.ExecuteNonQuery();
                MessageBox.Show("Completed");
            }
            catch (MySqlException){
                MessageBox.Show("Failed");
            }
            conn.Close();
            
        }//end DeleteParentInfo

        public void updateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string city, string state, string zip, string imageName) {
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\"+imageName;
            conn.Open();
            try{
              
                    string sql = @"UPDATE Guardian SET FirstName = @firstName, LastName = @lastName, Phone = @phone, Email = @email,"+
                                        "Address1 = @address, City = @city, StateAbrv = @state, Zip  = @zip, PhotoLocation = @imagePath WHERE Guardian_ID = @ID;";
               
                MySqlCommand mycommand = new MySqlCommand(sql, conn);
                mycommand.CommandText = sql;
                mycommand.Parameters.Add(new MySqlParameter("@firstName", firstName));
                mycommand.Parameters.Add(new MySqlParameter("@lastName", lastName));
                mycommand.Parameters.Add(new MySqlParameter("@phone", phone));
                mycommand.Parameters.Add(new MySqlParameter("@email", email));
                mycommand.Parameters.Add(new MySqlParameter("@address", address));
                mycommand.Parameters.Add(new MySqlParameter("@city", city));
                mycommand.Parameters.Add(new MySqlParameter("@state", state));
                mycommand.Parameters.Add(new MySqlParameter("@zip", zip));
                mycommand.Parameters.Add(new MySqlParameter("@ID", ID));
                mycommand.Parameters.Add(new MySqlParameter("@imagePath", imagePath));
                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
            }

            catch (MySqlException e) {
                MessageBox.Show(e.ToString()); 
            }
            conn.Close();  
        }//end UpdateParentInfo

         public int getMaxID() {
            conn.Open();
            string sql = "SELECT MAX(Child_ID) FROM Child;";
            MySqlCommand command = new MySqlCommand(sql, conn);
            int max = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return max;
        }//end getMaxID

        public int getMaxConnectionID(){
            conn.Open();
            string sql = "SELECT MAX(Allowance_ID) FROM AllowedConnections;";
            MySqlCommand command = new MySqlCommand(sql, conn);
            int max = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return max;
        }//end getMaxConnection

        public void addNewChild(string cID, string fName, string lName, string birthday, string allergies, string medical, string photo) {
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\"+photo;
            conn.Open();
            string sql = "INSERT INTO Child(Child_ID, FirstName, LastName, Birthday, Allergies, Medical, PhotoLocation) "
                         + "VALUES ('" + cID + "', '" + fName + "', '" + lName + "', '" + birthday + "', '" + allergies + "', '" + medical + "', '" + imagePath + "');";

                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.ExecuteNonQuery();
                conn.Close();
        }//end addNewChild

        public void updateAllowedConnections(string conID, string pID, string cID) {
            string familyID = pID.Remove(pID.Length - 1);
            conn.Open();
            try{
                string sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                + "VALUES(" + conID + ", " + pID + ", " + cID + ", " + familyID + ");";

                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;

                command.ExecuteNonQuery();
            }

            catch (MySqlException e){
                MessageBox.Show(e.ToString());
            }
            conn.Close();
        }//end upadateAllowedConnections

        public void updateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies, string imageName) {
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\" + imageName;
            conn.Open();
            try{
                string sql = @"UPDATE Child SET FirstName = @firstName, LastName = @lastName, Birthday = @birthday, Allergies = @allergies, Medical = @medical, PhotoLocation = @imagePath WHERE Child_ID = @ID;";
                
                MySqlCommand mycommand = new MySqlCommand(sql, conn);
                mycommand.CommandText = sql;
                mycommand.Parameters.Add(new MySqlParameter("@firstName", firstName));
                mycommand.Parameters.Add(new MySqlParameter("@lastName", lastName));
                mycommand.Parameters.Add(new MySqlParameter("@birthday", birthday));
                mycommand.Parameters.Add(new MySqlParameter("@allergies", allergies));
                mycommand.Parameters.Add(new MySqlParameter("@medical", medical));
                mycommand.Parameters.Add(new MySqlParameter("@ID", ID));
                mycommand.Parameters.Add(new MySqlParameter("@imagePath", imagePath));
                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
            }

            catch (MySqlException e){
                MessageBox.Show(e.ToString());
            }
            conn.Close();
        }//end UpdateChildInfo

        public void deleteChildInfo(string childID){
            conn.Open();
            try{
                string sql = "DELETE from AllowedConnections where Child_ID = " + childID;
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.ExecuteNonQuery();

                sql = "DELETE from Child where Child_ID = " + childID;
                command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.ExecuteNonQuery();
                MessageBox.Show("Completed");
            }
            catch (MySqlException){
                MessageBox.Show("Failed");
            }
            conn.Close();
        }//end deleteChildInfo

        public String[,] findChildren(string id) {
            conn.Open();

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where Guardian_ID = " + id;

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataAdapter DB = new MySqlDataAdapter(command);
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

            conn.Close();
            return data;
        }//end findChildren 

        public bool IDExists(string ID) {

            string sql = "select Guardian_ID " +
                         "from Guardian " +
                         "where Guardian_ID = " + ID;

            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            object recordFound = command.ExecuteScalar();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            conn.Close();
            return false;
        }//end IDExists

        public void addNewParent(string ID) {
            string familyID = ID.Remove(ID.Length-1);
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\";
            conn.Open();
            string sql = "INSERT INTO Guardian VALUES('" + ID + "', '0000', 'First Name', 'Last Name', 'Unknown', 'Unknown', 'Unknown', 'Unknown', 'Unknown', '--', 'Unknown', '" + imagePath + "default.jpg')";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();

            sql = "select Family_ID from Family where Family_ID = " + familyID;
            command = new MySqlCommand(sql, conn);
            object recordFound = command.ExecuteScalar();

            if (recordFound == DBNull.Value || recordFound == null) {
                sql = "INSERT INTO Family VALUES(" + familyID + ", 0)";
                command = new MySqlCommand(sql, conn);
                command.ExecuteNonQuery();
            }
            conn.Close();

        }//end addNewParent

        internal void editPin(string ID, string pin) {
            conn.Open();
            string sql = "update Guardian " +
                         "set GuardianPin = '" + pin + "' " +
                         "where Guardian_ID = '" + ID + "'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }//end editPin

    }//end AdminDatabase(class)
}
