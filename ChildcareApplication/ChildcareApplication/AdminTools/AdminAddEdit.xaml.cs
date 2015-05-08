using ChildcareApplication.DatabaseController;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChildcareApplication.AdminTools {
    /// <summary>
    /// Interaction logic for AdminAddEdit.xaml
    /// </summary>
    public partial class AdminAddEdit : Window {
        private AdminDB db;
        private string loggedInAs;
        private bool editingPW;
        private bool pwChanged;

        public AdminAddEdit(string username) {
            InitializeComponent();
            this.db = new AdminDB();
            this.loggedInAs = username;

            lst_AdminList.ItemsSource = db.FindAdmins();
            this.MouseDown += WindowMouseDown;
        }

        private void lst_AdminList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            if (lst_AdminList.SelectedItem != null) {
                string tempUN = lst_AdminList.SelectedItem.ToString();
                fillForms(tempUN);
                btn_DelAdmin.IsEnabled = true;
                txt_LoginName.IsEnabled = true;
                txt_Email.IsEnabled = true;
                rdb_Full.IsEnabled = true;
                rdb_Limited.IsEnabled = true;
                pwChanged = false;
            } else {
                clearForm();
            }
        }

        private void fillForms(string adminUN) {
            string tempPW = "1234567890";
            string accessLevel = db.GetAccessLevel(adminUN);
            string tempEM = db.GetAdminEmail(adminUN);

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

        private void btn_AddAdmin_Click(object sender, RoutedEventArgs e) {
            if (lst_AdminList.Items.Contains("default")) {
                MessageBox.Show("You cannot add a new administrator while there is still a default administrator.\nPlease edit the default administrator before adding a new one");
            } else {
                this.db.AddNewAdmin();
                lst_AdminList.ItemsSource = db.FindAdmins();
                lst_AdminList.UnselectAll();
            }
        }

        private void btn_DelAdmin_Click(object sender, RoutedEventArgs e) {
            if (lst_AdminList.SelectedItem.ToString().Equals(loggedInAs)) {
                MessageBox.Show("You cannot delete the currently logged in administrator");
            }
            else {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you wish to delete this administrator?", "Deletion Conformation", MessageBoxButton.OKCancel);
                if (messageBoxResult == MessageBoxResult.OK) {
                    MessageBox.Show(lst_AdminList.SelectedItem.ToString() + " has been deleted from the administrator list");
                    db.DeleteAdmin(lst_AdminList.SelectedItem.ToString());
                    lst_AdminList.ItemsSource = db.FindAdmins();
                    clearForm();
                } else {
                    MessageBox.Show("Deletion canceled");
                }

            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            //this check will ensure that if the user is logged in as the currently selected administrator and changes their name it won't break functionality
            if (lst_AdminList.SelectedItem.ToString().Equals(loggedInAs)) {
                loggedInAs = txt_LoginName.Text;
            }

            if (pwChanged) {
                if (rdb_Full.IsChecked == true)
                    db.UpdateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Password.Password, txt_Email.Text, "1");
                else if (rdb_Limited.IsChecked == true)
                    db.UpdateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Password.Password, txt_Email.Text, "2");
            } else {
                if (rdb_Full.IsChecked == true)
                    db.UpdateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Email.Text, "1");
                else if (rdb_Limited.IsChecked == true)
                    db.UpdateAdmin(lst_AdminList.SelectedItem.ToString(), txt_LoginName.Text, txt_Email.Text, "2");
            }


            lst_AdminList.ItemsSource = db.FindAdmins();
            clearForm();  
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            clearForm();
        }

        private void clearForm() {
            lst_AdminList.SelectedItem = null;
            txt_LoginName.Clear();
            txt_Password.Clear();
            txt_ConfirmPass.Clear();
            txt_Email.Clear();
            rdb_Full.IsChecked = false;
            rdb_Limited.IsChecked = false;
            btn_AddAdmin.IsEnabled = true;
            btn_DelAdmin.IsEnabled = false;
            txt_LoginName.IsEnabled = false;
            txt_Email.IsEnabled = false;
            rdb_Full.IsEnabled = false;
            rdb_Limited.IsEnabled = false;
            btn_Save.IsEnabled = false;
            btn_Cancel.IsEnabled = false;
        }

        private void btn_ChangePW_Click(object sender, RoutedEventArgs e) {
            if (!editingPW) {
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
            else if (editingPW) {
                if (passwordsMatch()) {
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
                    pwChanged = true;
                }
                else {
                    MessageBox.Show("Passwords do not match. Please try again");
                }
            }

        }

        private bool passwordsMatch() {
            return txt_Password.Password.Equals(txt_ConfirmPass.Password);
        }

        private void txt_Password_LostFocus(object sender, RoutedEventArgs e) {
            txt_Password.Password = AdminTools.Hashing.HashPass(txt_Password.Password);
        }

        private void txt_ConfirmPass_LostFocus(object sender, RoutedEventArgs e) {
            txt_ConfirmPass.Password = AdminTools.Hashing.HashPass(txt_ConfirmPass.Password);
        }

        private void txt_Password_GotFocus(object sender, RoutedEventArgs e) {
            txt_Password.Clear();
        }

        private void txt_ConfirmPass_GotFocus(object sender, RoutedEventArgs e) {
            txt_ConfirmPass.Clear();
        }

        private void SelectAllonFocus(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)sender;
            Dispatcher.BeginInvoke((Action)(tb.SelectAll));
        }

        private void txt_LoginName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void txt_Email_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void rdb_Full_Unchecked(object sender, RoutedEventArgs e) {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void rdb_Limited_Unchecked(object sender, RoutedEventArgs e) {
            btn_Save.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void txt_LoginName_LostFocus(object sender, RoutedEventArgs e) {
            if (lst_AdminList.Items.Contains(txt_LoginName.Text) && !(lst_AdminList.SelectedItem.ToString().Equals(txt_LoginName.Text))) {
                MessageBox.Show("An administrator with that name already exists. Please change the login name to be unique before continuing");
                btn_Save.IsEnabled = false;
            }
        }
    }
}