using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication.DatabaseController;

namespace ChildcareUnitTests {
    [TestClass]
    public class AdminLoginUnitTests {

        [TestMethod]
        public void TestValidateAdminLogin() {
            DatabaseController.LoginDB db = new DatabaseController.LoginDB();
            Assert.IsTrue(db.validateAdminLogin("a", "0CC175B9C0F1B6A831C399E2697726"));
            Assert.IsFalse(db.validateAdminLogin("a", "JunkPIN"));
            Assert.IsFalse(db.validateAdminLogin("JunkID", "0CC175B9C0F1B6A831C399E2697726"));
            Assert.IsFalse(db.validateAdminLogin("JunkID", "JunkPIN"));
        }

        [TestMethod]
        public void TestValidateAdminAccessLevel() {
            DatabaseController.LoginDB db = new DatabaseController.LoginDB();
            Assert.AreEqual(db.GetAccessLevel("a"), 1);
            Assert.AreEqual(db.GetAccessLevel("JunkID"), 0);
        }
    }
}
