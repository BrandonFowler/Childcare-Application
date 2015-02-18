using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;  

namespace ChildCareApp
{
    class ChildInfoDatabse
    {

        private SQLiteConnection dbCon;

        public ChildInfoDatabse()
        {
            dbCon = new SQLiteConnection("Data Source=../../ChildCare_v3.s3db;Version=3;");  
        }//end Database


        public DataSet GetChildInfo(string parentID)
        {

            dbCon.Open();
            string sql = "SELECT * FROM child, allowedconnections, guardian " +
                            "WHERE allowedconnections.Guardian_ID = " + parentID +
                            " AND allowedconnections.Child_ID = child.Child_ID" +
                            " GROUP BY child.Child_ID;";
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);

            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);


            dbCon.Close();
            return DS;
        }//end GetFirstName

        public void UpdateChildInfo(string ID, string firstName, string lastName, string birthday, string medical, string allergies)
        {
            dbCon.Open();

            try
            {

                string sql = @"UPDATE Child SET FirstName = @firstName, LastName = @lastName, Allergies = @allergies, Medical = @medical WHERE Child_ID = @ID;";
                SQLiteCommand mycommand = new SQLiteCommand(sql, this.dbCon);
                mycommand.CommandText = sql;
                mycommand.Parameters.Add(new SQLiteParameter("@firstName", firstName));
                mycommand.Parameters.Add(new SQLiteParameter("@lastName", lastName));
                mycommand.Parameters.Add(new SQLiteParameter("@birthday", birthday));
                mycommand.Parameters.Add(new SQLiteParameter("@allergies", allergies));
                mycommand.Parameters.Add(new SQLiteParameter("@medical", medical));

                mycommand.Parameters.Add(new SQLiteParameter("@ID", ID));

                mycommand.ExecuteNonQuery();
                MessageBox.Show("Completed");
            }

            catch (SQLiteException e)
            {
                MessageBox.Show(e.ToString());
            }
            dbCon.Close();
        }

        public void DeleteChildInfo(string childID)
        {

            dbCon.Open();
            try
            {
                string sql = "DELETE from Child where Child_ID = " + childID;
                SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
                command.CommandText = sql;
                command.ExecuteNonQuery();

                sql = "DELETE from AllowedConnections where Child_ID = " + childID;
                command = new SQLiteCommand(sql, this.dbCon);
                command.CommandText = sql;
                command.ExecuteNonQuery();

                MessageBox.Show("Completed");
            }
            catch (SQLiteException e)
            {
                MessageBox.Show("Failed");
            }
            dbCon.Close();

        }//end GetFirstName

        public String[,] findChildren(string id) {
            dbCon.Open();

            string sql = "select Child.* " +
                  "from AllowedConnections join Child on Child.Child_ID = AllowedConnections.Child_ID " +
                  "where Guardian_ID = " + id;

            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
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
