using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildcareApplication.AdminTools {
    class SettingsValidation {
        
        internal static bool ValidBillingDate(string inputDate) {
            int result;
            if (Int32.TryParse(inputDate, out result)) {
                if (result < 1 || result > 29) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }
        
        internal static bool PositiveInteger(string inputInt) {
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

        internal static bool ValidAge(string lowerAge, string higherAge) {
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
    }
}
