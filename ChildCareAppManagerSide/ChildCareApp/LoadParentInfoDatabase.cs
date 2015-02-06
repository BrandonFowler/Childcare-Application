using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data; 

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

        public void UpdateParentInfo(string ID, string firstName, string lastName, string phone, string email, string address, string city, string state, string zip)
        {
            dbCon.Open();
            /*  //SYNTAX erron somewhere in sql statement......
            string sql = "UPDATE Guardian SET FirstName = " + firstName + ", LastName = " + lastName + ", Phone = " + phone + ", Email = " + email +
                        ", Address1 = " +address+ ", City = " +city+ ", State = " +state+ ", Zip  =" +zip+ "WHERE Guardian_ID =" +ID+";";
           
            SQLiteCommand mycommand = new SQLiteCommand(sql, this.dbCon);
            mycommand.CommandText = sql;
            mycommand.ExecuteNonQuery(); 

            */
            dbCon.Close();  
        }
      

    }//end LoadParentInfoDatabase

}//end namespace
