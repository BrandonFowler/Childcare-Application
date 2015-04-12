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
using DatabaseController;

namespace ParentTools {
   
    public partial class AdminChildCheckIn : Window {

        private string guardianID;
        private ParentToolsDB db;

        public AdminChildCheckIn() {
            InitializeComponent();
            lst_Guardians.SelectionChanged += lst_Guardians_IndexChange;
            this.db = new ParentToolsDB();
            cnv_GuardianPic.Background = new SolidColorBrush(Colors.DimGray);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            ParentLogin loginWindow = new ParentLogin();
            loginWindow.Show();
            this.Close();
        }

        private void lst_Guardians_IndexChange(object sender, System.EventArgs e) {
            if (lst_Guardians.SelectedItem == null) {
                return;
            }
            string guardianInfo = lst_Guardians.SelectedItem.ToString();
            this.guardianID = guardianInfo.Substring(guardianInfo.LastIndexOf(' ') + 1);
            string imageLink = db.getGuardianImagePath(this.guardianID);
            if (imageLink != null) {
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                cnv_GuardianPic.Background = ib;
            }
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e) {
            cleanDisplay();
            if (String.IsNullOrWhiteSpace(txt_SearchBox.Text)) {
                MessageBox.Show("Please enter a name or ID.");
                return;
            }
            int n;
            bool isNumeric = int.TryParse(txt_SearchBox.Text, out n);
            if (isNumeric) {
                bool validated = db.validateGuardianID(txt_SearchBox.Text);
                if (validated) {
                    ChildLogin ChildLoginWindow = new ChildLogin(txt_SearchBox.Text);
                    ChildLoginWindow.Show();
                    ChildLoginWindow.WindowState = WindowState.Maximized;
                    this.Close();
                }
                else {
                    MessageBox.Show("No search results found");
                }
            }
            else {
                string[,] guardianInfo = db.retieveGuardiansByLastName(txt_SearchBox.Text);
                if (guardianInfo == null || guardianInfo.GetLength(0) == 0) {
                    MessageBox.Show("No search results found");
                    return;
                }
                setUpGuardianDisplay(guardianInfo);
            }
        }

        private void setUpGuardianDisplay(string[,] guardianInfo){
            for (int x = 0; x < guardianInfo.GetLength(0); x++) {
                string firstName = guardianInfo[x,0].PadRight(33);
                string lastName = guardianInfo[x, 1].PadRight(33);
                string ID = guardianInfo[x, 2];
                lst_Guardians.Items.Add(firstName + lastName + ID);
            }
            this.lbl_Categories.Visibility = Visibility.Visible;
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            if (lst_Guardians.SelectedItem == null) {
                MessageBox.Show("Please select a guardian.");
                return;
            }
            ChildLogin ChildLoginWindow = new ChildLogin(this.guardianID);
            ChildLoginWindow.Show();
            ChildLoginWindow.WindowState = WindowState.Maximized;
            this.Close();
        }

        private void cleanDisplay() {
            cnv_GuardianPic.Background = new SolidColorBrush(Colors.DimGray);
            this.lbl_Categories.Visibility = Visibility.Hidden;
            this.lst_Guardians.Items.Clear();
        }

    }
}
