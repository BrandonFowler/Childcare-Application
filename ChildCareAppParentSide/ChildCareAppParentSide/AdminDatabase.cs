using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace ChildCareAppParentSide {

    class AdminDatabase {
        private MySql.Data.MySqlClient.MySqlConnection conn;
        private string server;
        private string port;
        private string database;
        private string UID;
        private string password;
        private string connectionString;
        private Settings settings;

        public AdminDatabase() {
            this.settings = Settings.Instance;
            this.server = settings.server;
            this.port = settings.port;
            this.database = settings.databaseName;
            this.UID = settings.databaseUser;
            this.password = settings.databasePassword;
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

            try{
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();

                if (recordFound != DBNull.Value && recordFound != null){
                    conn.Close();
                    return true;
                }
                return false;
            }
            catch (MySqlException e){
                MessageBox.Show(e.ToString());
                conn.Close();
                return false;
            }
        }//end validateLogin

        public DataSet getParentInfo(string guardianID) {

            string sql = "select * from Guardian where Guardian_ID = @guardianID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@guardianID", guardianID));
            MySqlDataAdapter DB = new MySqlDataAdapter(command); 
            DataSet DS = new DataSet();

            try{
                conn.Open();
                DB.Fill(DS);
                conn.Close();
            }
            catch (MySqlException){
                MessageBox.Show("Database connection error: Unable to retrieve information for guardian");
                conn.Close();
            }
      
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

                MessageBox.Show("Completed deletion");
            }
            catch (MySqlException){
                MessageBox.Show("Database connection error: Failed to delete guardian");
            }
            
        }//end DeleteParentInfo

        public void updateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string city, string state, string zip, string imagePath) {
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
            catch (MySqlException) {
                MessageBox.Show("Database connection error: Failed to update guardian information");
                conn.Close();
            }
             
        }//end UpdateParentInfo

         public int getMaxID() {

            string sql = "SELECT MAX(Child_ID) FROM Child;";

            MySqlCommand command = new MySqlCommand(sql, conn);

            try{
                conn.Open();
                int max = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();

                return max;
            }
            catch{
                MessageBox.Show("Database connection error: Failed to retrieve critical information");
                conn.Close();
                return -1;
            }
        }//end getMaxID

        public int getMaxConnectionID(){
            
            string sql = "SELECT MAX(Allowance_ID) FROM AllowedConnections;";

            MySqlCommand command = new MySqlCommand(sql, conn);

            try{
                conn.Open();
                int max = Convert.ToInt32(command.ExecuteScalar());
                conn.Close();

                return max;
            }
            catch{
                MessageBox.Show("Database connection error: Failed to retrieve critical information");
                conn.Close();
                return -1;
            }
        }//end getMaxConnection

        public void addNewChild(string cID, string fName, string lName, string birthday, string allergies, string medical, string photo) {
            string imagePath = photo;

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

            try{
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch{
                MessageBox.Show("Database connection error: Unable to add new child");
                conn.Close();
            }
        }//end addNewChild

        public void updateAllowedConnections(string conID, string pID, string cID) {
            string familyID = pID.Remove(pID.Length - 1);
            
            try{

                string sql = "select Allowance_ID from AllowedConnections where Guardian_ID = @pID and Child_ID = @cID";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.Add(new MySqlParameter("@pID", pID));
                command.Parameters.Add(new MySqlParameter("@cID", cID));

                conn.Open();
                object recordExists = command.ExecuteScalar();
                conn.Close();

                if (recordExists != DBNull.Value && recordExists != null){
                    return;
                }

                sql = "INSERT INTO AllowedConnections(Allowance_ID, Guardian_ID, Child_ID, Family_ID) "
                                + "VALUES(@conID, @pID, @cID, @familyID);";

                command = new MySqlCommand(sql, conn);
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
                conn.Close();
                MessageBox.Show(e.ToString());
            }
            
        }//end upadateAllowedConnections

        public void updateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies, string imagePath) {
        
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
            catch (MySqlException){
                conn.Close();
                MessageBox.Show("Databse connection error: Unable to update child information");
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

                MessageBox.Show("Completed child deletion");
            }
            catch (MySqlException){
                MessageBox.Show("Database connection error: Failed to delete child");
                conn.Close();
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

            try{
                conn.Open();
                DB.Fill(DS);

                if (DS.Tables == null){
                    return null;
                }

                int cCount = DS.Tables[0].Columns.Count;
                int rCount = DS.Tables[0].Rows.Count;
                String[,] data = new string[rCount, cCount];

                for (int x = 0; x < rCount; x++){
                    for (int y = 0; y < cCount; y++){
                        data[x, y] = DS.Tables[0].Rows[x][y].ToString();
                    }
                }

                conn.Close();
                return data;
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to retrieve information for children");
                conn.Close();
                return null;
            }
        }//end findChildren 

        public bool IDExists(string ID) {

            string sql = "select Guardian_ID " +
                         "from Guardian " +
                         "where Guardian_ID = @ID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@ID", ID));

            try
            {
                conn.Open();
                object recordFound = command.ExecuteScalar();
                conn.Close();

                if (recordFound != DBNull.Value && recordFound != null){
                    conn.Close();
                    return true;
                }
            }
            catch(Exception){
                MessageBox.Show("Database connection error: Unable to check if ID exists");
                conn.Close();
            }

            return false;
        }//end IDExists

        public void addNewParent(string ID) {
            string familyID = ID.Remove(ID.Length-1);
            string imagePath = settings.photoPath + "/default.jpg";
            
            string sql = "INSERT INTO Guardian VALUES(@ID, '0000', 'First Name', 'Last Name', 'Unknown', 'Unknown', 'Unknown', 'Unknown', 'Unknown', '--', 'Unknown', @imagePath)";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@ID", ID));
            command.Parameters.Add(new MySqlParameter("@imagePath", imagePath));

            try
            {
                conn.Open();
                command.ExecuteNonQuery();

                sql = "select Family_ID from Family where Family_ID = @familyID";
                command = new MySqlCommand(sql, conn);
                command.Parameters.Add(new MySqlParameter("@familyID", familyID));

                object recordFound = command.ExecuteScalar();

                if (recordFound == DBNull.Value || recordFound == null){
                    sql = "INSERT INTO Family VALUES(" + familyID + ", 0)";
                    command = new MySqlCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception){
                MessageBox.Show("Database connection error: Unable to add new guardian");
                conn.Close();
            }

        }//end addNewParent

        public void editPin(string ID, string pin) {
            
            string sql = "update Guardian " +
                         "set GuardianPin = @pin " +
                         "where Guardian_ID = @ID";

            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add(new MySqlParameter("@pin", pin));
            command.Parameters.Add(new MySqlParameter("@ID", ID));

            try{
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException){
                MessageBox.Show("Database connection error: Unable to change pin");
            }
        }//end editPin

    }//end AdminDatabase(class)
}
