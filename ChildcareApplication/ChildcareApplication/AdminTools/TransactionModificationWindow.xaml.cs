using DatabaseController;
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

namespace AdminTools {
    public partial class TransactionModificationWindow : Window {
        private String transactionID;
        public TransactionModificationWindow(String transactionID) {
            InitializeComponent();
            this.transactionID = transactionID;
            LoadData();
        }

        public TransactionModificationWindow() {
            InitializeComponent();
            LoadEventCMB();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e) {
            if (VerifyFormData()) {

            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void LoadData() {
            ChildInfoDatabase childDB = new ChildInfoDatabase();
            TransactionDB transDB = new TransactionDB();
            ParentInfoDB parentDB = new ParentInfoDB();

            this.txt_ChildName.Text = childDB.GetChildName(this.transactionID);
            this.txt_GuardianName.Text = parentDB.GetParentNameFromTrans(this.transactionID);
            this.txt_TransactionTotal.Text = String.Format("{0:0.00}", transDB.GetTransactionTotal(this.transactionID));
            string eventName = transDB.GetEventName(this.transactionID);
            LoadEventCMB(eventName);
        }

        private void LoadEventCMB(params string[] selectedName) {
            EventModificationDB eventDB = new EventModificationDB();
            List<string> eventNames = eventDB.GetAllEventNames();

            for (int i = 0; i < eventNames.Count; i++) {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = eventNames[i];
                cmb_EventName.Items.Add(item);
                if (selectedName.Length == 1 && eventNames[i] == selectedName[0]) {
                    cmb_EventName.SelectedIndex = i;
                }
            }
        }

        private bool VerifyFormData() {
            ParentInfoDB parentDB = new ParentInfoDB();
            ChildInfoDatabase childDB = new ChildInfoDatabase();
            EventModificationDB eventDB = new EventModificationDB();

            if (!parentDB.GuardianNameExists(txt_GuardianName.Text)) {
                MessageBox.Show("The guardian name you entered does not exist in the database!  Please verify you have spelled it correctly.");
                txt_GuardianName.Focus();
                return false;
            }
            if(!childDB.ChildNameExists(txt_ChildName.Text)) {
                MessageBox.Show("The child name you entered does not exist in the database!  Please verify you have spelled it correctly.");
                txt_ChildName.Focus();
                return false;
            }
            if(cmb_EventName.SelectedIndex < 0) {
                MessageBox.Show("You must select an event from the drop down menu!");
                cmb_EventName.Focus();
                return false;
            }
            if (!ValidTime(txt_CheckIn.Text)) {
                MessageBox.Show("You must enter a valid time in the checked in text box!");
                txt_CheckIn.Focus();
                return false;
            }
            if (!ValidTime(txt_CheckOut.Text)) {
                MessageBox.Show("You must enter a valid time in the checked out text box!");
                txt_CheckOut.Focus();
                return false;
            }
            if (!ValidDate(txt_Date.Text)) {
                MessageBox.Show("You must enter a valid date in the transaction date text box!");
                txt_Date.Focus();
                return false;
            }
            if (!ValidTransactionTotal(txt_TransactionTotal.Text)) {
                MessageBox.Show("You must enter a valid transaction total in the transaction total text box!");
                txt_TransactionTotal.Focus();
                return false;
            }
            return true;
        }

        private bool ValidTime(string time) {
            String[] splitTime = time.Split(new char[] {' ', ':'});
            

            return true;
        }

        private bool ValidDate(string date) {
            return false;
        }

        private bool ValidTransactionTotal(string transactionTotal) {
            return false;
        }
    }
}
