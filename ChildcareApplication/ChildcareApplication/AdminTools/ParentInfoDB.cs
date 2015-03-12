using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminTools {
    class ParentInfoDB {
        private MySqlConnection connection;

        public ParentInfoDB() {
            this.connection = new MySqlConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");
            try {
                connection.Open();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public String GetParentName(String parentID) {
            String query = "SELECT FirstName, LastName FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            String result = reader.GetString("FirstName") + " " + reader.GetString("LastName");

            reader.Close();
            return result;
        }

        public String GetAddress1(String parentID) {
            String query = "SELECT Address1 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        public String GetAddress2(String parentID) {
            String query = "SELECT Address2 FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        //returns a string for the state, zip, and city
        public String GetAddress3(String parentID) {
            String query = "SELECT City, State, Zip FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            String result = reader.GetString("City") + ", " + reader.GetString("State") + " " + reader.GetString("Zip");

            reader.Close();
            return result;
        }

        public String GetPhoneNumber(String parentID) {
            String query = "SELECT Phone FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        public String GetPhotoPath(String parentID) {
            String query = "SELECT PhotoLocation FROM Guardian WHERE Guardian_ID = '" + parentID + "';";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            return Convert.ToString(cmd.ExecuteScalar());
        }

        ~ParentInfoDB() {
            this.connection.Close();
        }
    }
}
