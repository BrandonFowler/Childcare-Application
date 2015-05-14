using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChildcareApplication.Properties;
using System.Windows;
using MessageBoxUtils;

namespace GuardianTools {

    class GuardianToolsSettings {

        internal string GetClosingTime(string dayOfWeek) {
            DateTime closingTime;
            string closing = null;
            if (dayOfWeek.CompareTo("Monday") == 0) {
                closingTime = Settings.Default.MonClose;
            } else if (dayOfWeek.CompareTo("Tuesday") == 0) {
                closingTime = Settings.Default.TueClose;
            } else if (dayOfWeek.CompareTo("Wednesday") == 0) {
                closingTime = Settings.Default.WedClose;
            } else if (dayOfWeek.CompareTo("Thursday") == 0) {
                closingTime = Settings.Default.ThuClose;
            } else if (dayOfWeek.CompareTo("Friday") == 0) {
                closingTime = Settings.Default.FriClose;
            } else if (dayOfWeek.CompareTo("Saturday") == 0) {
                closingTime = Settings.Default.SatClose;
            } else {
                closingTime = Settings.Default.SunClose;
            }
            closing = closingTime.ToString("HH:mm:ss");
            if (closing.CompareTo("00:00:00") == 0) {
                return null;
            }
            return closing;
        }

        internal int GetRegularChildCap() {
            int cap;
            if (Int32.TryParse(Settings.Default.RegularMaxAge, out cap)) {
                return cap;
            }
            WPFMessageBox.Show("Error: Unable to retrieve settings data, child age group may be calculated incorrectly.");
            return 8;

        }

        internal int GetInfantCap() {
            int cap;
            if (Int32.TryParse(Settings.Default.InfantMaxAge, out cap)) {
                return cap;
            }
            WPFMessageBox.Show("Error: Unable to retrieve settings data, child age group may be calculated incorrectly.");
            return 1;

        }

        internal int GetBillingEnd() {
            int cap;
            if (Int32.TryParse(Settings.Default.BillingStartDate, out cap)) {
                return cap-1;
            }
            WPFMessageBox.Show("Error: Unable to retrieve billing dates, fee may be recorded incorrectly.");
            return 19;

        }

        internal int GetBillingStart() {
            int cap;
            if (Int32.TryParse(Settings.Default.BillingStartDate, out cap)) {
                return cap;
            }
            WPFMessageBox.Show("Error: Unable to retrieve billing dates, fee may be recorded incorrectly.");
            return 20;

        }

        internal int GetBillingCap() {
            int cap;
            if (Int32.TryParse(Settings.Default.MaxMonthlyFee, out cap)) {
                return cap;
            }
            WPFMessageBox.Show("Error: Unable to retrieve settings information, fee may be recorded incorrectly.");
            return 100;
        }

        internal string CheckAgeGroup(string birthday, string date) {
            DateTime DTBirthday = DateTime.Parse(birthday);
            DateTime DTDate = DateTime.Parse(date);
            TimeSpan difference = DTDate - DTBirthday;
            double infantDays = GetInfantCap() * 365.242;
            double regularDays = GetRegularChildCap() * 365.242;
            if (difference.Days < infantDays) {
                return "Infant";
            }
            else if (difference.Days < regularDays) {
                return "Regular";
            }
            return "Adolescent";
        }

        internal double CheckIfPastClosing(string dayOfWeek, TimeSpan time) {
            string closingTime = GetClosingTime(dayOfWeek);
            if (string.IsNullOrEmpty(closingTime)) {
                return 1;
            }
            TimeSpan TSClosingTime = TimeSpan.Parse(closingTime);
            double hourDifference = time.Hours - TSClosingTime.Hours;
            double minuteDifference = time.Minutes - TSClosingTime.Minutes;
            double hours = hourDifference + (minuteDifference / 60.0);
            if (hours < 0) {
                return 0;
            }
            return hours;
        }
    }
}
