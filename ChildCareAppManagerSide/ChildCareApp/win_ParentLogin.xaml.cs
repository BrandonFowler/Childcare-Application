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

namespace ChildCareApp
{
    /// <summary>
    /// Interaction logic for win_ParentLogin.xaml
    /// </summary>
    /// 
    /* If you chose to edit a prent, you will need the ID of them 
     * That is what this window is for, for the admin to enter an ID of a user 
     * for editing or deleting. 
     * Should we have PIN also required here? 
     */
    public partial class win_ParentLogin : Window {
        public win_ParentLogin() {
            InitializeComponent();
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {

            //need to do checks to see if parent id exists
            //this is just to get the flow going for testing

            win_AdminEditParentInfo AdminEditParentWindow = new win_AdminEditParentInfo();
            AdminEditParentWindow.Show();
            this.Close(); 

        }
    }
}
