using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChildCareAppParentSide {
   
    public partial class win_ChildLogin : Window {

        private string guardianID;
        private ChildCheckInDatabase db;
        private DateTime updateTime;

        public win_ChildLogin(string ID) {
            InitializeComponent();
            this.guardianID = ID;
            this.db = new ChildCheckInDatabase();
            setUpCheckInBox();
            setUpParentDisplay();
            eventsSetup();
            this.updateTime = new DateTime();
            updateTime = DateTime.Now;
            lbl_Time.DataContext = updateTime;
        }//end constructor

        private void btn_LogOutParent_Click(object sender, RoutedEventArgs e) {
            exitToLogin();
        }//end btn_LogOutParent

        private void setUpCheckInBox() {
            string[,] childrenData = db.findChildren(this.guardianID);

            if(childrenData == null){
                return;
            }

            for (int x = 0; x < childrenData.GetLength(0); x++) {
                Image image = buildImage(childrenData[x, 6], 60);
                if (!db.isCheckedIn(childrenData[x, 0],this.guardianID)){
                    lst_CheckInBox.Items.Add(new Child(childrenData[x, 1], childrenData[x, 2], image, childrenData[x, 0], childrenData[x, 3], childrenData[x, 4], childrenData[x, 5]));
                }
                else{
                    lst_CheckOutBox.Items.Add(new Child(childrenData[x, 1], childrenData[x, 2], image, childrenData[x, 0], childrenData[x, 3], childrenData[x, 4], childrenData[x, 5]));
                }
            }
          
        }//end setUpCheckInBox

        private Image buildImage(string path, int size) {
            Image image = new Image();
            image.Width = size;
            
            try {
                BitmapImage bitmapImage = new BitmapImage();
                var fileInfo = new FileInfo(@"" + path);
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            } catch {
                BitmapImage bitmapImage = new BitmapImage();
                var fileInfo = new FileInfo(@"../../../../Photos/default.jpg");
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            return image;
        }//end buildImage

        private void btn_CheckIn_Click(object sender, RoutedEventArgs e) {
            if (cbo_EventChoice.SelectedItem != null) {
                if (lst_CheckInBox.SelectedItem != null) {
                    string eventID = ((ComboBoxItem)cbo_EventChoice.SelectedItem).Tag.ToString();
                    string childID = ((Child)lst_CheckInBox.SelectedItem).ID;
                    string birthday = ((Child)lst_CheckInBox.SelectedItem).birthday;
                    db.checkIn(childID, eventID, guardianID, birthday);
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
                string childID = ((Child)lst_CheckOutBox.SelectedItem).ID;
                db.checkOut(childID, guardianID);
                lst_CheckInBox.Items.Add(lst_CheckOutBox.SelectedItem);
                lst_CheckOutBox.Items.Remove(lst_CheckOutBox.SelectedItem);
            }
        }//end btn_CheckOut_Click

        public void setUpParentDisplay() {
            string [] parentInfo = db.getParentInfo(this.guardianID);
            if (parentInfo != null){
                lbl_ParentName.Content = parentInfo[2] + " " + parentInfo[3];
                img_ParentPic.Source = (buildImage(parentInfo[11], 150)).Source;
            }
            else{
                exitToLogin();
            }
        }//end setUpParentDisplay

        private void exitToLogin(){
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close();
        }//end exitToLogin

        public void eventsSetup() {
            string[,] events = db.getEvents();

            if (events != null)
            {
                for (int x = 0; x < events.GetLength(0); x++){
                    ComboBoxItem newEvent = new ComboBoxItem() { Content = events[x, 1], Tag = events[x, 0] };
                    cbo_EventChoice.Items.Add(newEvent);
                }

                if (events.GetLength(0) == 1){
                    cbo_EventChoice.SelectedItem = cbo_EventChoice.Items[0];
                }
            }
            else{
                exitToLogin();
            }
        }//end eventSetup

    }//end win_ChildLoginWindow(Class)
}
