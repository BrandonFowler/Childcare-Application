using ChildcareApplication.AdminTools;
using DatabaseController;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBoxUtils;
using System;

namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_NewParentLogin.xaml
    /// </summary>
    public partial class NewParentLogin : Window {
        private GuardianInfoDB db;
        public NewParentLogin() {
            InitializeComponent();
            this.db = new GuardianInfoDB();
            this.MouseDown += WindowMouseDown;
            txt_ParentID1.Focus(); 
        }

        private void btn_AddNewParent_Click(object sender, RoutedEventArgs e) {
            string pID = "", PIN = "";
            bool formNotComplete = CheckIfNull();
            if (!formNotComplete)//form is completed
            {
                bool sameID = CheckIfSame(txt_ParentID1.Text, txt_ParentID2.Text);
                bool samePIN = CheckIfSame(psw_ParentPIN1.Password, psw_ParentPIN2.Password);

                bool regexID = RegExpressions.RegexID(txt_ParentID1.Text);
                bool regexPIN = RegExpressions.RegexPIN(psw_ParentPIN1.Password);

                if (sameID && samePIN && regexID && regexPIN) {
                    pID = string.Format("{0:000000}", txt_ParentID1.Text);
                    PIN = string.Format("{0:0000}", psw_ParentPIN1.Password);
                    DataSet DS = new DataSet();
                    DS = this.db.GetParentInfoDS(pID);
                    int count = DS.Tables[0].Rows.Count;

                    if (count == 0)//ID does not exist
                    {
                        string hashedPIN = ChildcareApplication.AdminTools.Hashing.HashPass(PIN);
                        hashedPIN = "\"" + hashedPIN + "\"";
                        this.db.AddNewParent(pID, hashedPIN, "\"First\"", "\"Last\"", "\"000-000-0000\"", "\"someEmail@email.com\"", "\"123 Road St\"", "\"none\"", "\"City\"", "\"WA\"", "\"12345\"", "\"../../Pictures/default.jpg\"");
                        MakeFamilyID(pID);

                        AdminEditParentInfo adminEditParentInfo = new AdminEditParentInfo(pID);
                        adminEditParentInfo.Show();
                        this.Close();

                    } else {
                        WPFMessageBox.Show("A Guardian with this ID already Exists. Please re-enter your ID");
                    }

                }

            }

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MakeFamilyID(string ID) {
            string familyID = "";

            for (int x = 0; x < ID.Length - 1; x++) {
                familyID += ID[x];
            }

            string fID = this.db.CheckIfFamilyExists(familyID);

            if (string.IsNullOrWhiteSpace(fID))//FamilyID does not exist
             {
                this.db.AddNewFamily(familyID, 0.0, 0, 0);
            }

        }
        internal bool CheckIfNull() {

            if (string.IsNullOrWhiteSpace(this.txt_ParentID1.Text)) {
                WPFMessageBox.Show("Please enter your ID number.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_ParentID2.Text)) {
                WPFMessageBox.Show("Please enter your ID number a second time.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.psw_ParentPIN1.Password)) {
                WPFMessageBox.Show("Please enter your PIN number.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.psw_ParentPIN2.Password)) {
                WPFMessageBox.Show("Please enter your PIN number a second time.");
                return true;
            }
            return false;
        }//end CheckIfNull

        internal bool CheckIfSame(string str1, string str2) {

            if (str1.Equals(str2))
                return true;
            else {
                WPFMessageBox.Show("Your ID or PIN numbers do not match. Please re-enter");

                return false;
            }

        }
   
        private void SelectAllGotFocus(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)sender;
            Dispatcher.BeginInvoke((Action)(tb.SelectAll));
 
        }

        private void SelectAllGotFocusPW(object sender, RoutedEventArgs e) {
            PasswordBox pwb = (PasswordBox)sender;
            Dispatcher.BeginInvoke((Action)(pwb.SelectAll));
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Key_Up_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

    }
}
