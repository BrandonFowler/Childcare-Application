using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseController;
using System.Windows;

namespace GuardianTools {
    class TransactionCharge {

        private GuardianToolsSettings settings;
        private string guardianID;
        private ConnectionsDB db;
        private string allowanceID;
        private double lateTime;
        private bool isLate = false;

        public TransactionCharge(String guardianID, String allowanceID) {
            this.settings = new GuardianToolsSettings();
            this.guardianID = guardianID;
            this.allowanceID = allowanceID;
            this.db = new ConnectionsDB();
        }

        public void setLateTime(double lateTime) {
            this.lateTime = lateTime;
        }

        public void setIsLate(bool isLate) {
            this.isLate = isLate;
        }

        public bool PrepareTransaction(string childID, string guardianID) {
            TransactionDB transDB = new TransactionDB();
            string[] transaction = transDB.FindTransaction(this.allowanceID);
            if (transaction == null || this.allowanceID == null) {
                MessageBox.Show("Unable to check out child. Please log out then try again.");
                return false;
            }
            string eventName = transaction[1];
            string transactionDate = transaction[3];
            string checkInTime = transaction[4];
            checkInTime = Convert.ToDateTime(checkInTime).ToString("HH:mm:ss");
            string checkOutTime = DateTime.Now.ToString("HH:mm:ss");
            double eventFee = FindEventFee(guardianID, eventName);
            eventFee = CalculateTransaction(checkInTime, checkOutTime, eventName, eventFee);
            CompleteTransaction(eventFee);
            return true;
        }

        public double CalculateTransaction(string checkInTime, string checkOutTime, string eventName, double eventFee) {
            EventDB eventDB = new EventDB();
            TimeSpan TimeSpanCheckOut = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
            TimeSpan TimeSpanCheckIn = TimeSpan.Parse(checkInTime);
            this.lateTime = settings.CheckIfPastClosing(DateTime.Now.DayOfWeek.ToString(), TimeSpanCheckOut);
            double totalCheckedInHours = (TimeSpanCheckOut.Hours - TimeSpanCheckIn.Hours) + ((TimeSpanCheckOut.Minutes - TimeSpanCheckIn.Minutes) / 60.0);
            double lateMaximum = eventDB.GetEventHourCap(eventName);
            if (totalCheckedInHours > lateMaximum) {
                double timeDifference = totalCheckedInHours - lateMaximum;
                if (timeDifference > this.lateTime) {
                    this.lateTime = timeDifference;
                    totalCheckedInHours = lateMaximum;
                }
                else {
                    totalCheckedInHours = totalCheckedInHours - this.lateTime;
                }
                this.isLate = true;
            }
            else if (this.lateTime > 0) {
                totalCheckedInHours = totalCheckedInHours - this.lateTime;
                this.isLate = true;
            }
            return getCharge(eventFee, eventName, totalCheckedInHours);
        }

        public double getCharge(double eventFee, string eventName, double totalCheckedInHours){
            if (CheckIfHourly(eventName)) {
                eventFee = eventFee * totalCheckedInHours;
                eventFee = Math.Round(eventFee, 2, MidpointRounding.AwayFromZero);
            }
            eventFee = eventFee - GetBillingCap(eventName, guardianID, eventFee);
            if (totalCheckedInHours < 0) {
                MessageBox.Show("Negative value calculated for childcare charge. Please check your system clock for currency");
                eventFee = 0;
            }
            return eventFee;
        }

        public void CompleteTransaction(double eventFee) {
            TransactionDB transDB = new TransactionDB();
            string eventFeeRounded = eventFee.ToString("f2");
            db.CheckOut(DateTime.Now.ToString("HH:mm:ss"), eventFeeRounded, this.allowanceID);
            transDB.UpdateFamilyBalance(guardianID, eventFee);
            if (this.isLate) {
                double lateFee = CalculateLateFee();
            }
        }

        public double CalculateLateFee() {
            TransactionDB transDB = new TransactionDB();
            EventDB eventDB = new EventDB();
            String name = "Late Fee";
            double lateFee = eventDB.GetLateFee(name);
            lateFee = lateFee * this.lateTime;
            string maxTransactionID = transDB.GetNextTransID();
            transDB.AddLateFee(maxTransactionID, name, this.allowanceID, DateTime.Now.ToString("yyyy-MM-dd"), lateFee);
            transDB.UpdateFamilyBalance(guardianID, lateFee);
            return lateFee;
        }

        public bool CheckIfHourly(string eventName) {
            EventDB eventDB = new EventDB();
            string[] eventData = eventDB.GetEvent(eventName);
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
            EventDB eventDB = new EventDB();
            bool discount = false;
            int childrenCheckedIn = db.NumberOfCheckedIn(guardianID);
            string[] eventData = eventDB.GetEvent(eventName);
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

        public double GetBillingCap(string eventName, string guardianID, double eventFee) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            int billingStart = settings.GetBillingStart();
            int billingEnd = settings.GetBillingEnd();
            DateTime DTStart;
            DateTime DTEnd;
            if (DateTime.Now.Day > billingEnd) {
                DTStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingStart);
                DTEnd = FindBillingEnd(DTStart, billingEnd);
            }
            else {
                DTEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingEnd);
                DTStart = FindBillingStart(DTEnd, billingStart);
            }
            return BillingCapCalc(DTStart, DTEnd, eventName, familyID, eventFee);
        }

        public double BillingCapCalc(DateTime DTStart, DateTime DTEnd, string eventName, string familyID, double eventFee) {
            TransactionDB transDB = new TransactionDB();
            double cap = settings.GetBillingCap();
            string start = DTStart.ToString("yyyy-MM-dd");
            string end = DTEnd.ToString("yyyy-MM-dd");
            if (eventName.CompareTo("Regular Childcare") == 0 || eventName.CompareTo("Infant Childcare") == 0 || eventName.CompareTo("Adolescent Childcare") == 0) {
                object recordFound = transDB.SumRegularCare(start, end, familyID);
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

        public DateTime FindBillingEnd(DateTime DTStart, int billingEnd) {
            DateTime DTEnd;
            int endMonth = DTStart.Month + 1;
            if (endMonth == 13) {
                int endYear = DTStart.Year + 1;
                DTEnd = new DateTime(endYear, 1, billingEnd);
            }
            else {
                DTEnd = new DateTime(DateTime.Now.Year, endMonth, billingEnd);
            }
            return DTEnd;
        }

        public DateTime FindBillingStart(DateTime DTEnd, int billingStart) {
            DateTime DTStart;
            int startMonth = DTEnd.Month - 1;
            if (startMonth == 0) {
                int startYear = DTEnd.Year - 1;
                DTStart = new DateTime(startYear, 12, billingStart);
            }
            else {
                DTStart = new DateTime(DateTime.Now.Year, startMonth, billingStart);
            }
            return DTStart;
        }

    }
}
