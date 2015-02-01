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

namespace ChildCareApp {
     
    /// <summary>
    /// Interaction logic for win_DeleteConformation.xaml
    /// </summary>
    
    public partial class win_DeleteConformation : Window {

        public win_DeleteConformation() {
            
            InitializeComponent();
        }//end class

        private void btn_Yes_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;  
            this.Close(); 
        }//end btn_Yes_Click

        private void btn_No_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;   
            this.Close(); 
        }//end btn_No_Click

    }//end window
}
