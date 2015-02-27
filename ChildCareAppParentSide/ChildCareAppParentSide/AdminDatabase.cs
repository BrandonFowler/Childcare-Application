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
                         "where AdministratorPW= @password and AdministratorUN= @userName";

            
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@password", password));
            command.Parameters.Add(new MySqlParameter("@userName", userName));

            conn.Open();
            object recordFound = command.ExecuteScalar();
            conn.Close();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }
            return false;
        }//end validateLogin

        public DataSet getParentInfo(string guardianID) {

            string sql = "select * from Guardian where Guardian_ID = @guardianID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));
            MySqlDataAdapter DB = new MySqlDataAdapter(command); 
            DataSet DS = new DataSet();

            conn.Open();
            DB.Fill(DS);
            conn.Close();  
      
            return DS; 
        }//end GetParentInfo

        public void deleteParentInfo(string guardianID){

            try{
                string sql = "DELETE from Guardian where Guardian_ID = @guardianID";
                
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Completed");
            }
            catch (MySqlException){
                MessageBox.Show("Failed");
            }
            
        }//end DeleteParentInfo

        public void updateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string city, string state, string zip, string imageName) {
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\"+imageName;
            try{
              
                string sql = @"UPDATE Guardian "+
                              "SET FirstName = @firstName, LastName = @lastName, Phone = @phone, Email = @email,"+
                              "Address1 = @address, City = @city, StateAbrv = @state, Zip  = @zip, PhotoLocation = @imagePath "+ 
                              "WHERE Guardian_ID = @ID;";
               
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@firstName", firstName));
                command.Parameters.Add(new MySqlParameter("@lastName", lastName));
                command.Parameters.Add(new MySqlParameter("@phone", phone));
                command.Parameters.Add(new MySqlParameter("@email", email));
                command.Parameters.Add(new MySqlParameter("@address", address));
                command.Parameters.Add(new MySqlParameter("@city", city));
                command.Parameters.Add(new MySqlParameter("@state", state));
                command.Parameters.Add(new MySqlParameter("@zip", zip));
                command.Parameters.Add(new MySqlParameter("@ID", ID));
                command.Parameters.Add(new MySqlParameter("@imagePath", imagePath));

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close(); 

                MessageBox.Show("Completed");
            }
            catch (MySqlException e) {
                MessageBox.Show(e.ToString()); 
            }
             
        }//end UpdateParentInfo

         public int getMaxID() {

            string sql = "SELECT MAX(Child_ID) FROM Child;";

            MySqlCommand command = new MySqlCommand(sql, conn);

            conn.Open();
            int max = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            return max;
        }//end getMaxID

        public int getMaxConnectionID(){
            
            string sql = "SELECT MAX(Allowance_ID) FROM AllowedConnections;";

            MySqlCommand command = new MySqlCommand(sql, conn);

            conn.Open();
            int max = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            return max;
        }//end getMaxConnection

        public void addNewChild(string cID, string fName, string lName, string birthday, string allergies, string medical, string photo) {
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\"+photo;

            string sql = "INSERT INTO Child(Child_ID, FirstName, LastName, Birthday, Allergies, Medical, PhotoLocation) "
                         + "VALUES (@cID, @fName, @lName, @birthday, @allergies, @medical, @photo);";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.CommandText = sql;
            command.Parameters.Add(new MySqlParameter("@cID", cID));
            command.Parameters.Add(new MySqlParameter("@fName", fName));
            command.Parameters.Add(new MySqlParameter("@lName", lName));
            command.Parameters.Add(new MySqlParameter("@birthday", birthday));
            command.Parameters.Add(new MySqlParameter("@allergies", allergies));
            command.Parameters.Add(new MySqlParameter("@medical", medical));
            command.Parameters.Add(new MySqlParameter("@photo", photo));

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }//end addNewChild

        public void updateAllowedConnections(string conID, string pID, string cID) {
            string familyID = pID.Remove(pID.Length - 1);
            
            try{
                string sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                + "VALUES(@conID, @pID, @cID, @familyID);";

                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@conID", conID));
                command.Parameters.Add(new MySqlParameter("@pID", pID));
                command.Parameters.Add(new MySqlParameter("@cID", cID));
                command.Parameters.Add(new MySqlParameter("@familyID", familyID));

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }

            catch (MySqlException e){
                MessageBox.Show(e.ToString());
            }
            
        }//end upadateAllowedConnections

        public void updateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies, string imageName) {
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\" + imageName;
            
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

                conn.Open();
                mycommand.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Completed");
            }
            catch (MySqlException e){
                MessageBox.Show(e.ToString());
            }
            
        }//end UpdateChildInfo

        public void deleteChildInfo(string childID, string guardianID){
            
            try{
                string sql = "DELETE from AllowedConnections where Child_ID = @childID and Guardian_ID = @guardianID";

                MySqlCommand command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@childID", childID));
                command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));

                conn.Open();
                command.ExecuteNonQuery();

                sql = "DELETE from Child where Child_ID = @childID";
                command = new MySqlCommand(sql, conn);
                command.CommandText = sql;
                command.Parameters.Add(new MySqlParameter("@childID", childID));

                command.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Completed");
            }
            catch (MySqlException){
                MessageBox.Show("Failed");
            }
            conn.Close();
        }//end deleteChildInfo

        public String[,] findChildren(string ID) {

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where Guardian_ID = @ID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@ID", ID));

            MySqlDataAdapter DB = new MySqlDataAdapter(command);
            DataSet DS = new DataSet();

            conn.Open();
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
                         "where Guardian_ID = @ID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@ID", ID));

            conn.Open();
            object recordFound = command.ExecuteScalar();
            conn.Close();

            if (recordFound != DBNull.Value && recordFound != null) {
                conn.Close();
                return true;
            }

            return false;
        }//end IDExists

        public void addNewParent(string ID) {
            string familyID = ID.Remove(ID.Length-1);
            string imagePath = "..\\\\..\\\\..\\\\..\\\\Photos\\\\" + "default.jpg";
            
            string sql = "INSERT INTO Guardian VALUES(@ID, '0000', 'First Name', 'Last Name', 'Unknown', 'Unknown', 'Unknown', 'Unknown', 'Unknown', '--', 'Unknown', @imagePath)";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@ID", ID));
            command.Parameters.Add(new MySqlParameter("@imagePath", imagePath));

            conn.Open();
            command.ExecuteNonQuery();

            sql = "select Family_ID from Family where Family_ID = @familyID";
            command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@familyID", familyID));

            object recordFound = command.ExecuteScalar();
            
            if (recordFound == DBNull.Value || recordFound == null) {
                sql = "INSERT INTO Family VALUES(" + familyID + ", 0)";
                command = new MySqlCommand(sql, conn);
                command.ExecuteNonQuery();
            }
            conn.Close();

        }//end addNewParent

        public void editPin(string ID, string pin) {
            
            string sql = "update Guardian " +
                         "set GuardianPin = @pin " +
                         "where Guardian_ID = @ID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@pin", pin));
            command.Parameters.Add(new MySqlParameter("@ID", ID));

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }//end editPin

    }//end AdminDatabase(class)
}
