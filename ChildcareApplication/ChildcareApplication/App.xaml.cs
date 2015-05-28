using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DatabaseController;


namespace ChildcareApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            DatabaseController.MaintenanceDB db = new DatabaseController.MaintenanceDB();
            db.SetDefaults();
            bool success = db.Clean();
            success = db.Backup();
            if (!success) {
                UserSelection US = new UserSelection();
                US.Show();
            }
        }
    }
}
