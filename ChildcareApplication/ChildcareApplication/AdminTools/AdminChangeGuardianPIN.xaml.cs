using DatabaseController;
using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ChildcareApplication.AdminTools {
    /// <summary>
    /// Interaction logic for AdminChangeGuardianPIN.xaml
    /// </summary>
    public partial class AdminChangeGuardianPIN : Window {
        private GuardianInfoDB db;
        private bool formError;
        public AdminChangeGuardianPIN(string pID) {
             
            InitializeComponent();
            this.db = new GuardianInfoDB();
            this.MouseDown += WindowMouseDown;
            txt_ParentID1.Text = pID; 
            psw_ParentPIN1.Focus();
        }

        private void btn_ChnagePIN_Click(object sender, RoutedEventArgs e) {
            string pID = "", PIN = "";
            bool formNotComplete = CheckIfNull();
            if (!formNotComplete)//form is completed
            {
               
                bool samePIN = CheckIfSame(psw_ParentPIN1.Password, psw_ParentPIN2.Password);

                bool regexPIN = RegExpressions.RegexPIN(psw_ParentPIN1.Password);

                if (samePIN && regexPIN) {
                    pID = string.Format("{0:000000}", txt_ParentID1.Text);
                    PIN = string.Format("{0:0000}", psw_ParentPIN1.Password);

                        string hashedPIN = ChildcareApplication.AdminTools.Hashing.HashPass(PIN);
                        this.db.UpdateParentPIN(pID, hashedPIN);
                        this.Close();

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
            formError = true; 
           if (string.IsNullOrWhiteSpace(this.psw_ParentPIN1.Password) && formError) {
                WPFMessageBox.Show("Please enter your PIN number.");
                formError = false; 
                return true;
            } else if (string.IsNullOrWhiteSpace(this.psw_ParentPIN2.Password) && formError) {
                WPFMessageBox.Show("Please enter your PIN number a second time.");
                formError = false; 
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
