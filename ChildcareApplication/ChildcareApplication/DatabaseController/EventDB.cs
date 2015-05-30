using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace DatabaseController {
    class EventDB {
        private SQLiteConnection dbCon;

        public EventDB() {
            this.dbCon = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }
        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, Double hourlyDiscount) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, null, null, null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, Double dailyDiscount) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', null, null, null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, Double hourlyDiscount, int eventMonth, int eventDay) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, Double dailyDiscount, int eventMonth, int eventDay) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, Double hourlyDiscount, String weekday) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', '" + hourlyDiscount + "', null, null, null, null, '" + weekday + "', null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, Double dailyDiscount, String weekday) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', '" + dailyDiscount + "', null, null, '" + weekday + "', null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "',  null, null, null, null, null, null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', null, null, null, null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, int eventMonth, int eventDay) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', null, null, null, '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, int eventMonth, int eventDay) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', null, '" + eventMonth + "', '" + eventDay + "', null, null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, String weekday) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', '" + hourlyPrice;
            query += "', null, null, null, null, null, '" + weekday + "', null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, String weekday) {
            String query = "INSERT INTO EventData VALUES ('" + eventName + "', null ";
            query += ", null, '" + dailyPrice + "', null, null, null, '" + weekday + "', null, null);";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, Double hourlyDiscount, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", DailyPrice = null, DailyDiscount = null, EventMonth = null,";
            query += " EventWeekday = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, Double dailyDiscount, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", HourlyPrice = null, HourlyDiscount = null, EventMonth = null,";
            query += " EventDay = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, Double hourlyDiscount, int eventMonth, int eventDay, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", DailyPrice = null, DailyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, Double dailyDiscount, int eventMonth, int eventDay, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", HourlyPrice = null, HourlyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, Double hourlyDiscount, String weekday, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = " + hourlyDiscount + ", EventWeekday = '" + weekday + "', DailyPrice = null,";
            query += " DailyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, Double dailyDiscount, String weekday, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = " + dailyDiscount + ", EventWeekday = '" + weekday + "', HourlyPrice = null,";
            query += " HourlyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceAlwaysAvailable(String eventName, Double hourlyPrice, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = null, DailyPrice = null, DailyDiscount = null, EventMonth = null,";
            query += " EventWeekday = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceAlwaysAvailable(String eventName, Double dailyPrice, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, HourlyPrice = null, HourlyDiscount = null, EventMonth = null,";
            query += " EventDay = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceSpecificDay(String eventName, Double hourlyPrice, int eventMonth, int eventDay, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = null, EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", DailyPrice = null, DailyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceSpecificDay(String eventName, Double dailyPrice, int eventMonth, int eventDay, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, EventMonth = " + eventMonth + ", EventDay = " + eventDay;
            query += ", HourlyPrice = null, HourlyDiscount = null, EventWeekday = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void HourlyPriceWeeklyOcur(String eventName, Double hourlyPrice, String weekday, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', HourlyPrice = " + hourlyPrice + ", ";
            query += "HourlyDiscount = null, EventWeekday = '" + weekday + "', DailyPrice = null,";
            query += " DailyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public void DailyPriceWeeklyOcur(String eventName, Double dailyPrice, String weekday, String oldEventName) {
            String query = "UPDATE EventData SET EventName = '" + eventName + "', DailyPrice = " + dailyPrice + ", ";
            query += "DailyDiscount = null, EventWeekday = '" + weekday + "', HourlyPrice = null,";
            query += " HourlyDiscount = null, EventMonth = null, EventDay = null WHERE EventName = '" + oldEventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public bool EventNameExists(string eventName) {
            int count;

            if (eventName.Length < 1) {
                return false;
            }
            try {
                dbCon.Open();

                string query = "Select count(*) from EventData where EventName = '" + eventName + "';";

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

        public List<string> GetAllEventNames() {
            String query = "SELECT EventName FROM EventData Where EventDeletionDate IS null;";
            List<string> results = new List<string>();

            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    String eventName = reader.GetString(0);
                    results.Add(eventName);
                }
                reader.Close();
                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
            return results;
        }

        public DataTable GetEventDisplay() {
            String query = "SELECT EventName AS 'Event Name', HourlyPrice AS 'Hourly Price', HourlyDiscount AS 'Hourly Discount', ";
            query += "DailyPrice AS 'Daily Price', DailyDiscount AS 'Daily Discount', EventMonth AS 'Event Month', ";
            query += "EventDay AS 'Event Day', EventWeekday AS 'Event Weekday', EventMaximumHours AS 'Maximum Hours' FROM EventData WHERE EventDeletionDate IS null;";
            DataTable table;

            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                table = new DataTable("Event Info");
                adapter.Fill(table);

                dbCon.Close();
                return table;
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
                return null;
            }
        }

        public string[] GetEvent(string eventName) {
            string sql = "select * " +
                  "from EventData " +
                  "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            try {
                dbCon.Open();
                DB.Fill(DS);
                int cCount = DS.Tables[0].Columns.Count;
                string[] eventData = new string[cCount];
                for (int x = 0; x < cCount; x++) {
                    eventData[x] = DS.Tables[0].Rows[0][x].ToString();
                }
                dbCon.Close();
                return eventData;
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible. Please insure the charge was calculated correctly.");
                dbCon.Close();
                return null;
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve critical information. Please insure charge was calculated correctly.");
                return null;
            }
        }

        public void UpdateMaxHours(string eventName, string maxHours) {
            String query = "UPDATE EventData SET EventMaximumHours = " + maxHours + " Where EventName = '" + eventName + "';";
            try {
                dbCon.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, dbCon);
                cmd.ExecuteNonQuery();

                dbCon.Close();
            } catch (Exception exception) {
                WPFMessageBox.Show(exception.Message);
            }
        }

        public double GetEventHourCap(string eventName) {
            String sql = "select EventMaximumHours " +
                         "from EventData " +
                         "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            double eventHasNoMax = 24;
            try {
                dbCon.Open();
                object recordFound = command.ExecuteScalar();
                dbCon.Close();
                if (recordFound != DBNull.Value && recordFound != null) {
                    return Convert.ToDouble(recordFound);
                }
                return eventHasNoMax;
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return eventHasNoMax;
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve event specifications. Possible late fee calculations could be incorrect.");
                return eventHasNoMax;
            }
        }

        public string[] GetCurrentEvents() {
            DateTime dt = DateTime.Now;
            string dateTime = DateTime.Now.ToString();
            string month = Convert.ToDateTime(dateTime).ToString("MM");
            string day = Convert.ToDateTime(dateTime).ToString("dd");
            string dayOfWeek = dt.DayOfWeek.ToString();
            string sql = "select EventName " +
                         "from EventData " +
                         "where ((EventDay= @day and EventMonth= @month or " +
                         "EventWeekday= @dayOfWeek or EventName='Regular Childcare') " +
                         "and EventDeletionDate is NULL) or (EventDay is NULL and EventMonth is NULL " +
                         "and EventWeekday is NULL and EventDeletionDate is NULL and " +
                         "EventName != 'Late Fee' and EventName != 'Infant Childcare' and EventName != 'Adolescent Childcare')";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            command.Parameters.Add(new SQLiteParameter("@day", day));
            command.Parameters.Add(new SQLiteParameter("@month", month));
            command.Parameters.Add(new SQLiteParameter("@dayOfWeek", dayOfWeek));
            DataSet DS = new DataSet();
            try {
                dbCon.Open();
                DB.Fill(DS);
                int rCount = DS.Tables[0].Rows.Count;
                string[] events = new string[rCount];
                for (int x = 0; x < rCount; x++) {
                    events[x] = DS.Tables[0].Rows[x][0].ToString();
                }
                dbCon.Close();
                return events;
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
                return null;
            } catch (Exception) {
                WPFMessageBox.Show("Unable to retrieve information for events");
                dbCon.Close();
                return null;
            }
        }

        public double GetLateFee(string eventName) {
            string sql = "select HourlyPrice " +
                         "from EventData " +
                         "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            try {
                dbCon.Open();
                string fee = Convert.ToString(command.ExecuteScalar());
                dbCon.Close();

                double lateFee = Convert.ToDouble(fee);
                return lateFee;
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible. Any late fees have not been recorded.");
                dbCon.Close();
                return 0;
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve critical information. Any late fees have not been recorded.");
                return 0;
            }
        }

        public void DeleteEvent(string eventName) {
            string sql = "Update EventData Set EventDeletionDate = @date " +
                         "where EventName = @eventName";
            SQLiteCommand command = new SQLiteCommand(sql, dbCon);
            command.Parameters.Add(new SQLiteParameter("@eventName", eventName));
            command.Parameters.Add(new SQLiteParameter("@date", DateTime.Now.ToString("yyyy-MM-dd")));
            try {
                dbCon.Open();
                command.ExecuteNonQuery();
                dbCon.Close();
            } catch (System.Data.SQLite.SQLiteException) {
                WPFMessageBox.Show("Database connection error. Please insure the database exists, and is accessible.");
                dbCon.Close();
            } catch (Exception) {
                dbCon.Close();
                WPFMessageBox.Show("Unable to retrieve critical information.");
            }
        }
    }
}
