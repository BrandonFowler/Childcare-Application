using DatabaseController;
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

namespace AdminTools
{
    /// <summary>
    /// Interaction logic for win_ParentLogin.xaml
    /// </summary>
    /// 

    public partial class ParentIDEntry : Window {
 
        private LoginDB db;
        private bool editParent; 
        public ParentIDEntry(bool editP) {
            InitializeComponent();
            editParent = editP; 
            this.db = new LoginDB();
            this.txt_IDEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.Focus();
        }

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e) {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back)
            {

            }
            else
            {
                MessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }
        }//end KeyPressedValidateNumber


        private void btn_Submit_Click(object sender, RoutedEventArgs e) {

            if (string.IsNullOrWhiteSpace(this.txt_IDEntry.Text))
            {
                MessageBox.Show("Please enter the parents ID number.");

            }
            else
            {
                string ID = txt_IDEntry.Text;
                bool userFound = this.db.validateLogin(ID);

                if (userFound)
                {
                    
                    if (editParent)
                        DisplayAdminEditParentWindow(ID);

                    else
                        DisplayAdminEditChildInfo(ID);
                }//end if
                else
                {
                    MessageBox.Show("User ID or PIN does not exist");
                }//end else

            }//end else

        }//end submit click

        private void DisplayAdminEditParentWindow(string ID) {

            AdminEditParentInfo AdminEditParentWindow = new AdminEditParentInfo(ID);
            AdminEditParentWindow.Show();
            this.Close();
        }//end DisplayAdminEditParentWindow

        private void DisplayAdminEditChildInfo(string ID) {

            win_AdminEditChildInfo AdminEditChildInfo = new win_AdminEditChildInfo(ID);
            AdminEditChildInfo.Show();
            this.Close();
        }//end DisplayAdminEditChildInfo
    }
}
