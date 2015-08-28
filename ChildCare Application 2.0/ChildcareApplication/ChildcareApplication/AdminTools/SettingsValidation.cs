﻿using System;

namespace ChildcareApplication.AdminTools {
    class SettingsValidation {

        public static bool ValidBillingDate(string inputDate) {
            int result;
            if (Int32.TryParse(inputDate, out result)) {
                if (result < 2 || result > 28) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        public static bool PositiveInteger(string inputInt) {
            int result;
            if (Int32.TryParse(inputInt, out result)) {
                if (result < 1) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        public static bool ValidAge(string lowerAge, string higherAge) {
            int lowerResult;
            int higherResult;
            if (Int32.TryParse(lowerAge, out lowerResult) && Int32.TryParse(higherAge, out higherResult)) {
                if (lowerResult < 0 || higherResult < 0 || lowerResult > higherResult) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        public static bool ValidHours(string openingInput, string closingInput) {
            DateTime openingTime;
            DateTime closingTime;
            if (DateTime.TryParse(openingInput, out openingTime) && DateTime.TryParse(closingInput, out closingTime)) {
                if (openingTime > closingTime) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }
    }
}
