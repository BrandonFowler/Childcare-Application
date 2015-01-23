using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChildCareApp {
    /// <summary>
    /// Interaction logic for win_AdminEditParentInfo.xaml
    /// </summary>
    public partial class win_AdminEditParentInfo : Window {
        public win_AdminEditParentInfo() {
            InitializeComponent();
            cnv_ParentIcon.Background = new SolidColorBrush(Colors.Aqua); //setting anvas color so we can see it
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {

            //save all information to database
            
            ClearFields(); 

        }

        private void btn_Finish_Click(object sender, RoutedEventArgs e) {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); 
        }
        private void ClearFields() {
            txt_Address.Clear();
            txt_City.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_MiddleInitial.Clear();
            txt_IDNumber.Clear();
            txt_PhoneNumber.Clear();
            txt_Zip.Clear();
        }
    }
}
