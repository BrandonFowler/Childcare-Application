using DatabaseController;
using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdminTools {

    public partial class SelectGuardian : Window {

        private string guardianID;
        private List<string> allowanceIDs;
        private TransactionModificationWindow transWin;

        public SelectGuardian(List<string> allowanceIDs, TransactionModificationWindow transWin) {
            InitializeComponent();
            dta_GuardianList.SelectionChanged += lst_Guardians_IndexChange;
            this.allowanceIDs = allowanceIDs;
            this.transWin = transWin;
            this.MouseDown += WindowMouseDown;
            LoadGrid();
        }

        private void LoadGrid() {
            ConnectionsDB conDB = new ConnectionsDB();
            DataTable table = new DataTable();
            string[] guardianName = new string[2]; //[first, last]

            table.Columns.Add("First Name", typeof(string));
            table.Columns.Add("Last Name", typeof(string));
            table.Columns.Add("Allowance ID", typeof(string));

            for (int i = 0; i < this.allowanceIDs.Count; i++) {
                guardianName = conDB.GetGuardianName(allowanceIDs[i]);
                table.Rows.Add(guardianName[0], guardianName[1], allowanceIDs[i]);
            }
            this.dta_GuardianList.ItemsSource = table.DefaultView;
        }

        private void lst_Guardians_IndexChange(object sender, System.EventArgs e) {
            if (dta_GuardianList.SelectedItem != null) {
                GuardianInfoDB parentDB = new GuardianInfoDB();
                ConnectionsDB conDB = new ConnectionsDB();
                String allowanceID = "";
                DataRowView selectedFile = (DataRowView)dta_GuardianList.SelectedItem;
                allowanceID = Convert.ToString(selectedFile.Row.ItemArray[2]);

                this.guardianID = conDB.GetGuardianID(allowanceID);
                string imageLink = parentDB.GetGuardianImagePath(this.guardianID);
                if (imageLink != null) {
                    ImageBrush ib = new ImageBrush();
                    ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                    cnv_GuardianPic.Background = ib;
                    cnv_GuardianPic.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void btn_Select_Click(object sender, RoutedEventArgs e) {
            if (this.dta_GuardianList.SelectedIndex != -1) {
                DataRowView row = (DataRowView)this.dta_GuardianList.SelectedItem;
                this.transWin.SetAllowanceID(row.Row[2].ToString());
                this.Close();
            } else {
                WPFMessageBox.Show("You must select a guardian from the list!");
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
