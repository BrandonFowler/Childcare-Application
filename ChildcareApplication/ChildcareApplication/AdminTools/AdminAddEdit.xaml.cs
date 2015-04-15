using ChildcareApplication.DatabaseController;
using System.Windows;

namespace ChildcareApplication.AdminTools
{
    /// <summary>
    /// Interaction logic for AdminAddEdit.xaml
    /// </summary>
    public partial class AdminAddEdit : Window
    {
        private AdminDB db;
        private string loggedInAs;
        private bool editingPW;

        public AdminAddEdit(string username)
        {
            InitializeComponent();
            this.db = new AdminDB();
            this.loggedInAs = username;

            lst_AdminList.ItemsSource = db.findAdmins();
        }

        private void lst_AdminList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(lst_AdminList.SelectedItem != null)
            {
                string tempUN = lst_AdminList.SelectedItem.ToString();
                fillForms(tempUN);
                btn_DelAdmin.IsEnabled = true;
                txt_LoginName.IsEnabled = true;
                txt_Email.IsEnabled = true;
                rdb_Full.IsEnabled = true;
                rdb_Limited.IsEnabled = true;                
            }
        }

        private void fillForms(string adminUN)
        {
            string tempPW = "1234567890";
            string accessLevel = db.getAccessLevel(adminUN);
            string tempEM = db.getEmail(adminUN);

            txt_LoginName.Text = adminUN;
            txt_Password.Password = tempPW;
            txt_ConfirmPass.Password = tempPW;
            btn_ChangePW.IsEnabled = true;
            editingPW = false;
            txt_Email.Text = tempEM;

            if (accessLevel.Equals("1"))
                rdb_Full.IsChecked = true;
            else if (accessLevel.Equals("2"))
                rdb_Limited.IsChecked = true;

            btn_Save.IsEnabled = false;
            btn_Cancel.IsEnabled = false;
        }

        private void btn_AddAdmin_Click(object sender, RoutedEventArgs e)
        {
            //disable add new admin button until the user either submits or cancels the addition
            btn_AddAdmin.IsEnabled = false;
            this.db.addNewAdmin();
            lst_AdminList.ItemsSource = db.findAdmins();
            lst_AdminList.UnselectAll();

        }

        private void btn_DelAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (lst_AdminList.SelectedItem.ToString().Equals(loggedInAs))
            {
                MessageBox.Show("You cannot delete the currently logged in administrator");
            }
            else
            {
                db.deleteAdmin(lst_AdminList.SelectedItem.ToString());
                lst_AdminList.ItemsSource = db.findAdmins();
                clearForm();
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if(rdb_Full.IsChecked == true)
                db.updateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Password.Password, txt_Email.Text, "1");
            else if(rdb_Limited.IsChecked == true)
                db.updateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Password.Password, txt_Email.Text, "2");

            if(!(txt_LoginName.Text.Equals("default")))
                btn_AddAdmin.IsEnabled = true;
            lst_AdminList.ItemsSource = db.findAdmins();
            fillForms(txt_LoginName.Text);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            clearForm();
        }

        private void clearForm()
        {
            if (btn_AddAdmin.IsEnabled == false)
                btn_AddAdmin.IsEnabled = true;

            lst_AdminList.SelectedItem = null;
            txt_LoginName.Clear();
            txt_Password.Clear();
            txt_ConfirmPass.Clear();
            txt_Email.Clear();
            rdb_Full.IsChecked = false;
            rdb_Limited.IsChecked = false;

            btn_DelAdmin.IsEnabled = false;
            txt_LoginName.IsEnabled = false;
            txt_Email.IsEnabled = false;
            rdb_Full.IsEnabled = false;
            rdb_Limited.IsEnabled = false;
            btn_Save.IsEnabled = false;
            btn_Cancel.IsEnabled = false;
        }

        private void btn_ChangePW_Click(object sender, RoutedEventArgs e)
        {
            if(!editingPW)
            {
                lst_AdminList.IsEnabled = false;
                btn_AddAdmin.IsEnabled = false;
                btn_DelAdmin.IsEnabled = false;
                btn_Save.IsEnabled = false;
                btn_Cancel.IsEnabled = false;
                txt_LoginName.IsEnabled = false;
                txt_Email.IsEnabled = false;
                rdb_Full.IsEnabled = false;
                rdb_Limited.IsEnabled = false;

                txt_Password.IsEnabled = true;
                txt_ConfirmPass.IsEnabled = true;
                editingPW = true;
                lbl_PassText.Text = "Confirm Changes";
            }
            else if (editingPW)
            {
                if(passwordsMatch())
                {
                    lst_AdminList.IsEnabled = true;
                    btn_AddAdmin.IsEnabled = true;
                    btn_DelAdmin.IsEnabled = true;
                    btn_Save.IsEnabled = true;
                    btn_Cancel.IsEnabled = true;
                    txt_LoginName.IsEnabled = true;
                    txt_Email.IsEnabled = true;
                    rdb_Full.IsEnabled = true;
                    rdb_Limited.IsEnabled = true;

                    txt_Password.IsEnabled = false;
                    txt_ConfirmPass.IsEnabled = false;
                    editingPW = false;
                    lbl_PassText.Text = "Change Password";
                }
                else
                {
                    MessageBox.Show("Passwords do not match. Please try again");
                }
            }

        }

        private bool passwordsMatch()
        {
            return txt_Password.Password.Equals(txt_ConfirmPass.Password);
        }

        private void txt_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            txt_Password.Password = AdminTools.Hashing.HashPass(txt_Password.Password);
        }

        private void txt_ConfirmPass_LostFocus(object sender, RoutedEventArgs e)
        {
            txt_ConfirmPass.Password = AdminTools.Hashing.HashPass(txt_ConfirmPass.Password);
        }

        private void txt_LoginName_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_LoginName.SelectAll();
        }

        private void txt_Password_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_Password.Clear();
        }

        private void txt_ConfirmPass_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_ConfirmPass.Clear();
        }

        private void txt_Email_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_Email.SelectAll();
        }

        private void txt_LoginName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void txt_Email_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void rdb_Full_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void rdb_Limited_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }
    }
}
