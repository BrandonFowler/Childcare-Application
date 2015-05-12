using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBoxUtils;

namespace DatabaseController {
    class GuardianInfoDB {
        private SQLiteConnection dbCon;
        public GuardianInfoDB() {
            this.dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        public String GetParentName(String parentID) {
            String result = "";

            try {
                dbCon.Open();

                String query = "SELECT FirstName, LastName FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                result = reader.GetString(0) + " " + reader.GetString(1);

                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetAddress1(String parentID) {
            String query = "SELECT Address1 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            String result = "";
            try {

                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                dbCon.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetAddress2(String parentID) {
            String query = "SELECT Address2 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            String result = "";
            try {
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                dbCon.Open();

                result = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return result;
        }

        //returns a string for the state, zip, and city
        public String GetAddress3(String parentID) {
            String query = "SELECT City, StateAbrv, Zip FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            String result = "";

            try {
                dbCon.Open();

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                result = reader.GetString(0) + ", " + reader.GetString(1) + " " + reader.GetString(2);
                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetPhoneNumber(String parentID) {
            String query = "SELECT Phone FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            String result = "";

            try {
                dbCon.Open();

                result = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetPhotoPath(String parentID) {
            String query = "SELECT PhotoLocation FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            String result = "";

            try {
                dbCon.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return result;
        }

        public String GetCurrentDue(String parentID) {
            String familyID = parentID.Remove(parentID.Length - 1);

            String query = "SELECT FamilyTotal FROM Family WHERE Family_ID = '" + familyID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

            String curDue = "$";

            try {
                dbCon.Open();
                curDue += Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }

            if (curDue.IndexOf('.') == curDue.Length - 2) {
                curDue += "0";
            } else if (!curDue.Contains('.')) {
                curDue += ".00";
            }
            return curDue;
        }

        public bool GuardianIDExists(string guardianID) {
            String query = "SELECT Guardian_ID FROM Guardian WHERE Guardian_ID = '" + guardianID + "';";
            SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
            String result = "";

            try {
                dbCon.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }

            if( result == guardianID) {
                return true;
            } else {
                return false;
            }
        }

        public bool GuardianNameExists(string fullName) {
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
                dbCon.Open();

                string query = "Select count(*) from Guardian where FirstName = '" + firstName + "' and LastName = '" + lastName + "';";

                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0) {
                    return true;
                }
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return false;
        }

        public DataSet GetParentInfoDS(string parentID) {
            DataSet DS = new DataSet();
            try {
                dbCon.Open();


                string sql = "select * from Guardian where Guardian_ID = @parentID";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.Parameters.Add(new SQLiteParameter("@parentID", parentID));
                SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

                DB.Fill(DS);
            } catch (SQLiteException e) {
                WPFMessageBox.Show(e.Message);
            }
            dbCon.Close();
            return DS;
        }

        public void DeleteParentInfo(string parentID) {

            dbCon.Open();
            try {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = @"UPDATE Guardian SET GuardianDeletionDate = @today WHERE Guardian_ID = @parentID;";

                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.CommandText = sql;
                command.Parameters.Add(new SQLiteParameter("@today", today));
                command.Parameters.Add(new SQLiteParameter("@parentID", parentID));

                command.ExecuteNonQuery();

                WPFMessageBox.Show("Completed");
            } catch (SQLiteException e) {
                WPFMessageBox.Show(e.Message);
            }
            dbCon.Close();

        }

        public void AddNewParent(string ID, string PIN, string firstName, string lastName, string phone, string email, string address, string address2, string city, string state, string zip, string photo) {
            

            try {
                dbCon.Open();
                string sql = @"INSERT INTO Guardian(Guardian_ID, GuardianPIN, FirstName, LastName, Phone, Email, Address1, Address2, City, StateAbrv, Zip, PhotoLocation) " +
                "VALUES('" + ID + "', " + PIN + ", " + firstName + ", " + lastName + ", " + phone + ", " + email + ", " + address + ", " + address2 + ", " + city + ", " + state + ", " + zip + ", " + photo + ");";
                SQLiteCommand mycommand = new SQLiteCommand(sql, dbCon);
                mycommand.ExecuteNonQuery();
                WPFMessageBox.Show("Completed");
            } catch (SQLiteException e) {
                WPFMessageBox.Show(e.Message);
            }
            dbCon.Close();
        }

        public void UpdateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string address2, string city, string state, string zip, string path) {
            

            try {
                dbCon.Open();
                string sql = @"UPDATE Guardian SET FirstName = @firstName, LastName = @lastName, Phone = @phone, Email = @email," +
                                    "Address1 = @address, Address2 = @address2, City = @city, StateAbrv = @state, Zip  = @zip, PhotoLocation = @path WHERE Guardian_ID = @ID;";

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
                mycommand.Parameters.Add(new SQLiteParameter("@path", path));

                mycommand.ExecuteNonQuery();
                WPFMessageBox.Show("Completed");
            } catch (SQLiteException e) {
                WPFMessageBox.Show(e.Message);
            }
            dbCon.Close();
        }

        public string CheckIfFamilyExists(string familyID) {
            string result = ""; 
            try {

                dbCon.Open();
                string sql = "select * from Family where Family_ID = @familyID;";
                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
                SQLiteDataAdapter DB = new SQLiteDataAdapter(command);

                object res = command.ExecuteScalar();
                if (res != DBNull.Value) {

                    dbCon.Close();
                    return (string)res;

                }


            } catch (SQLiteException e) {
                WPFMessageBox.Show(e.Message);
            }

            dbCon.Close();
            return result;
        }

        public void AddNewFamily(string familyID, double ballance) {

            try {
                dbCon.Open();
                string sql = @"INSERT INTO Family(Family_ID, FamilyTotal) " +
                "VALUES(@familyID, @ballance);";
                SQLiteCommand command = new SQLiteCommand(sql, dbCon);
                command.Parameters.Add(new SQLiteParameter("@familyID", familyID));
                command.Parameters.Add(new SQLiteParameter("@ballance", ballance));

                command.ExecuteNonQuery();

            } catch (SQLiteException e) {
                WPFMessageBox.Show(e.Message);
            }
            dbCon.Close();
        }

        public string[] GetParentInfo(String guardianID) {
            string sql = "select * " +
                         "from Guardian " +
                         "where Guardian_ID = @guardianID";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@guardianID", guardianID));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try {
                dbCon.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                string[] data = new String[cCount];
                for (int x = 0; x < cCount; x++) {
                    data[x] = DS.Tables[0].Rows[0][x].ToString();
                }
                dbCon.Close();
                return data;
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return null;
            }catch (Exception) {
                WPFMessageBox.Show("Unable to retrieve information for guardian");
                dbCon.Close();
                return null;
            }
        }

        public DataTable RetieveGuardiansByLastName(string name) {
            string sql = "select LastName as 'First Name', FirstName as 'Last Name', Guardian_ID as ID " +
                         "from Guardian " +
                         "where upper(LastName) = upper(@name)";
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            DataTable table;
            try
            {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.Parameters.Add(new SQLiteParameter("@name", name));
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                table = new DataTable("Parent Report");
                adapter.Fill(table);
                connection.Close();
                return table;
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return null;
            }catch (Exception) {
                WPFMessageBox.Show("Unable to retrieve information for guardians");
                dbCon.Close();
                return null;
            }
        }

        public bool ValidateGuardianID(string ID) {
            String sql = "select Guardian_ID " +
                         "from Guardian " +
                         "where Guardian_ID = @ID";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
            try {
                dbCon.Open();
                object recordFound = command.ExecuteScalar();
                dbCon.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    return true;
                }
                return false;
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return false;
            }catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve guardian information.");
                return false;
            }
        }

        public string GetGuardianImagePath(string ID) {
            String sql = "select PhotoLocation " +
                        "from Guardian " +
                        "where Guardian_ID = @ID";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
            try {
                dbCon.Open();
                object recordFound = command.ExecuteScalar();
                dbCon.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    return (string)recordFound;
                }
                return null;
            }catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return null;
            }catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve guardian picture.");
                return null;
            }
        }
    }
}
