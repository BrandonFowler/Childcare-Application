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

namespace ChildCareAppParentSide {
  
    public partial class win_Confirmation : Window {

        public string message = "Are you sure?";

        public win_Confirmation() {
            InitializeComponent();
        }//end default

        public win_Confirmation(string message) {
            InitializeComponent();
            this.message = message;
            lbl_AreYouSure.Content = message;
        }//end class

        private void btn_Yes_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;  
            this.Close(); 
        }//end btn_Yes_Click

        private void btn_No_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;   
            this.Close(); 
        }//end btn_No_Click

    }//end window
}
