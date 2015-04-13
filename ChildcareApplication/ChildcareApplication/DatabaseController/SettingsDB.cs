
/*
 * 
 * 
 * 
 * 
 * 
 * NOT NEEDED DELETE ME?????????????????
 * 
 * NOT NEEDED DELETE ME?????????????????
 * 
 * NOT NEEDED DELETE ME?????????????????
 * 
 * 
 * NOT NEEDED DELETE ME?????????????????
 * 
 * NOT NEEDED DELETE ME?????????????????
 * 
 * 
 * 
 * NOT NEEDED DELETE ME?????????????????
 * NOT NEEDED DELETE ME?????????????????
 * 
 * NOT NEEDED DELETE ME?????????????????
 * NOT NEEDED DELETE ME?????????????????
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */















using System.Collections;
using System.Data;
using System.Data.SQLite;

namespace ChildcareApplication.DatabaseController
{
    class SettingsDB
    {
        private SQLiteConnection conn;

        public SettingsDB() 
        {
            this.conn = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
        }

        internal string[] getCurrentSettings() 
        {
            conn.Open();
            string[] settingsList = new string[19];

            string settingSQL = "SELECT * FROM ApplicationSettings";

            SQLiteCommand settingComm = new SQLiteCommand(settingSQL, conn);
            SQLiteDataAdapter settingAdapater = new SQLiteDataAdapter(settingComm);
            DataSet settings = new DataSet();
            settingAdapater.Fill(settings);

            settingsList[0] = settings.Tables[0].Rows[0][1].ToString(); //billing start date
            settingsList[1] = settings.Tables[0].Rows[0][1].ToString(); //max monthly fee
            settingsList[2] = settings.Tables[0].Rows[0][1].ToString(); //days to hold records
            settingsList[3] = settings.Tables[0].Rows[0][1].ToString(); //max infant age
            settingsList[4] = settings.Tables[0].Rows[0][1].ToString(); //max regular age

            string operatingSQL = "SELECT * FROM OperatingHours";
            SQLiteCommand operatingComm = new SQLiteCommand(operatingSQL, conn);
            SQLiteDataAdapter operatingAdapter = new SQLiteDataAdapter(operatingComm);
            DataSet operating = new DataSet();
            operatingAdapter.Fill(operating);



            conn.Close();

            return settingsList;
        }

        internal void saveNewSettings(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17, string p18, string p19)
        {
        }
    }
}