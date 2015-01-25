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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ChildCareApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            
        }

        private void btn_EditParentsKids_Click(object sender, RoutedEventArgs e) {
            win_AddOrEditParent AddOrEditParent = new win_AddOrEditParent();
            AddOrEditParent.Show();
            //this.Close(); 
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            if (txt_UserName.Text.Equals("1") && txt_Password.Text.Equals("1")) {
                lbl_ManagerPas.Visibility = System.Windows.Visibility.Hidden;
                lbl_UserName.Visibility = System.Windows.Visibility.Hidden;
                txt_Password.IsEnabled = false;
                txt_Password.Visibility = System.Windows.Visibility.Hidden;
                txt_UserName.IsEnabled = false;
                txt_UserName.Visibility = System.Windows.Visibility.Hidden;
                btn_Login.IsEnabled = false;
                btn_Login.Visibility = System.Windows.Visibility.Hidden;
                btn_EditParentsKids.IsEnabled = true;
                btn_EditParentsKids.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void btn_EditPricing_Click(object sender, RoutedEventArgs e) {

        }

        private void btn_EditSpecialEvents_Click(object sender, RoutedEventArgs e) {

        }

        private void btn_BusinessReport_Click(object sender, RoutedEventArgs e) {

        }

        private void btn_ParentReport_Click(object sender, RoutedEventArgs e) {

        }

    }
}
