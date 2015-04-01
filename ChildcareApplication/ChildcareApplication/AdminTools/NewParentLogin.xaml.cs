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
using System.Data;
using DatabaseController;

namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_NewParentLogin.xaml
    /// </summary>
    public partial class NewParentLogin : Window {
        private LoadParentInfoDatabase db;
        public NewParentLogin() {
            InitializeComponent();
            this.db = new LoadParentInfoDatabase();
        }

        private void btn_AddNewParent_Click(object sender, RoutedEventArgs e) {
            string pID = "", PIN = "";
            bool formNotComplete = CheckIfNull();
            if (!formNotComplete)//form is completed
            {
                bool sameID = checkIfSame(txt_ParentID1.Text, txt_ParentID2.Text);
                bool samePIN = checkIfSame(psw_ParentPIN1.Password, psw_ParentPIN2.Password);
                if (sameID && samePIN)//both IDand PIN are the same vlues
                {
                    bool numbersID = checkIfNumbers(txt_ParentID1.Text, txt_ParentID2.Text);
                    bool numbersPIN = checkIfNumbers(psw_ParentPIN1.Password, psw_ParentPIN2.Password);
                    if (numbersID && numbersPIN)//both ID and PIN are numbers
                    {
                        pID = txt_ParentID1.Text;
                        PIN = psw_ParentPIN1.Password;
                        DataSet DS = new DataSet();
                        DS = this.db.GetParentInfo(pID);
                        int count = DS.Tables[0].Rows.Count;

                        if (count == 0)//ID does not exist
                        {

                            this.db.AddNewParent(pID, PIN, "\"First\"", "\"Last\"", "\"000-000-0000\"", "\"someEmail@email.com\"", "\"123 Road St\"", "\"none\"", "\"City\"", "\"WA\"", "\"12345\"", "\"../../Pictures/default.jpg\"");
                            MakeFamilyID(pID);
                            //MessageBox.Show("Made New Parent");

                            AdminEditParentInfo adminEditParentInfo = new AdminEditParentInfo(pID);
                            adminEditParentInfo.Show();
                            this.Close();

                        } else {
                            MessageBox.Show("A Guardian with this ID already Exists. Please re-enter your ID");
                        }
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
            DataSet DS = new DataSet();
            DS = this.db.checkIfFamilyExists(familyID);

            int count = DS.Tables[0].Rows.Count;

            if (count == 0)//FamilyID does not exist
             {
                this.db.AddNewFamily(familyID, 0.0);
            }

        }
        private bool CheckIfNull() {


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

        private bool checkIfSame(string str1, string str2) {

            if (str1.Equals(str2))
                return true;
            else {
                MessageBox.Show("Your ID or PIN numbers do not match. Please re-enter");

                return false;
            }

        }

        private bool checkIfNumbers(string str1, string str2) {
            int parseNum1, parseNum2;

            bool isNum1 = int.TryParse(str1, out parseNum1);
            bool isNum2 = int.TryParse(str1, out parseNum2);

            if (isNum1 && isNum2)
                return true;
            else {
                MessageBox.Show("Your ID or PIN numbers are not numbers only. Please re-enter.");

                return false;
            }
        }

    }
}
