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
        public void TestGetCharge() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            Assert.AreEqual(tCharge.getCharge(5.0, "Regular Childcare", 3.0), 15.0);
            Assert.AreNotEqual(tCharge.getCharge(5.0, "Regular Childcare", 3.0), 14.0);
        }

        [TestMethod]//This test does not pass. I may need help. 
        public void TestCalculateLateFee() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            Assert.AreEqual(tCharge.CalculateLateFee(), 30.0);
            Assert.AreNotEqual(tCharge.CalculateLateFee(), 60.0);
        }

        [TestMethod]
        public void TestCheckIfHourly() {
            GuardianTools.TransactionCharge tCharge = new GuardianTools.TransactionCharge("123450", "000001");
            Assert.IsTrue(tCharge.CheckIfHourly("Regular Childcare"));
            Assert.IsFalse(tCharge.CheckIfHourly("Camp"));
        }


    }
}
