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
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e) {

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
            this.txt_EventName.Text = transDB.GetEventName(this.transactionID);
            this.txt_TransactionTotal.Text = String.Format("{0:0.00}", transDB.GetTransactionTotal(this.transactionID));
        }
    }
}
