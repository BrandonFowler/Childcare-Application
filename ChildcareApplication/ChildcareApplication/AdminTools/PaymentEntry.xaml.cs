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

namespace ChildcareApplication.AdminTools {
    public partial class PaymentEntry : Window {
        private String guardianID;
        private ParentReport callingWindow;
        public PaymentEntry(String guardianID, ParentReport parentReport) {
            InitializeComponent();
            this.guardianID = guardianID;
            this.callingWindow = parentReport;
            InitializeCurrentBalance();
            this.txt_PaymentAmount.Focus();
        }

        private void InitializeCurrentBalance() {
            GuardianInfoDB parentInfoDB = new GuardianInfoDB();

            lbl_CurrentBalance.Content += " " + parentInfoDB.GetCurrentDue(guardianID);
        }

        private void btn_SubmitPayment_Click(object sender, RoutedEventArgs e) {
            SubmitPayment();
        }

        private void SubmitPayment() {
            TransactionDB transDB = new TransactionDB();
            Double num;

            if (Double.TryParse(txt_PaymentAmount.Text, out num) && num > 0) {
                num *= -1;
                if (num.ToString().Contains('.')) {
                    if (num.ToString().Split('.')[1].Length < 3) {
                        transDB.UpdateFamilyBalance(this.guardianID, num);

                        this.callingWindow.UpdateCurDue(this.guardianID);
                        this.Close();
                    } else {
                        MessageBox.Show("You must enter a valid dollar number in the Payment Amount box.");
                    }
                } else {
                    transDB.UpdateFamilyBalance(this.guardianID, num);

                    this.callingWindow.UpdateCurDue(this.guardianID);
                    this.Close();
                }
            } else {
                MessageBox.Show("You must enter a valid dollar number in the Payment Amount box.");
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void txt_PaymentAmount_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SubmitPayment();
            }
        }
    }
}
