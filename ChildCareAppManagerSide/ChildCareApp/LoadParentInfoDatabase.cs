using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;  

namespace ChildCareApp {

    class LoadParentInfoDatabase {

        private SQLiteConnection dbCon;

        public LoadParentInfoDatabase()
        {
            dbCon = new SQLiteConnection("Data Source=../../ChildCare_v3.s3db;Version=3;");  
        }//end Database

        public DataSet GetParentInfo(string parentID) {

            dbCon.Open();
            string sql = "select * from Guardian where Guardian_ID = " + parentID;
            SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);

            SQLiteDataAdapter DB = new SQLiteDataAdapter(command);
            DataSet DS = new DataSet();
            DB.Fill(DS);


            dbCon.Close();        
            return DS; 
        }//end GetFirstName

        public void DeleteParentInfo(string parentID)
        {

            dbCon.Open();
            try
            {
                string sql = "DELETE from Guardian where Guardian_ID = " + parentID;
                SQLiteCommand command = new SQLiteCommand(sql, this.dbCon);
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

        public void UpdateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string address2, string city, string state, string zip) {
            dbCon.Open();

            try
            {
              
                    string sql = @"UPDATE Guardian SET FirstName = @firstName, LastName = @lastName, Phone = @phone, Email = @email,"+
                                        "Address1 = @address, Address2 = @address2, City = @city, State = @state, Zip  = @zip WHERE Guardian_ID = @ID;";
                SQLiteCommand mycommand = new SQLiteCommand(sql, this.dbCon);
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
            }

            catch (SQLiteException e)
            {
                MessageBox.Show(e.ToString()); 
            }
            dbCon.Close();  
        }
      

    }//end LoadParentInfoDatabase

}//end namespace
