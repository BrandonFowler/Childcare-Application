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
using ChildcareApplication.AdminTools;

namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_NewParentLogin.xaml
    /// </summary>
    public partial class NewParentLogin : Window {
        private GuardianInfoDB db;
        public NewParentLogin() {
            InitializeComponent();
            this.db = new GuardianInfoDB();
        }

        private void btn_AddNewParent_Click(object sender, RoutedEventArgs e) {
            string pID = "", PIN = "";
            bool formNotComplete = CheckIfNull();
            if (!formNotComplete)//form is completed
            {
                bool sameID = checkIfSame(txt_ParentID1.Text, txt_ParentID2.Text);
                bool samePIN = checkIfSame(psw_ParentPIN1.Password, psw_ParentPIN2.Password);

                bool regexID = RegExpressions.regexID(txt_ParentID1.Text);
                bool regexPIN = RegExpressions.regexPIN(psw_ParentPIN1.Password);

                if(sameID && samePIN && regexID && regexPIN)
                {
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

                    }
                    else
                    {
                        MessageBox.Show("A Guardian with this ID already Exists. Please re-enter your ID");
                    }

                }
               /* bool correctLength = checkLength(txt_ParentID1.Text, txt_ParentID2.Text, 6, 0);
                bool corretLengthPIN = checkLength(psw_ParentPIN1.Password, psw_ParentPIN2.Password, 4, 1);
                if (sameID && samePIN && correctLength && corretLengthPIN)//both IDand PIN are the same vlues
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
                            string hashedPIN = ChildcareApplication.AdminTools.Hashing.HashPass(PIN);
                            hashedPIN = "\"" + hashedPIN + "\""; 
                            this.db.AddNewParent(pID, hashedPIN, "\"First\"", "\"Last\"", "\"000-000-0000\"", "\"someEmail@email.com\"", "\"123 Road St\"", "\"none\"", "\"City\"", "\"WA\"", "\"12345\"", "\"../../Pictures/default.jpg\"");
                            MakeFamilyID(pID);
                            //MessageBox.Show("Made New Parent");

                            AdminEditParentInfo adminEditParentInfo = new AdminEditParentInfo(pID);
                            adminEditParentInfo.Show();
                            this.Close();

                        } else {
                            MessageBox.Show("A Guardian with this ID already Exists. Please re-enter your ID");
                        }
                    }
                }*/


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

        private void txt_ParentID1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txt_ParentID1 != null)
            {
                txt_ParentID1.SelectAll();
            }
        }

        private void txt_ParentID1_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txt_ParentID1_GotFocus(sender, e); 
        }

        private void txt_ParentID2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txt_ParentID2 != null)
            {
                txt_ParentID2.SelectAll();
            }
        }

        private void txt_ParentID2_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txt_ParentID2_GotFocus(sender, e); 
        }

    }
}
