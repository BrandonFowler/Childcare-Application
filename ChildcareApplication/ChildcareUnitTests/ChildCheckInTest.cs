using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication.Properties;
namespace ChildcareUnitTests {
    [TestClass]
    public class ChildCheckInTest {
        [TestMethod]
        public void TestGetClosingTime() {
            GuardianTools.GuardianToolsSettings guardianTools = new GuardianTools.GuardianToolsSettings(); 
            DateTime closingTime = Settings.Default.SunClose;

            string sun = closingTime.ToString("HH:mm:ss");
            Assert.AreEqual(guardianTools.GetClosingTime(sun), null);

            string mon = "Monday";
            Assert.AreEqual(guardianTools.GetClosingTime(mon), "19:00:00");

            string tue = "Tuesday";
            Assert.AreEqual(guardianTools.GetClosingTime(tue), "19:00:00");

            string wed = "Wednesday";
            Assert.AreEqual(guardianTools.GetClosingTime(wed), "19:00:00");

            string thur = "Thursday";
            Assert.AreEqual(guardianTools.GetClosingTime(thur), "19:00:00");

            string fri = "Friday";
            Assert.AreEqual(guardianTools.GetClosingTime(fri), "20:00:00");

            string sat = "Saturday";
            Assert.AreEqual(guardianTools.GetClosingTime(sat), "12:30:00");
        }

        [TestMethod]
        public void TestGetRegularChildCap() {
            GuardianTools.GuardianToolsSettings guardianTools = new GuardianTools.GuardianToolsSettings();
            Assert.AreEqual(guardianTools.GetRegularChildCap(), Int32.Parse(Settings.Default.RegularMaxAge));
            Assert.AreNotEqual(guardianTools.GetRegularChildCap(), Int32.Parse(Settings.Default.RegularMaxAge) + 1);
        }
        
        [TestMethod]
        public void TestGetInfantCap() {
            GuardianTools.GuardianToolsSettings guardianTools = new GuardianTools.GuardianToolsSettings();
            Assert.AreEqual(guardianTools.GetInfantCap(), Int32.Parse(Settings.Default.InfantMaxAge));
            Assert.AreNotEqual(guardianTools.GetInfantCap(), Int32.Parse(Settings.Default.InfantMaxAge) + 1);
        }

        [TestMethod]
        public void TestGetBillingEnd() { 
            GuardianTools.GuardianToolsSettings guardianTools = new GuardianTools.GuardianToolsSettings();
            Assert.AreEqual(guardianTools.GetBillingEnd(), Int32.Parse(Settings.Default.BillingStartDate) - 1);
            Assert.AreNotEqual(guardianTools.GetBillingEnd(), Settings.Default.BillingStartDate);
        }

        [TestMethod]
        public void TestGetBillingStart() {
            GuardianTools.GuardianToolsSettings guardianTools = new GuardianTools.GuardianToolsSettings();
            Assert.AreEqual(guardianTools.GetBillingStart(), Int32.Parse(Settings.Default.BillingStartDate));
            Assert.AreNotEqual(guardianTools.GetBillingStart(), Int32.Parse(Settings.Default.BillingStartDate) - 1);
        }

        [TestMethod]
        public void TestCheckAgeGroup() {
            GuardianTools.GuardianToolsSettings guardianTools = new GuardianTools.GuardianToolsSettings();
            string birthday, date;
            birthday = "02/15/1991";
            date = "04/22/2015";
            Assert.AreEqual(guardianTools.CheckAgeGroup(birthday, date), "Adolescent");

            birthday = "01/01/2000";
            date = "02/02/2000";
            Assert.AreEqual(guardianTools.CheckAgeGroup(birthday, date), "Infant");

            birthday = "01/01/2000";
            date = "02/02/2001";
            Assert.AreEqual(guardianTools.CheckAgeGroup(birthday, date), "Regular");
        }
    }
}
