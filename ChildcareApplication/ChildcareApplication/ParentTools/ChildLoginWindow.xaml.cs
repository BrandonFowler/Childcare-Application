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
        private ParentToolsDB db;
        private DateTime updateTime;

        public ChildLogin(string ID) {
            InitializeComponent();
            this.guardianID = ID;
            this.db = new ParentToolsDB();
            SetUpCheckInBox();
            cnv_GuardianPic.Background = new SolidColorBrush(Colors.Aqua);
            SetUpParentDisplay();
            EventsSetup();
            this.updateTime = new DateTime();
            updateTime = DateTime.Now;
            lbl_Time.DataContext = updateTime;
            lst_CheckInBox.GotFocus += CheckInGotFocus;
            lst_CheckOutBox.SelectionChanged += CheckOutBoxSelectionChanged;
        }

        private void btn_LogOutParent_Click(object sender, RoutedEventArgs e) {
            ExitToLogin();
        }

        private void SetUpCheckInBox() {
            string[,] childrenData = db.FindChildren(this.guardianID);
            if(childrenData == null){
                return;
            }
            for (int x = 0; x < childrenData.GetLength(0); x++) {
                Image image = BuildImage(childrenData[x, 6], 60);
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

        private Image BuildImage(string path, int size) {
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
                    bool success = db.CheckIn(childID, eventID, guardianID, birthday);
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
                bool success = CompleteTransaction(childID, guardianID);
                if (success){
                    lst_CheckInBox.Items.Add(lst_CheckOutBox.SelectedItem);
                    lst_CheckOutBox.Items.Remove(lst_CheckOutBox.SelectedItem);
                }
            }
            btn_CheckOut.Background = Brushes.Blue;
        }

        public void SetUpParentDisplay() {
            string [] parentInfo = db.GetParentInfo(this.guardianID);
            string imageLink = db.GetGuardianImagePath(this.guardianID);
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
            ParentLogin loginWindow = new ParentLogin();
            loginWindow.Show();
            this.Close();
        }

        public void EventsSetup() {
            string[] events = db.GetEvents();
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

        private void CheckInGotFocus(object sender, System.EventArgs e) {
            if (cbo_EventChoice.SelectedItem != null) {
                btn_CheckIn.Background = Brushes.Green;
            }
        }

        private void CheckOutBoxSelectionChanged(object sender, System.EventArgs e) {
            btn_CheckOut.Background = Brushes.Green;
        }

        private bool CompleteTransaction(string childID, string guardianID) {
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = DateTime.Now.ToString();
            string currentDateString = Convert.ToDateTime(dateTimeString).ToString("yyyy-MM-dd");
            string currentTimeString = Convert.ToDateTime(dateTimeString).ToString("HH:mm:ss");
            string allowanceID = db.GetTransactionAllowanceID(guardianID, childID);
            string[] transaction = db.FindTransaction(allowanceID);
            if (transaction == null || allowanceID == null) {
                MessageBox.Show("Unable to check out child. Please log out then try again.");
                return false;
            }
            string transactionID = transaction[0];
            string eventName = transaction[1];
            string checkInTime = transaction[4];
            bool isLate = false;
            bool ishourly = CheckIfHourly(eventName);
            double eventFee = FindEventFee(guardianID, eventName);
            TimeSpan TimeSpanTime = TimeSpan.Parse(currentTimeString);
            checkInTime = Convert.ToDateTime(checkInTime).ToString("HH:mm:ss");
            TimeSpan TimeSpanCheckInTime = TimeSpan.Parse(checkInTime);
            double lateTime = db.CheckIfPastClosing(currentDateTime.DayOfWeek.ToString(), TimeSpanTime);
            double hourDifference = TimeSpanTime.Hours - TimeSpanCheckInTime.Hours;
            double minuteDifference = TimeSpanTime.Minutes - TimeSpanCheckInTime.Minutes;
            double totalCheckedInHours = hourDifference + (minuteDifference / 60.0);
            double lateMaximum = db.GetEventHourCap(eventName);
            if (totalCheckedInHours > lateMaximum) {
                double timeDifference = totalCheckedInHours - lateMaximum;
                if (timeDifference > lateTime) {
                    lateTime = timeDifference;
                    totalCheckedInHours = lateMaximum;
                }
                else {
                    totalCheckedInHours = totalCheckedInHours - lateTime;
                }
                isLate = true;
            }
            else if (lateTime > 0) {
                totalCheckedInHours = totalCheckedInHours - lateTime;
                isLate = true;
            }

            if (ishourly) {
                eventFee = eventFee * totalCheckedInHours;
                eventFee = Math.Round(eventFee, 2, MidpointRounding.AwayFromZero);
            }
        
            eventFee = eventFee - BillingCapCalc(eventName, guardianID, transaction[3], eventFee);
            string eventFeeRounded = eventFee.ToString("f2");
            db.CheckOut(currentTimeString, eventFeeRounded, allowanceID);
            db.AddToBalance(guardianID, eventFee);
            if (isLate) {
                eventName = "Late Fee";
                double lateFee = db.GetLateFee(eventName);
                lateFee = lateFee * lateTime;
                int maxTransactionID = db.GetNextPrimary("ChildcareTransaction_ID", "ChildcareTransaction");
                string sMaxTransactionID = Convert.ToString(maxTransactionID);
                sMaxTransactionID = sMaxTransactionID.ToString().PadLeft(10, '0');
                db.AddLateFee(sMaxTransactionID, eventName, allowanceID, currentDateString, lateFee);
                db.AddToBalance(guardianID, lateFee);
            }
            return true;
        }

        public bool CheckIfHourly(string eventName) {
            string[] eventData = db.GetEvent(eventName);
            if (eventData == null) {
                return false;
            }
            if (String.IsNullOrWhiteSpace(eventData[1])) {
                return false;
            }
            else {
                return true;
            }
        }

        public double FindEventFee(string guardianID, string eventName) {
            bool discount = false;
            int childrenCheckedIn = db.NumberOfCheckedIn(guardianID);
            string[] eventData = db.GetEvent(eventName);
            if ((childrenCheckedIn > 1) && (eventData[2] != null || eventData[4] != null)) {
                discount = true;
            }
            if (eventData == null) {
                return 0.0;
            }
            if (discount) {
                if (String.IsNullOrWhiteSpace(eventData[2])) {
                    return Convert.ToDouble(eventData[4]);
                }
                else {
                    return Convert.ToDouble(eventData[2]);
                }
            }
            else {
                if (String.IsNullOrWhiteSpace(eventData[1])) {
                    return Convert.ToDouble(eventData[3]);
                }
                else {
                    return Convert.ToDouble(eventData[1]);
                }
            }
        }

        public double BillingCapCalc(string eventName, string guardianID, string transactionDate, double eventFee) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            double cap = db.GetBillingCap();
            int billingStart = db.GetBillingStart();
            int billingEnd = db.GetBillingEnd();
            DateTime DTStart;
            DateTime DTEnd;
            if (DateTime.Now.Day > billingEnd) {
                DTStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingStart);
                int endMonth = DTStart.Month + 1;
                if (endMonth == 13) {
                    int endYear = DTStart.Year + 1;
                    DTEnd = new DateTime(endYear, 1, billingEnd);
                }
                else {
                    DTEnd = new DateTime(DateTime.Now.Year, endMonth, billingEnd);
                }
            }
            else {
                DTEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingEnd);
                int startMonth = DTEnd.Month - 1;
                if (startMonth == 0) {
                    int startYear = DTEnd.Year - 1;
                    DTStart = new DateTime(startYear, 12, billingStart);
                }
                else {
                    DTStart = new DateTime(DateTime.Now.Year, startMonth, billingStart);
                }
            }
            string start = DTStart.ToString("yyyy-MM-dd");
            string end = DTEnd.ToString("yyyy-MM-dd");
            if (eventName.CompareTo("Regular Childcare") == 0 || eventName.CompareTo("Infant Childcare") == 0 || eventName.CompareTo("Adolescent Childcare") == 0) {
                object recordFound = db.SumRegularCare(start, end, familyID);
                double sum;
                if (recordFound == DBNull.Value || recordFound == null) {
                    return 0;
                }
                else {
                    sum = Convert.ToDouble(recordFound);
                }
                double total = sum + eventFee;
                double capdiff = total - cap;
                if (capdiff > 0 && capdiff < eventFee) {
                    return capdiff;
                }
                else if (capdiff >= eventFee) {
                    return eventFee;
                }
            }
            return 0.0;
        }

    }
}
