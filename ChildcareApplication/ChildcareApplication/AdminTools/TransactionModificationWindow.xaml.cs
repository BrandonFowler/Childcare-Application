using DatabaseController;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            this.transactionID = "";
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e) {
            TransactionDB transDB = new TransactionDB();

            if (VerifyFormData()) {
                if (this.transactionID != "") {
                    UpdateTransaction();
                } else {
                    NewTransaction();
                }
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
            int hour, minute;

            if (splitTime.Length != 3) {
                return false;
            }

            if (!Int32.TryParse(splitTime[0], out hour)) {
                return false;
            }
            if (!Int32.TryParse(splitTime[1], out minute)) {
                return false;
            }
            if (!(splitTime[2] == "AM" || splitTime[2] == "PM")) {
                return false;
            }
            if (hour < 1 || hour > 12 || minute < 0 || minute > 59) {
                return false;
            }
            return true;
        }

        private bool ValidDate(string date) {
            DateTime dt;

            if(!DateTime.TryParse(date, out dt)) {
                return false;
            }
            return true;
        }

        private bool ValidTransactionTotal(string transactionTotal) {
            int transTotal;

            if (!Int32.TryParse(transactionTotal, out transTotal)) {
                return false;
            }
            
            return true;
        }

        private void UpdateTransaction() {
            TransactionDB transDB = new TransactionDB();


        }

        private void NewTransaction() {
            TransactionDB transDB = new TransactionDB();
            //string transactionID = transDB.GetNextTransID();
            

        }
    }
}
