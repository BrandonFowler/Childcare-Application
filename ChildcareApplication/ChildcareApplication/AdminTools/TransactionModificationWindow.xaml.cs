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
using MessageBoxUtils;

namespace AdminTools {
    public partial class TransactionModificationWindow : Window {
        private String transactionID;
        private bool dataChanged;

        public TransactionModificationWindow(String transactionID) {
            InitializeComponent();
            this.transactionID = transactionID;
            LoadData();
            this.dataChanged = false;
            this.MouseDown += WindowMouseDown;
        }

        public TransactionModificationWindow() {
            InitializeComponent();
            LoadEventCMB();
            this.transactionID = "";
            this.dataChanged = false;
            this.MouseDown += WindowMouseDown;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e) {
            TransactionDB transDB = new TransactionDB();

            if (this.dataChanged) {
                if (VerifyFormData()) {
                    SubmitChanges();
                    this.Close();
                }
                this.dataChanged = false;
            } else {
                WPFMessageBox.Show("You did not modify any of the data in this form.  If you would like to cancel modifying this transaction, please click the cancel button.");
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void LoadData() {
            ChildInfoDatabase childDB = new ChildInfoDatabase();
            TransactionDB transDB = new TransactionDB();
            GuardianInfoDB parentDB = new GuardianInfoDB();

            this.txt_ChildName.Text = childDB.GetChildName(this.transactionID);
            this.txt_GuardianName.Text = transDB.GetParentNameFromTrans(this.transactionID);
            this.txt_TransactionTotal.Text = String.Format("{0:0.00}", transDB.GetTransactionTotal(this.transactionID));
            string eventName = transDB.GetTransEventName(this.transactionID);
            LoadEventCMB(eventName);
            this.txt_CheckIn.Text = transDB.GetTwelveHourTime(this.transactionID, "CheckedIn");
            this.txt_CheckOut.Text = transDB.GetTwelveHourTime(this.transactionID, "CheckedOut");
            this.txt_Date.Text = transDB.GetTransactionDate(this.transactionID);
        }

        private void LoadEventCMB(params string[] selectedName) {
            EventDB eventDB = new EventDB();
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
            GuardianInfoDB parentDB = new GuardianInfoDB();
            ChildInfoDatabase childDB = new ChildInfoDatabase();
            EventDB eventDB = new EventDB();

            if (!parentDB.GuardianNameExists(txt_GuardianName.Text)) {
                WPFMessageBox.Show("The guardian name you entered does not exist in the database!  Please verify you have spelled it correctly.");
                txt_GuardianName.Focus();
                return false;
            }
            if(!childDB.ChildNameExists(txt_ChildName.Text)) {
                WPFMessageBox.Show("The child name you entered does not exist in the database!  Please verify you have spelled it correctly.");
                txt_ChildName.Focus();
                return false;
            }
            if(cmb_EventName.SelectedIndex < 0) {
                WPFMessageBox.Show("You must select an event from the drop down menu!");
                cmb_EventName.Focus();
                return false;
            }
            if (!ValidTime(txt_CheckIn.Text)) {
                WPFMessageBox.Show("You must enter a valid time in the checked in text box!");
                txt_CheckIn.Focus();
                return false;
            }
            if (!ValidTime(txt_CheckOut.Text)) {
                WPFMessageBox.Show("You must enter a valid time in the checked out text box!");
                txt_CheckOut.Focus();
                return false;
            }
            if (!ValidDate(txt_Date.Text)) {
                WPFMessageBox.Show("You must enter a valid date in the transaction date text box!");
                txt_Date.Focus();
                return false;
            }
            if (!ValidTransactionTotal(txt_TransactionTotal.Text)) {
                WPFMessageBox.Show("You must enter a valid transaction total in the transaction total text box!");
                txt_TransactionTotal.Focus();
                return false;
            }
            return true;
        }

        private bool ValidTime(string time) {
            String[] splitTime = time.Split(new char[] {' ', ':'});
            int hour, minute, second;

            if (splitTime.Length != 4) {
                return false;
            }
            if (splitTime[1].Length != 2 || splitTime[2].Length != 2 || splitTime[3].Length != 2) {
                return false;
            }
            if (!Int32.TryParse(splitTime[0], out hour)) {
                return false;
            }
            if (!Int32.TryParse(splitTime[1], out minute)) {
                return false;
            }
            if (!Int32.TryParse(splitTime[2], out second)) {
                return false;
            }
            if (!(splitTime[3] == "AM" || splitTime[3] == "PM")) {
                return false;
            }
            if (hour < 1 || hour > 12 || minute < 0 || minute > 59 || second < 0 || second > 59) {
                return false;
            }
            return true;
        }

        private bool ValidDate(string date) {
            DateTime dt;
            string[] splitDate = date.Split('/');

            if(!DateTime.TryParse(date, out dt)) {
                return false;
            }
            if (splitDate[0].Length != 2 || splitDate[1].Length != 2 || splitDate[2].Length != 4) {
                return false;
            }
            if (Convert.ToInt32(splitDate[2]) < 1000) {
                return false;
            }
            return true;
        }

        private bool ValidTransactionTotal(string transactionTotal) {
            double transTotal;

            if (!Double.TryParse(transactionTotal, out transTotal)) {
                return false;
            }
            if (transTotal < 0) {
                return false;
            }
            return true;
        }

        private void SubmitChanges() {
            TransactionDB transDB = new TransactionDB();
            ConnectionsDB conDB = new ConnectionsDB();
            string transID, eventName, allowanceID, transDate, checkedIn, checkedOut, transTotal;

            eventName = ((ComboBoxItem)cmb_EventName.SelectedItem).Content.ToString();
            allowanceID = conDB.GetAllowanceIDOnNames(txt_GuardianName.Text, txt_ChildName.Text);
            transDate = FormatDate(txt_Date.Text);
            checkedIn = FormatTime(txt_CheckIn.Text);
            checkedOut = FormatTime(txt_CheckOut.Text);
            transTotal = txt_TransactionTotal.Text;

            if (this.transactionID == "") {
                transID = transDB.GetNextTransID();
                transDB.NewTransaction(transID, eventName, allowanceID, transDate, checkedIn, checkedOut, transTotal);
            } else {
                transDB.UpdateTransaction(this.transactionID, eventName, allowanceID, transDate, checkedIn, checkedOut, transTotal);
            }
        }

        //expects time as HH:MM:SS PM
        private string FormatTime(string time) {
            string[] splitTime = time.Split(new char[] { ' ', ':' });
            
            if (splitTime[3] == "PM" && Convert.ToInt32(splitTime[0]) != 12) {
                int hours = Convert.ToInt32(splitTime[0]);

                splitTime[0] = (hours + 12).ToString();
            }
            if (splitTime[0].Length == 1) {
                splitTime[0] = "0" + splitTime[0];
            }
            return (splitTime[0] + ":" + splitTime[1] + ":" + splitTime[2]);
        }

        private string FormatDate(string date) {
            string[] splitDate = date.Split('/');

            if (splitDate[0].Length == 1) {
                splitDate[0] = "0" + splitDate[0];
            }
            if (splitDate[1].Length == 1) {
                splitDate[1] = "0" + splitDate[1];
            }
            return splitDate[2] + "-" + splitDate[0] + "-" + splitDate[1];
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)((TextBox)sender).SelectAll);
        }

        private void KeyUp_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

        private void TextChanged_Event(object sender, Object e) {
            this.dataChanged = true;
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
