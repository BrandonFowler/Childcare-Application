using DatabaseController;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
    public partial class EditTransactionWindow : Window {
        public EditTransactionWindow() {
            InitializeComponent();
            LoadTransactions();
        }

        private string BuildQuery() {
            string query = "SELECT Guardian.Guardian_ID AS 'Guardian ID', ChildcareTransaction.ChildcareTransaction_ID AS 'Transaction ID', ";
            query += "strftime('%m/%d/%Y', ChildcareTransaction.TransactionDate) AS Date, Guardian.FirstName AS First, Guardian.LastName AS Last, ";
            query += "Guardian.Phone, Guardian.Address1, Guardian.Address2, Guardian.City, Guardian.StateAbrv AS State, Guardian.Zip, ";
            query += "'$' || printf('%.2f', ChildcareTransaction.TransactionTotal) AS 'Total Charges' ";
            query += "From Guardian NATURAL JOIN AllowedConnections NATURAL JOIN ChildcareTransaction NATURAL JOIN Family ";
            query += "ORDER BY ChildcareTransaction.TransactionDate;";

            return query;
        }

        private void LoadTransactions() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/ChildcareDB.s3db;Version=3;");
            String query = BuildQuery();
            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                DataTable table = new DataTable("Transactions");
                adapter.Fill(table);

                TransactionDataGrid.ItemsSource = table.DefaultView;

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
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
                db.DeleteTransaction(txt_TransactionID.Text);
                LoadTransactions();
            }
        }

        private bool VerifyTextBox() {
            TransactionDB db = new TransactionDB();
            int temp;

            if (txt_TransactionID.Text.Length == 10 && Int32.TryParse(txt_TransactionID.Text, out temp)) {
                if (db.TransactionExists(txt_TransactionID.Text)) {
                    return true;
                } else {
                    MessageBox.Show("The Transaction ID you entered does not exist in the database.  Verify you entered the correct Transaction ID.");
                    txt_TransactionID.Focus();
                    return false;
                }
            } else {
                MessageBox.Show("The Transaction ID you entered does not exist in the database.  Verify you entered the correct Transaction ID.");
                txt_TransactionID.Focus();
                return false;
            }
        }

        private void btn_NewTransaction_Click(object sender, RoutedEventArgs e) {
            TransactionModificationWindow win = new TransactionModificationWindow();
            win.ShowDialog();
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
    }
}
