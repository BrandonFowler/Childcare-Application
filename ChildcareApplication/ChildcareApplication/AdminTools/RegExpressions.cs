using System.Text.RegularExpressions;
using System.Windows;

namespace ChildcareApplication.AdminTools {
    class RegExpressions {

        public static bool RegexPhoneNumber(string data) {
            Regex regex = new Regex(@"(^[(]\d{3}[)]\d{7}$)|(^[(]\d{3}[)]\d{3}-\d{4}$)|(^\d{3}-\d{3}-\d{4}$)|(^\d{10}$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;
        }//end regexPhoneNumber

        public static bool RegexEmail(string data) {
            Regex regex = new Regex(@"(^(\w)+(\.(\w+))*@((\w+)\.(\w+)(\.(\w+))*)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;
        }//end regexEmail

        public static bool RegexName(string data) {

            Regex regex = new Regex(@"(^([a-zA-Z])+$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;
        }//end regexName

        public static bool RegexDate(string data) {

            Regex regex = new Regex(@"(^(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])-(19|20)\d\d$)|(^(0[1-9]|1[012])\s(0[1-9]|[12][0-9]|3[01])\s(19|20)\d\d$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;
        }//end regexDate

        public static bool RegexAddress(string data) {
            Regex regex = new Regex(@"(^(\d{3,6}\s)([a-zA-Z]+\s?)+((RD|rd|ST|st|BLVD|blvd|AVE|ave|CT|ct|LN|ln|WAY|way|PL|pl|ALY|aly|Road|Street|Boulevard|Avenue|Court|Lane|Place|Alley)?\.?)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;

        }//end regexAddress

        public static bool RegexCity(string data) {
            Regex regex = new Regex(@"(^([\w\s]+)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;
        }//end regexCityState

        public static bool RegexZIP(string data) {
            Regex regex = new Regex(@"(^(\d{5})$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter. Ex: 12345");
            return false;
        }//end regexCityState

        public static bool RegexID(string data) {
            Regex regex = new Regex(@"(^(\d{6})$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter. Ex: 123456");
            return false;
        }//end regexCityState

        public static bool RegexPIN(string data) {
            Regex regex = new Regex(@"(^(\d{4})$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter. Ex: 1234");
            return false;
        }//end regexCityState

        public static bool RegexFilePath(string data) {
            Regex regex = new Regex(@"(^(.+)(\.jpg)$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            MessageBox.Show("The word " + data + " is not valid. Please re-enter.");
            return false;
        }//end regexCityState

        public static bool RegexValidateEventName(string data) {

            Regex regex = new Regex(@"(^([a-zA-Z]+\s?)+$)");
            Match match = regex.Match(data);

            if (match.Success)
                return true;

            return false;


        }
    }
}
