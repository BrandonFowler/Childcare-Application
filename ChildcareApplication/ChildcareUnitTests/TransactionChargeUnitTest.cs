using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChildcareUnitTests {
    [TestClass]
    public class TransactionChargeUnitTest {

        [TestMethod]
        public void TestPrepareTransaction() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001"); 
            Assert.IsFalse(tCharge.PrepareTransaction("123453", "123450")); 
        }

        [TestMethod]
        public void TestCalculateTransaction() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            tCharge.setLateTime(0.0);
            Assert.AreEqual(tCharge.CalculateTransaction("12:00:00", "13:00:00", "Regular Childcare", 5.0), 5.0);
        }

        [TestMethod]
        public void TestGetCharge() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            Assert.AreEqual(tCharge.getCharge(5.0, "Regular Childcare", 3.0), 15.0);
            Assert.AreNotEqual(tCharge.getCharge(5.0, "Regular Childcare", 3.0), 14.0);
        }

        [TestMethod]
        public void TestCalculateLateFee() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            tCharge.setLateTime(1.0);
            Assert.AreEqual(tCharge.CalculateLateFee(DateTime.Now.ToString("yyyy-MM-dd")), 30.0);
            Assert.AreNotEqual(tCharge.CalculateLateFee(DateTime.Now.ToString("yyyy-MM-dd")), 60.0);
        }

        [TestMethod]
        public void TestCheckIfHourly() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            Assert.IsTrue(tCharge.CheckIfHourly("Regular Childcare"));
            Assert.IsFalse(tCharge.CheckIfHourly("Camp"));
        }

        [TestMethod]
        public void TestFindEventFee() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            Assert.AreEqual(tCharge.FindEventFee("123450", "Regular Childcare"), 5.0);
            Assert.AreEqual(tCharge.FindEventFee("123450", "Camp"), 36.0);
        }

        [TestMethod]
        public void TestFindBillingEnd() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            DateTime DTstart = new DateTime(2015, 02, 20);
            DateTime DTend = new DateTime(2015, 03, 19);
            Assert.AreEqual(tCharge.FindBillingEnd(DTstart, 19), DTend);
        }

        [TestMethod]
        public void TestFindBillingStart() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            DateTime DTstart = new DateTime(2015, 04, 20);
            DateTime DTend = new DateTime(2015, 03, 19);
            Assert.AreEqual(tCharge.FindBillingEnd(DTend, 20), DTstart);
        }

    }
}
