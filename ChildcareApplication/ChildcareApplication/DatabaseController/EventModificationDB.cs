using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatabaseController {
    class EventModificationDB {

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, Double hourlyDiscount) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, null, null, null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, Double dailyDiscount) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', null, null, null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, Double hourlyDiscount, int eventMonth, int eventDay) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, Double dailyDiscount, int eventMonth, int eventDay) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, Double hourlyDiscount, String weekday) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, null, null, '" + weekday + "', null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, Double dailyDiscount, String weekday) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', null, null, '" + weekday + "', null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "',  null, null, null, null, null, null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', null, null, null, null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, int eventMonth, int eventDay) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', null, null, null, '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, int eventMonth, int eventDay) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', null, '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, String weekday) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', null, null, null, null, null, '" + weekday + "', null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, String weekday) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', null, null, null, '" + weekday + "', null, null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, Double hourlyDiscount, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", DailyPrice = null, DailyDiscount = null, EventMonth = null,";
            query += " EventWeekday = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, Double dailyDiscount, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", HourlyPrice = null, HourlyDiscount = null, EventMonth = null,";
            query += " EventDay = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, Double hourlyDiscount, int eventMonth, int eventDay, String oldEventName) { 
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", DailyPrice = null, DailyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, Double dailyDiscount, int eventMonth, int eventDay, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", HourlyPrice = null, HourlyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, Double hourlyDiscount, String weekday, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", EventWeekday = '" + weekday + "', DailyPrice = null,";
            query += " DailyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, Double dailyDiscount, String weekday, String oldEventName) { 
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", EventWeekday = '" + weekday + "', HourlyPrice = null,";
            query += " HourlyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = null, DailyPrice = null, DailyDiscount = null, EventMonth = null,";
            query += " EventWeekday = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, HourlyPrice = null, HourlyDiscount = null, EventMonth = null,";
            query += " EventDay = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, int eventMonth, int eventDay, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = null, EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", DailyPrice = null, DailyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, int eventMonth, int eventDay, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", HourlyPrice = null, HourlyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, String weekday, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = null, EventWeekday = '" + weekday + "', DailyPrice = null,";
            query += " DailyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, String weekday, String oldEventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, EventWeekday = '" + weekday + "', HourlyPrice = null,";
            query += " HourlyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public bool EventNameExists(string eventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            int count;

            if (eventName.Length < 1) {
                return false;
            }
            try {
                connection.Open();

                string query = "Select count(*) from EventData where EventName = '" + eventName + "';";

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

        public List<string> GetAllEventNames() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT EventName FROM EventData;";
            List<string> results = new List<string>();

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    String eventName = reader.GetString(0);
                    results.Add(eventName);
                }
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return results;
        }

        public DataTable GetEvents() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "SELECT EventName AS 'Event Name', HourlyPrice AS 'Hourly Price', HourlyDiscount AS 'Hourly Discount', ";
            query += "DailyPrice AS 'Daily Price', DailyDiscount AS 'Daily Discount', EventMonth AS 'Event Month', ";
            query += "EventDay AS 'Event Day', EventWeekday AS 'Event Weekday', EventMaximumHours AS 'Maximum Hours' FROM EventData WHERE EventDeletionDate IS null;";
            DataTable table;

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                table = new DataTable("Event Info");
                adapter.Fill(table);

                connection.Close();
                return table;
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
                return null;
            }
        }

        public string[] GetEvent(string eventName) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            string[] eventInfo = new string[10];
            try {
                connection.Open();
                String query = "SELECT * FROM EventData WHERE EventName = '" + eventName + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                eventInfo = FillEventInfo(eventInfo, reader);

              connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            return eventInfo;
        }

        private string[] FillEventInfo(string[] eventInfo, SQLiteDataReader reader) {
            if (!reader.IsDBNull(0)) {
                eventInfo[0] = reader.GetString(0);
            } else {
                eventInfo[0] = null;
            }
            if (!reader.IsDBNull(1)) {
                eventInfo[1] = "" + reader.GetDouble(1);
            } else {
                eventInfo[1] = null;
            }
            if (!reader.IsDBNull(2)) {
                eventInfo[2] = "" + reader.GetDouble(2);
            } else {
                eventInfo[2] = null;
            }
            if (!reader.IsDBNull(3)) {
                eventInfo[3] = "" + reader.GetDouble(3);
            } else {
                eventInfo[3] = null;
            }
            if (!reader.IsDBNull(4)) {
                eventInfo[4] = "" + reader.GetDouble(4);
            } else {
                eventInfo[4] = null;
            }
            if (!reader.IsDBNull(5)) {
                eventInfo[5] = "" + reader.GetInt32(5);
            } else {
                eventInfo[5] = null;
            }
            if (!reader.IsDBNull(6)) {
                eventInfo[6] = "" + reader.GetInt32(6);
            } else {
                eventInfo[6] = null;
            }
            if (!reader.IsDBNull(7)) {
                eventInfo[7] = reader.GetString(7);
            } else {
                eventInfo[7] = null;
            }
            if (!reader.IsDBNull(8)) {
                eventInfo[8] = "" + reader.GetInt32(8);
            } else {
                eventInfo[8] = null;
            }
            if (!reader.IsDBNull(9)) {
                eventInfo[9] = reader.GetString(9);
            } else {
                eventInfo[9] = null;
            }
            return eventInfo;
        }

        public void UpdateMaxHours(string eventName, string maxHours) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = "UPDATE EventData SET EventMaximumHours = " + maxHours + " Where EventName = '" + eventName + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
