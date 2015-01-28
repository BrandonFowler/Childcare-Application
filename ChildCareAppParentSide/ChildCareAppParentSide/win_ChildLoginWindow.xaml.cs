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

        public win_ChildLogin() {
            InitializeComponent();
        }//end default constructor

        public win_ChildLogin(string ID) {
            InitializeComponent();
            this.parentID = ID;
            this.db = new Database();
            setUpCheckInBox();
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
                    Image Image = buildImage(childrenData[x, 0]);
                    lst_CheckInBox.Items.Add(new Child(childrenData[x, 1], Image));
                }
            }
        }//end setUpCheckInBox

        private Image buildImage(string path) {
            Image image = new Image();
            image.Width = 60;
            BitmapImage bitmapImage = new BitmapImage();
            var fileInfo = new FileInfo(@"..\..\default.jpg");
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(fileInfo.FullName);
            bitmapImage.DecodePixelWidth = 60;
            bitmapImage.EndInit();
            image.Source = bitmapImage;
            return image;
        }//end buildImage

    }//end win_ChildLoginWindow(Class)
}
