using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChildcareApplication.AdminTools {
    class EventModificationDB {

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, Double hourlyDiscount) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + GetNextEventID() + "', '" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, null, null, null, null);";
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
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + GetNextEventID() + "', '" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', null, null, null, null);";
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
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + GetNextEventID() + "', '" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, '" + eventMonth + "', '" + eventDay + "', null, null);";
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
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + GetNextEventID() + "', '" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', '" + eventMonth + "', '" + eventDay + "', null, null);";
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
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + GetNextEventID() + "', '" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, null, null, '" + weekday + "', null);";
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
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "INSERT INTO EventData VALUES ('" + GetNextEventID() + "', '" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', null, null, '" + weekday + "', null);";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private String GetNextEventID() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String eventID = "";
            int eventNum;
            String query = "SELECT MAX(Event_ID) FROM EventData;";

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                eventID = Convert.ToString(cmd.ExecuteScalar());
                eventNum = Convert.ToInt32(eventID);
                eventNum++;
                eventID = "" + eventNum;

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            while (eventID.Length < 6) {
                eventID = "0" + eventID;
            }
            return eventID;
        }
        
        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, Double hourlyDiscount, String eventID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + " DailyPrice = null, DailyDiscount = null, EventMonth = null,";
            query += " EventWeekday = null WHERE Event_ID = '" + eventID + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, Double dailyDiscount, String eventID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + " HourlyPrice = null, HourlyDiscount = null, EventMonth = null,";
            query += " EventDay = null,  EventWeekday = null WHERE Event_ID = '" + eventID + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, Double hourlyDiscount, int eventMonth, int eventDay, String eventID) { 
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + " EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", DailyPrice = null, DailyDiscount = null, EventWeekday = null WHERE Event_ID = '" + eventID + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, Double dailyDiscount, int eventMonth, int eventDay, String eventID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", HourlyPrice = null, HourlyDiscount = null, EventWeekday = null WHERE Event_ID = '" + eventID + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, Double hourlyDiscount, String weekday, String eventID) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", EventWeekday = '" + weekday + "', DailyPrice = null,";
            query += " DailyDiscount = null WHERE Event_ID = '" + eventID + "';";
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, Double dailyDiscount, String weekday, String eventID) { 
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", EventWeekday = '" + weekday + "', HourlyPrice = null,";
            query += " HourlyDiscount = null, EventMonth = null, EventDay = null WHERE Event_ID = '" + eventID + "';";
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
