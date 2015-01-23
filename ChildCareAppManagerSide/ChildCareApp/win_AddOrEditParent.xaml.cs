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
    /// Interaction logic for win_AddOrEditParent.xaml
    /// </summary>
    /// 
    /* This window is for the admin to be able to choose to add a new parent or 
     * to Edit or Delete an existing one. I dont know if we want the do add parent here, 
     * so just remove that option if you think it is not something the admin should do here. 
     */
    public partial class win_AddOrEditParent : Window {
        public win_AddOrEditParent() {
            InitializeComponent();
        }

        private void btn_AddNewParent_Click(object sender, RoutedEventArgs e) {



        }

        private void btn_EditOrDeleteParent_Click(object sender, RoutedEventArgs e) {

            win_ParentLogin ParentLogin = new win_ParentLogin();
            ParentLogin.Show();
            this.Close(); 
        }
    }
}
