using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AdminTools;
using System.Windows.Media;
using DatabaseController;
using System.Windows.Input;
using MessageBoxUtils;

namespace GuardianTools {
   
    public partial class ChildLogin : Window {

        private string guardianID;
        private ConnectionsDB db;
        private DateTime updateTime;

        public ChildLogin(string ID) {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.guardianID = ID;
            this.db = new ConnectionsDB();
            SetUpCheckInBox();
            SetUpParentDisplay();
            EventsSetup();
            this.updateTime = new DateTime();
            updateTime = DateTime.Now;
            lbl_Time.DataContext = updateTime;
            this.lst_CheckInBox.GotFocus += listBoxFocus;
            this.lst_CheckOutBox.GotFocus += listBoxFocus;
            this.MouseDown += WindowMouseDown;
        }

        private void btn_LogOutParent_Click(object sender, RoutedEventArgs e) {
            ExitToLogin();
        }

        private void listBoxFocus(object sender, RoutedEventArgs e){
            if ((sender as ListBox) != null && (sender as ListBox).Name.CompareTo("lst_CheckInBox") == 0) {
                if (this.lst_CheckInBox.HasItems) {
                    this.lst_CheckInBox.SelectedIndex = 0;
                }
            }
            else {
                if (this.lst_CheckOutBox.HasItems) {
                    this.lst_CheckOutBox.SelectedIndex = 0;
                }
            }
        }

        public void SetUpCheckInBox() {
            ChildInfoDatabase childDB = new ChildInfoDatabase();
            string[,] childrenData = childDB.FindChildren(this.guardianID);
            if(childrenData == null){
                return;
            }
            for (int x = 0; x < childrenData.GetLength(0); x++) {
                Image image = BuildImage(childrenData[x, 6], 70);
                if (!db.IsCheckedIn(childrenData[x, 0],this.guardianID)){
                    lst_CheckInBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2], 
                        image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
                }
                else{
                    lst_CheckOutBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2], 
                        image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
                }
            } 
        }

        public Image BuildImage(string path, int size) {
            Image image = new Image();
            image.Width = size;
            try {
                BitmapImage bitmapImage = new BitmapImage();
                var fileInfo = new FileInfo(@"" + path);
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.DecodePixelHeight = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            catch (System.IO.DirectoryNotFoundException) {
                WPFMessageBox.Show("Error loading photo. Pease insure your photos are in the correct directory.");
                BitmapImage bitmapImage = new BitmapImage();
                var fileInfo = new FileInfo(@"" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "Childcare Application/Pictures/default.jpg"); //TAG: pictures access
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.DecodePixelHeight = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            catch(Exception){
                WPFMessageBox.Show("Unable to load photo for unknown reason.");
            }
            return image;
        }

        private void btn_CheckIn_Click(object sender, RoutedEventArgs e) {
            if (cbo_EventChoice.SelectedItem != null) {
                if (lst_CheckInBox.SelectedItem != null) {
                    string eventID = ((ComboBoxItem)cbo_EventChoice.SelectedItem).Tag.ToString();
                    string childID = ((Child)lst_CheckInBox.SelectedItem).ID;
                    string birthday = ((Child)lst_CheckInBox.SelectedItem).birthday;
                    bool success = db.CheckIn(childID, eventID, guardianID, birthday);
                    if (success){
                        lst_CheckOutBox.Items.Add(lst_CheckInBox.SelectedItem);
                        lst_CheckInBox.Items.Remove(lst_CheckInBox.SelectedItem);
                    }
                }
                else {
                    WPFMessageBox.Show("Please select the child that you wish to check in.");
                }
            }
            else {
                WPFMessageBox.Show("Please choose and event.");
            }
        }

        private void btn_CheckOut_Click(object sender, RoutedEventArgs e) {
            TransactionDB transDB = new TransactionDB();
            if (lst_CheckOutBox.SelectedItem != null) {
                string childID = ((Child)lst_CheckOutBox.SelectedItem).ID;
                string allowanceID = transDB.GetIncompleteTransAllowanceID(guardianID, childID);
                TransactionCharge transactionCharge = new TransactionCharge(this.guardianID, allowanceID);
                bool success = transactionCharge.PrepareTransaction(childID, guardianID);
                if (success){
                    lst_CheckInBox.Items.Add(lst_CheckOutBox.SelectedItem);
                    lst_CheckOutBox.Items.Remove(lst_CheckOutBox.SelectedItem);
                }
            }
            else {
                WPFMessageBox.Show("Please select the child that you wish to check out.");
            }
        }

        public void SetUpParentDisplay() {
            GuardianInfoDB parentDB = new GuardianInfoDB();
            string [] parentInfo = parentDB.GetParentInfo(this.guardianID);
            string imageLink = parentDB.GetGuardianImagePath(this.guardianID);
            if (parentInfo != null){
                lbl_ParentName.Content = parentInfo[2] + " " + parentInfo[3];
                if (imageLink != null) {
                    ImageBrush ib = new ImageBrush();
                    ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                    cnv_GuardianPic.Background = ib;
                }
            }
            else{
                ExitToLogin();
            }
        }

        private void ExitToLogin(){
            GuardianCheckIn loginWindow = new GuardianCheckIn();
            loginWindow.Show();
            this.Close();
        }

        public void EventsSetup() {
            EventDB eventDB = new EventDB();
            string[] events = eventDB.GetCurrentEvents();
            if (events != null){
                for (int x = 0; x < events.GetLength(0); x++){
                    ComboBoxItem newEvent = new ComboBoxItem() { Content = events[x], Tag = events[x] };
                    cbo_EventChoice.Items.Add(newEvent);
                }

                if (events.GetLength(0) == 1){
                    cbo_EventChoice.SelectedItem = cbo_EventChoice.Items[0];
                }
            }
            else{
                ExitToLogin();
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

    }
}
