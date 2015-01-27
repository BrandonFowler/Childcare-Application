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

namespace ChildCareApp
{
    /// <summary>
    /// Interaction logic for win_ParentLogin.xaml
    /// </summary>
    /// 
    /* This class is almost identical to the mainWindow.cs... 
     * There has to be a better way of doing this and not just redoing all the same code
     * This is for testing,, we sould redo if we have time
     */
    public partial class win_ParentLogin : Window {

        private bool IDBoxSelected = false;
        private bool PINBoxSelected = false;
        private Database db;
        private bool editParent; 
        public win_ParentLogin(bool editP) {
            InitializeComponent();
            editParent = editP; 
            this.db = new Database();
            this.txt_IDEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.GotFocus += OnIDBoxFocus;
            this.txt_PINEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_PINEntry.GotFocus += OnPINBoxFocus;
            this.txt_IDEntry.Focus();
        }

        private void OnIDBoxFocus(object sender, EventArgs e)
        {
            this.IDBoxSelected = true;
            this.PINBoxSelected = false;
        }//end OnTDBoxFocus

        private void OnPINBoxFocus(object sender, EventArgs e)
        {
            this.PINBoxSelected = true;
            this.IDBoxSelected = false;
        }//end OnPINBoxFocus

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e)
        {
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

            string ID = txt_IDEntry.Text;
            string PIN = txt_PINEntry.Text;
            bool userFound = this.db.validateLogin(ID, PIN);

            if (userFound)
            {
                MessageBox.Show("User found");
                if (editParent)
                    DisplayAdminEditParentWindow(ID);

                else
                    DisplayAdminEditChildInfo(ID); 
            }
            else
            {
                MessageBox.Show("User ID or PIN does not exist");
            }
            
             

        }//end submit click

        private void DisplayAdminEditParentWindow(string ID) {

            win_AdminEditParentInfo AdminEditParentWindow = new win_AdminEditParentInfo(ID);
            AdminEditParentWindow.Show();
            this.Close();
        }//end DisplayAdminEditParentWindow

        private void DisplayAdminEditChildInfo(string ID)
        {

            win_AdminEditChildInfo AdminEditChildInfo = new win_AdminEditChildInfo(ID);
            AdminEditChildInfo.Show();
            this.Close();
        }//end DisplayAdminEditChildInfo
    }
}
