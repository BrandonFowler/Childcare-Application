using System;
using System.Collections.Generic;
using System.IO;
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
   
    public partial class win_ChildLogin : Window {

        private string parentID;
        private Database db;
        private DateAndTime updateTime;

        public win_ChildLogin() {
            InitializeComponent();
        }//end default constructor

        public win_ChildLogin(string ID) {
            InitializeComponent();
            this.parentID = ID;
            this.db = new Database();
            setUpCheckInBox();
            setUpParentDisplay();
            eventsSetup();
            this.updateTime = new DateAndTime();
            updateTime.Update();
            lbl_Time.DataContext = updateTime;
        }//end constructor

        private void btn_LogOutParent_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close();
        }//end btn_LogOutParent

        private void setUpCheckInBox() {
            string[,] childrenData = db.findChildren(this.parentID);
            if (childrenData != null) {
                for (int x = 0; x < childrenData.GetLength(0); x++) {
                    Image Image = buildImage(childrenData[x, 0], 60);
                    lst_CheckInBox.Items.Add(new Child(childrenData[x, 1], Image));
                }
            }
        }//end setUpCheckInBox

        private Image buildImage(string path, int size) {
            Image image = new Image();
            image.Width = size;
            BitmapImage bitmapImage = new BitmapImage();
            var fileInfo = new FileInfo(@"..\..\default.jpg");
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(fileInfo.FullName);
            bitmapImage.DecodePixelWidth = size;
            bitmapImage.EndInit();
            image.Source = bitmapImage;
            return image;
        }//end buildImage

        private void btn_CheckIn_Click(object sender, RoutedEventArgs e) {
            if (cbo_EventChoice.SelectedItem != null) {
                if (lst_CheckInBox.SelectedItem != null) {
                    lst_CheckOutBox.Items.Add(lst_CheckInBox.SelectedItem);
                    lst_CheckInBox.Items.Remove(lst_CheckInBox.SelectedItem);
                }
            }
            else {
                MessageBox.Show("Please choose and event.");
            }
        }//end btn_CheckIn_Click

        private void btn_CheckOut_Click(object sender, RoutedEventArgs e) {
            if (lst_CheckOutBox.SelectedItem != null) {
                lst_CheckInBox.Items.Add(lst_CheckOutBox.SelectedItem);
                lst_CheckOutBox.Items.Remove(lst_CheckOutBox.SelectedItem);
            }
        }//end btn_CheckOut_Click

        public void setUpParentDisplay() {
            string [] parentInfo = db.getParentInfo(this.parentID);
            lbl_ParentName.Content = parentInfo[1];
            img_ParentPic.Source = (buildImage("default.jpg", 100)).Source;
        }//end setUpParentDisplay

        public void eventsSetup() {
            cbo_EventChoice.Items.Add("Normal Price");
        }//end eventSetup

    }//end win_ChildLoginWindow(Class)
}
