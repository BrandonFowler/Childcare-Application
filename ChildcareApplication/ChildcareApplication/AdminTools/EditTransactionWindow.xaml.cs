using DatabaseController;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class EditTransactionWindow : Window {
        public EditTransactionWindow() {
            InitializeComponent();
            LoadTransactions();
            this.MouseDown += WindowMouseDown;
            txt_TransactionID.Focus();
        }

        private void LoadTransactions() {
            TransactionDB transDB = new TransactionDB();
            DataTable table = transDB.GetTransactions();
            TransactionDataGrid.ItemsSource = table.DefaultView;
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void txt_TransactionID_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_TransactionID.SelectAll);
        }

        private void btn_EditTransaction_Click(object sender, RoutedEventArgs e) {
            EditTransaction();
        }

        private void btn_DeleteTransaction_Click(object sender, RoutedEventArgs e) {
            if (VerifyTextBox()) {
                TransactionDB db = new TransactionDB();
                
                UpdateTotals();
                db.DeleteTransaction(txt_TransactionID.Text);
                LoadTransactions();
            }
        }

        private void UpdateTotals() {
            TransactionDB db = new TransactionDB();
            string[] data = db.GetUpdateTotalsDetails(txt_TransactionID.Text); //[guardianID, eventName, transactionTotal]

            if (IsRegular(data[1])) {
                db.UpdateRegularBalance(data[0], Double.Parse(data[2]) * -1);
            } else if (data[1].Contains("Camp") || data[1].Contains("camp")) {
                db.UpdateCampBalance(data[0], Double.Parse(data[2]) * -1);
            } else {
                db.UpdateMiscBalance(data[0], Double.Parse(data[2]) * -1);
            }
        }

        private bool IsRegular(string eventName) {
            return (eventName == "Regular Childcare" || eventName == "Infant Childcare" || eventName == "Adolescent Childcare");
        }

        private bool VerifyTextBox() {
            TransactionDB db = new TransactionDB();
            int temp;

            if (txt_TransactionID.Text.Length == 10 && Int32.TryParse(txt_TransactionID.Text, out temp)) {
                if (db.TransactionExists(txt_TransactionID.Text)) {
                    return true;
                } else {
                    WPFMessageBox.Show("The Transaction ID you entered does not exist in the database.  Verify you entered the correct Transaction ID.");
                    txt_TransactionID.Focus();
                    return false;
                }
            } else {
                WPFMessageBox.Show("The Transaction ID you entered does not exist in the database.  Verify you entered the correct Transaction ID.");
                txt_TransactionID.Focus();
                return false;
            }
        }

        private void btn_NewTransaction_Click(object sender, RoutedEventArgs e) {
            TransactionModificationWindow win = new TransactionModificationWindow();
            win.ShowDialog();
            this.LoadTransactions();
        }

        private void txt_TransactionID_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                EditTransaction();
            }
        }

        private void EditTransaction() {
            if (VerifyTextBox()) {
                TransactionModificationWindow win = new TransactionModificationWindow(txt_TransactionID.Text);
                win.ShowDialog();
                this.LoadTransactions();
            }
        }

        private void TransactionDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (TransactionDataGrid.HasItems) {
                DataRowView row = (DataRowView)TransactionDataGrid.SelectedItem;
                txt_TransactionID.Text = row.Row[1].ToString();
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
