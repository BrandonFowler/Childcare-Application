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
            DatabaseController.CleanDB db = new DatabaseController.CleanDB();
            bool success = db.clean();
            if (!success) {
                UserSelection US = new UserSelection();
                US.Show();
            }
        }
    }
}
