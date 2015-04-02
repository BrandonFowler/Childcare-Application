using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AdminTools;
using System.Windows.Media;
using DatabaseController;

namespace ParentTools {
   
    public partial class ChildLogin : Window {

        private string guardianID;
        private ChildCheckInDatabase db;
        private DateTime updateTime;

        public ChildLogin(string ID) {
            InitializeComponent();
            this.guardianID = ID;
            this.db = new ChildCheckInDatabase();
            setUpCheckInBox();
            setUpParentDisplay();
            eventsSetup();
            this.updateTime = new DateTime();
            updateTime = DateTime.Now;
            lbl_Time.DataContext = updateTime;
            lst_CheckInBox.GotFocus += CheckInGotFocus;
            lst_CheckOutBox.SelectionChanged += CheckOutBoxSelectionChanged;
        }

        private void btn_LogOutParent_Click(object sender, RoutedEventArgs e) {
            exitToLogin();
        }

        private void setUpCheckInBox() {
            string[,] childrenData = db.findChildren(this.guardianID);

            if(childrenData == null){
                return;
            }

            for (int x = 0; x < childrenData.GetLength(0); x++) {
                Image image = buildImage(childrenData[x, 6], 60);
                if (!db.isCheckedIn(childrenData[x, 0],this.guardianID)){
                    lst_CheckInBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2], image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
                }
                else{
                    lst_CheckOutBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2], image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
                }
            }
          
        }

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
                var fileInfo = new FileInfo(@"../../Pictures/default.jpg");
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            return image;
        }

        private void btn_CheckIn_Click(object sender, RoutedEventArgs e) {
            if (cbo_EventChoice.SelectedItem != null) {
                if (lst_CheckInBox.SelectedItem != null) {
                    string eventID = ((ComboBoxItem)cbo_EventChoice.SelectedItem).Tag.ToString();
                    string childID = ((Child)lst_CheckInBox.SelectedItem).ID;
                    string birthday = ((Child)lst_CheckInBox.SelectedItem).birthday;
                    bool success = db.checkIn(childID, eventID, guardianID, birthday);
                    if (success){
                        lst_CheckOutBox.Items.Add(lst_CheckInBox.SelectedItem);
                        lst_CheckInBox.Items.Remove(lst_CheckInBox.SelectedItem);
                    }
                }
            }
            else {
                MessageBox.Show("Please choose and event.");
            }
            btn_CheckIn.Background = Brushes.Blue;
        }

        private void btn_CheckOut_Click(object sender, RoutedEventArgs e) {
            if (lst_CheckOutBox.SelectedItem != null) {
                string childID = ((Child)lst_CheckOutBox.SelectedItem).ID;
                bool success = db.checkOut(childID, guardianID);
                if (success){
                    lst_CheckInBox.Items.Add(lst_CheckOutBox.SelectedItem);
                    lst_CheckOutBox.Items.Remove(lst_CheckOutBox.SelectedItem);
                }
            }
            btn_CheckOut.Background = Brushes.Blue;
        }

        public void setUpParentDisplay() {
            string [] parentInfo = db.getParentInfo(this.guardianID);
            if (parentInfo != null){
                lbl_ParentName.Content = parentInfo[2] + " " + parentInfo[3];
                img_ParentPic.Source = (buildImage(parentInfo[11], 150)).Source;
            }
            else{
                exitToLogin();
            }
        }

        private void exitToLogin(){
            ParentLogin loginWindow = new ParentLogin();
            loginWindow.Show();
            this.Close();
        }

        public void eventsSetup() {
            string[] events = db.getEvents();

            if (events != null)
            {
                for (int x = 0; x < events.GetLength(0); x++){
                    ComboBoxItem newEvent = new ComboBoxItem() { Content = events[x], Tag = events[x] };
                    cbo_EventChoice.Items.Add(newEvent);
                }

                if (events.GetLength(0) == 1){
                    cbo_EventChoice.SelectedItem = cbo_EventChoice.Items[0];
                }
            }
            else{
                exitToLogin();
            }
        }

        private void CheckInGotFocus(object sender, System.EventArgs e) {
            if (cbo_EventChoice.SelectedItem != null) {
                btn_CheckIn.Background = Brushes.Green;
            }
        }
        private void CheckOutBoxSelectionChanged(object sender, System.EventArgs e) {
            btn_CheckOut.Background = Brushes.Green;
        }

    }
}
