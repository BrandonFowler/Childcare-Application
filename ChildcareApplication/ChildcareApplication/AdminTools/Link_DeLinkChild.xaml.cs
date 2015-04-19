using ChildcareApplication.AdminTools;
using DatabaseController;
using System.Windows;
namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_Link_DeLinkChild.xaml
    /// </summary>

    public partial class Link_DeLinkChild : Window {
        //private LoadParentInfoDatabase db;
        private ChildInfoDatabase db;
        int linked;
        string childID;

        //private ChildInfoDatabse dbChild;
        public Link_DeLinkChild(int link, string cID) {
            linked = link;
            childID = cID;
            InitializeComponent();
            this.db = new ChildInfoDatabase();
        }

        private void btn_Enter_Click(object sender, RoutedEventArgs e) {
            ConnectionsDB conDB = new ConnectionsDB();
            string fID = "", pID = "";
            bool formNotComplete = CheckIfNull();
            if (!formNotComplete)//form is completed
             {
                bool sameID = checkIfSame(txt_GuardianID.Text, txt_GuardianID2.Text);
                bool regexID = RegExpressions.RegexID(txt_GuardianID.Text);
                if (sameID && regexID)//both IDand PIN are the same vlues
                 {
                    pID = txt_GuardianID.Text;

                    MakeFamilyID(pID);
                    if (linked == 0) {//link child
 
                        int connID = this.db.GetMaxConnectionID();
                        connID = connID + 1;

                        string connectionID = connID.ToString();
                        fID = MakeFamilyID(pID);
                        conDB.UpdateAllowedConnections(connectionID, pID, childID, fID);
                    } else if (linked == 1) {//delink child

                        conDB.DeleteAllowedConnection(childID, pID);
                    }
                    this.Close();
                }
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private string MakeFamilyID(string ID) {
            string familyID = "";

            for (int x = 0; x < ID.Length - 1; x++) {
                familyID += ID[x];
            }
            return familyID;
        }
        private bool CheckIfNull() {


            if (string.IsNullOrWhiteSpace(this.txt_GuardianID.Text)) {
                MessageBox.Show("Please enter the ID number.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_GuardianID2.Text)) {
                MessageBox.Show("Please enter the ID number a second time.");
                return true;
            }

            return false;
        }//end CheckIfNull

        private bool checkIfSame(string str1, string str2) {

            if (str1.Equals(str2))
                return true;
            else {
                MessageBox.Show("Your ID numbers do not match. Please re-enter");

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
                MessageBox.Show("Your ID numbers are not numbers only. Please re-enter.");

                return false;
            }
        }
    }
}
