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
                    lst_CheckInBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2], 
                        image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
                }
                else{
                    lst_CheckOutBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2], 
                        image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
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
                bool success = completeTransaction(childID, guardianID);
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

        private bool completeTransaction(string childID, string guardianID) {//This method still requires some cleanup/refactor
            DateTime currentDateTime = DateTime.Now;
            string dateTimeString = DateTime.Now.ToString();
            string currentDateString = Convert.ToDateTime(dateTimeString).ToString("yyyy-MM-dd");
            string currentTimeString = Convert.ToDateTime(dateTimeString).ToString("HH:mm:ss");
            string allowanceID = db.getTransactionAllowanceID(guardianID, childID);
            string[] transaction = db.findTransaction(allowanceID);
            if (transaction == null || allowanceID == null) {
                MessageBox.Show("Unable to check out child. Please log out then try again.");
                return false;
            }
            string transactionID = transaction[0];
            string eventName = transaction[1];
            string checkInTime = transaction[4];
            bool isLate = false;
            bool ishourly = checkIfHourly(eventName);
            double eventFee = findEventFee(guardianID, eventName);
            TimeSpan TimeSpanTime = TimeSpan.Parse(currentTimeString);
            checkInTime = Convert.ToDateTime(checkInTime).ToString("HH:mm:ss");
            TimeSpan TimeSpanCheckInTime = TimeSpan.Parse(checkInTime);
            double lateTime = db.checkIfPastClosing(currentDateTime.DayOfWeek.ToString(), TimeSpanTime);
            if (ishourly) {
                double hourDifference = TimeSpanTime.Hours - TimeSpanCheckInTime.Hours;
                double minuteDifference = TimeSpanTime.Minutes - TimeSpanCheckInTime.Minutes;
                double totalCheckedInHours = hourDifference + (minuteDifference / 60.0);
                if ((eventName.CompareTo("Regular Childcare") == 0 || eventName.CompareTo("Infant Childcare") == 0) && totalCheckedInHours > 3) {//Change to way this is done to accomidate dynamic hour cap per event and older child event
                    double timeDifference = totalCheckedInHours - 3;
                    if (timeDifference > lateTime) {
                        lateTime = timeDifference;
                        totalCheckedInHours = 3;
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

                eventFee = eventFee * totalCheckedInHours;
                eventFee = Math.Round(eventFee, 2, MidpointRounding.AwayFromZero);
            }
            else {
                if (lateTime > 0) {
                    isLate = true;
                }
            }
            eventFee = eventFee - billingCapCalc(eventName, guardianID, transaction[3], eventFee);
            string eventFeeRounded = eventFee.ToString("f2");
            db.checkOut(currentTimeString, eventFeeRounded, allowanceID);
            db.addToBalance(guardianID, eventFee);
            if (isLate) {
                eventName = "Late Fee";
                double lateFee = db.getLateFee(eventName);
                lateFee = lateFee * lateTime;
                int maxTransactionID = db.getNextPrimary("ChildcareTransaction_ID", "ChildcareTransaction");
                string sMaxTransactionID = Convert.ToString(maxTransactionID);
                sMaxTransactionID = sMaxTransactionID.ToString().PadLeft(10, '0');
                db.addLateFee(sMaxTransactionID, eventName, allowanceID, currentDateString, lateFee);
                db.addToBalance(guardianID, lateFee);
            }
            return true;
        }

        public bool checkIfHourly(string eventName) {
            string[] eventData = db.getEvent(eventName);
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

        public double findEventFee(string guardianID, string eventName) {
            bool discount = false;
            int childrenCheckedIn = db.numberOfCheckedIn(guardianID);
            string[] eventData = db.getEvent(eventName);
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

        public double billingCapCalc(string eventName, string guardianID, string transactionDate, double eventFee) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            double cap = 100;//Change from hard code
            int billingStart = 20;//Change from hard code
            int billingEnd = 19;//Change from hard code
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
            if (eventName.CompareTo("Regular Childcare") == 0 || eventName.CompareTo("Infant Childcare") == 0) {//Add older child
                object recordFound = db.sumRegularCare(start, end, familyID);
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
