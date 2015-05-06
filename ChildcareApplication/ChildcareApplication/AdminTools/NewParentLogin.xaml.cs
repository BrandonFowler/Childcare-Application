using ChildcareApplication.AdminTools;
using DatabaseController;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                        MessageBox.Show("A Guardian with this ID already Exists. Please re-enter your ID");
                    }

                }

            }

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Close();
        }

        private void MakeFamilyID(string ID) {
            string familyID = "";

            for (int x = 0; x < ID.Length - 1; x++) {
                familyID += ID[x];
            }
            //DataSet DS = new DataSet();
            //DS = this.db.checkIfFamilyExists(familyID);
            string fID = this.db.CheckIfFamilyExists(familyID);

            if (string.IsNullOrWhiteSpace(fID))//FamilyID does not exist
             {
                this.db.AddNewFamily(familyID, 0.0);
            }

        }
        public bool CheckIfNull() {

            if (string.IsNullOrWhiteSpace(this.txt_ParentID1.Text)) {
                MessageBox.Show("Please enter your ID number.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_ParentID2.Text)) {
                MessageBox.Show("Please enter your ID number a second time.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.psw_ParentPIN1.Password)) {
                MessageBox.Show("Please enter your PIN number.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.psw_ParentPIN2.Password)) {
                MessageBox.Show("Please enter your PIN number a second time.");
                return true;
            }
            return false;
        }//end CheckIfNull

        public bool CheckIfSame(string str1, string str2) {

            if (str1.Equals(str2))
                return true;
            else {
                MessageBox.Show("Your ID or PIN numbers do not match. Please re-enter");

                return false;
            }

        }

        private void txt_GotFocus(object sender, RoutedEventArgs e) {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null) {
                textBox.SelectAll();
            }
        }

        private void txt_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void psw_GotFocus(object sender, RoutedEventArgs e) {
            var passwordBox = e.OriginalSource as PasswordBox;
            if (passwordBox != null) {
                passwordBox.SelectAll();
            }
        }

        private void psw_GotMouseCapture(object sender, MouseEventArgs e) {
            psw_GotFocus(sender, e);
        }

        private void txt_ParentID1_GotFocus(object sender, RoutedEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void txt_ParentID2_GotFocus(object sender, RoutedEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void txt_ParentID1_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotMouseCapture(sender, e);
        }

        private void txt_ParentID2_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotMouseCapture(sender, e);
        }

        private void psw_ParentPIN1_GotFocus(object sender, RoutedEventArgs e) {
            psw_GotFocus(sender, e);
        }

        private void psw_ParentPIN1_GotMouseCapture(object sender, MouseEventArgs e) {
            psw_GotMouseCapture(sender, e);
        }

        private void psw_ParentPIN2_GotFocus(object sender, RoutedEventArgs e) {
            psw_GotFocus(sender, e);
        }

        private void psw_ParentPIN2_GotMouseCapture(object sender, MouseEventArgs e) {
            psw_GotMouseCapture(sender, e);
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

    }
}
