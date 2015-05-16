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
using AdminTools;
using DatabaseController;
using MessageBoxUtils;

namespace ChildcareApplication.AdminTools {
    public partial class PaymentEntry : Window {
        private String guardianID;
        private ParentReport callingWindow;
        private string type;

        public PaymentEntry(String guardianID, ParentReport parentReport, string type) {
            InitializeComponent();
            this.type = type;
            this.guardianID = guardianID;
            this.callingWindow = parentReport;
            InitializeCurrentBalance();
            this.txt_PaymentAmount.Focus();
            this.MouseDown += WindowMouseDown;
        }

        private void InitializeCurrentBalance() {
            GuardianInfoDB parentInfoDB = new GuardianInfoDB();

            lbl_CurrentBalance.Content += " " + parentInfoDB.GetCurrentDue(guardianID, this.type);
        }

        private void btn_SubmitPayment_Click(object sender, RoutedEventArgs e) {
            SubmitPayment();
        }

        private void SubmitPayment() {
            double num;

            if (Double.TryParse(txt_PaymentAmount.Text, out num) && num > 0) {
                num *= -1;
                if (num.ToString().Contains('.')) {
                    if (num.ToString().Split('.')[1].Length < 3) {
                        UpdateBalance(num);
                    } else {
                        WPFMessageBox.Show("You must enter a valid dollar number in the Payment Amount box.");
                    }
                } else {
                    UpdateBalance(num);
                }
            } else {
                WPFMessageBox.Show("You must enter a valid dollar number in the Payment Amount box.");
            }
        }

        private void UpdateBalance(double num) {
            TransactionDB transDB = new TransactionDB();

            if (this.type == "Regular") {
                transDB.UpdateRegularBalance(this.guardianID, (num));
                this.callingWindow.UpdateRegularDue(this.guardianID);
            } else if (this.type == "Camp") {
                transDB.UpdateCampBalance(this.guardianID, (num));
                this.callingWindow.UpdateCampDue(this.guardianID);
            } else if (this.type == "Misc") {
                transDB.UpdateMiscBalance(this.guardianID, (num));
                this.callingWindow.UpdateMiscDue(this.guardianID);
            } else {
                WPFMessageBox.Show("A valid type was not sent to the PaymentEntry window!");
            }
            this.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void txt_PaymentAmount_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SubmitPayment();
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void txt_PaymentAmount_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)((TextBox)sender).SelectAll);
        }
    }
}
