using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace ChildCareAppParentSide {
    class Settings {

        private static Settings instance;
        public string server;
        public string port;
        public string databaseName;
        public string databaseUser;
        public string databasePassword;
        public string photoPath;
        public string regularCareCap;
        public string billStart;
        public string billEnd;

        private Settings() {
            string line;
            if (File.Exists("settings.txt")) {
                var filestream = new FileStream("settings.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var file = new StreamReader(filestream, Encoding.UTF8, true, 128);
                while ((line = file.ReadLine()) != null) {
                    string[] tokens = line.Split(new string[] { "=" }, StringSplitOptions.None);
                    if (tokens != null && tokens.Length == 2) {
                        if (tokens[0].CompareTo("Server") == 0) {
                            this.server = tokens[1];
                        }
                        else if (tokens[0].CompareTo("Port") == 0) {
                            this.port = tokens[1];
                        }
                        else if (tokens[0].CompareTo("Database") == 0) {
                            this.databaseName = tokens[1];
                        }
                        else if (tokens[0].CompareTo("UID") == 0) {
                            this.databaseUser = tokens[1];
                        }
                        else if (tokens[0].CompareTo("UPW") == 0) {
                            this.databasePassword = tokens[1];
                        }
                        else if (tokens[0].CompareTo("PhotoPath") == 0) {
                            this.photoPath = tokens[1];
                            this.photoPath = this.photoPath.Replace(@"\", @"/");
                        }
                        else if (tokens[0].CompareTo("CareCap") == 0) {
                            this.regularCareCap = tokens[1];
                        }
                        else if (tokens[0].CompareTo("BillStart") == 0) {
                            this.billStart = tokens[1];
                        }
                        else if (tokens[0].CompareTo("BillEnd") == 0) {
                            this.billEnd = tokens[1];
                        }
                    }
                }
            }
            else {
                MessageBox.Show("Cannot find settings. Please configure settings in the admin menu.");
            }
        }// end Settings

        public static Settings Instance{
            get 
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }// end Settings

        public static void reset() {
            instance = null;
        }//end reset

    }//end Settings(class)
}
