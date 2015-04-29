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
using System.Data;

namespace GuardianTools {
   
    public partial class AdminChildCheckIn : Window {

        private string guardianID;
        private ConnectionsDB db;

        public AdminChildCheckIn() {
            InitializeComponent();
            dta_GuardianList.SelectionChanged += lst_Guardians_IndexChange;
            this.db = new ConnectionsDB();
            txt_SearchBox.Focus();
            this.txt_SearchBox.KeyDown += new KeyEventHandler(KeyPressedEnter);
            this.dta_GuardianList.KeyDown += new KeyEventHandler(KeyPressedEnter);
            this.MouseDown += WindowMouseDown;
        }

        private void KeyPressedEnter(Object o, KeyEventArgs e) {
                if (e.Key == Key.Return) {
                    if (txt_SearchBox.IsSelectionActive) {
                        Search();
                    }
                    else if (dta_GuardianList.SelectedItem != null) {
                        Login();
                    }
                }    
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            GuardianCheckIn loginWindow = new GuardianCheckIn();
            loginWindow.Show();
            this.Close();
        }

        private void lst_Guardians_IndexChange(object sender, System.EventArgs e) {
            if (dta_GuardianList.SelectedItem == null) {
                return;
            }
            GuardianInfoDB parentDB = new GuardianInfoDB();
            String guardianInfo = "";
            for (int i = 0; i < dta_GuardianList.SelectedItems.Count; i++){
                System.Data.DataRowView selectedFile = (System.Data.DataRowView)dta_GuardianList.SelectedItems[i];
                guardianInfo = Convert.ToString(selectedFile.Row.ItemArray[2]);
            }
            this.guardianID = guardianInfo;
            string imageLink = parentDB.GetGuardianImagePath(this.guardianID);
            if (imageLink != null) {
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                cnv_GuardianPic.Background = ib;
                cnv_GuardianPic.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e) {
            Search();
        }

        public void Search() {
            GuardianInfoDB parentDB = new GuardianInfoDB();
            CleanDisplay();
            if (String.IsNullOrWhiteSpace(txt_SearchBox.Text)) {
                MessageBox.Show("Please enter a name or ID.");
                return;
            }
            int n;
            bool isNumeric = int.TryParse(txt_SearchBox.Text, out n);
            if (isNumeric) {
                bool validated = parentDB.ValidateGuardianID(txt_SearchBox.Text);
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
                DataTable guardianInfo = parentDB.RetieveGuardiansByLastName(txt_SearchBox.Text);
                if (guardianInfo == null) {
                    MessageBox.Show("No search results found");
                    return;
                }
                dta_GuardianList.ItemsSource = guardianInfo.DefaultView;
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            Login();
        }

        private void Login() {
            if (dta_GuardianList.SelectedItem == null) {
                MessageBox.Show("Please select a guardian.");
                return;
            }
            ChildLogin ChildLoginWindow = new ChildLogin(this.guardianID);
            ChildLoginWindow.Show();
            ChildLoginWindow.WindowState = WindowState.Maximized;
            this.Close();
        }

        private void CleanDisplay() {
            var bc = new BrushConverter();
            cnv_GuardianPic.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            this.dta_GuardianList.Items.Clear();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

    }
}
