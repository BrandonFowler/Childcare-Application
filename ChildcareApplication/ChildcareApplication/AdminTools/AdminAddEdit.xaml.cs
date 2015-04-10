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

        public AdminAddEdit()
        {
            InitializeComponent();
            this.db = new AdminDB();

            lst_AdminList.ItemsSource = db.findAdmins();
        }

        private void lst_AdminList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(lst_AdminList.SelectedItem != null)
            {
                string tempUN = lst_AdminList.SelectedItem.ToString();
                fillForms(tempUN);
                btn_DelAdmin.IsEnabled = true;
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
            MessageBox.Show("NYI");
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if(rdb_Full.IsChecked == true)
                db.updateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Email.Text, "1");
            else if(rdb_Limited.IsChecked == true)
                db.updateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Email.Text, "2");

            if(!(txt_LoginName.Text.Equals("default")))
                btn_AddAdmin.IsEnabled = true;
            lst_AdminList.ItemsSource = db.findAdmins();
            fillForms(txt_LoginName.Text);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
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
        }

        private void btn_ChangePW_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NYI");
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void txt_LoginName_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_LoginName.SelectAll();
        }

        private void txt_Password_GotFocus(object sender, RoutedEventArgs e)
        {
            btn_ChangePW.Focus();
        }

        private void txt_ConfirmPass_GotFocus(object sender, RoutedEventArgs e)
        {
            btn_ChangePW.Focus();
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
