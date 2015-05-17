using System.Text.RegularExpressions;
using System.Windows;
using MessageBoxUtils;

namespace ChildcareApplication.AdminTools {
    class RegExpressions {

        public static bool RegexPhoneNumber(string data) {
            /*Phone number: 
             * (509)5555555
             * (509)555-5555
             * 5095555555
             * 509-555-5555
             */
            Regex regex = new Regex(@"(^[(]\d{3}[)]\d{7}$)|(^[(]\d{3}[)]\d{3}-\d{4}$)|(^\d{3}-\d{3}-\d{4}$)|(^\d{10}$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX:(509)555-5555");
            return false;
        }//end regexPhoneNumber

        public static bool RegexEmail(string data) {
            /*Email:
             * e.mail@ewu.edu
             * email@ewu.edu
             * email@somthing.net
             * e.mail@this.this.that
             */
            Regex regex = new Regex(@"(^(\w)+(\.(\w+))*@((\w+)\.(\w+)(\.(\w+))*)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: e.mail@ewu.edu");
            return false;
        }//end regexEmail

        public static bool RegexName(string data) {
            /*Name:
             * One
             * Name
             * Only
             */
            Regex regex = new Regex(@"(^([a-zA-Z])+$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: Brian");
            return false;
        }//end regexName

        public static bool RegexDate(string data) {
            /*Date:
             * 02 15 1991
             * 02-15-1991
             */

            Regex regex = new Regex(@"(^(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])-(19|20)\d\d$)|(^(0[1-9]|1[012])\s(0[1-9]|[12][0-9]|3[01])\s(19|20)\d\d$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: 02-15-1991");
            return false;
        }//end regexDate

        public static bool RegexAddress(string data) {
            /*Address: 
             * 123 N Street St.
             * 123 N Street Street
             * 123 Road Rd
             * 123 Road Road
             */
            Regex regex = new Regex(@"(^(\d{3,6}\s)([a-zA-Z]+\s?)+((RD|rd|ST|st|BLVD|blvd|AVE|ave|CT|ct|LN|ln|WAY|way|PL|pl|ALY|aly|Road|Street|Boulevard|Avenue|Court|Lane|Place|Alley)?\.?)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: 123 Road Rd.");
            return false;

        }//end regexAddress

        public static bool RegexCity(string data) {
            /*City:
             * Spokane
             * San Antonio
             */
            Regex regex = new Regex(@"(^([\w\s]+)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: Spokane");
            return false;
        }//end regexCityState

        public static bool RegexZIP(string data) {
            /*ZIP:
             * 12345
             */
            Regex regex = new Regex(@"(^(\d{5})$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. Ex: 12345");
            return false;
        }//end regexCityState

        public static bool RegexID(string data) {
            /*ID:
             * 123456
             */
            Regex regex = new Regex(@"(^(\d{6})$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. Ex: 123456");
            return false;
        }//end regexCityState

        public static bool RegexPIN(string data) {
            /*PIN:
             * 1234
             */
            Regex regex = new Regex(@"(^(\d{4})$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. Ex: 1234");
            return false;
        }//end regexCityState

        public static bool RegexFilePath(string data) {
            /*FilePath
             * anythingYouWant.jpg
             * kh_2323-^&6kh_kh.jpg
             */
            Regex regex = new Regex(@"(^(.+)(\.jpg)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: file_name.jpg");
            return false;
        }//end regexCityState

        public static bool RegexValidateEventName(string data) {
            /*Event Nmae:
             * Some Event Name
             * Event
             */

            Regex regex = new Regex(@"(^([a-zA-Z0-9]+\s?)+$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            //WPFMessageBox.Show("The word " + data + " is not valid. Please re-enter. EX: Some Event Name");
            return false;


        }
    }
}
