using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication.DatabaseController;

namespace ChildcareUnitTests {
    [TestClass]
    public class AdminLoginUnitTests {

        [TestMethod]
        public void testValidateAdminLogin() {
            DatabaseController.LoginDB db = new DatabaseController.LoginDB();
            Assert.IsTrue(db.validateAdminLogin("a", "0CC175B9C0F1B6A831C399E2697726"));//Check validation works with good data
            Assert.IsFalse(db.validateAdminLogin("a", "JunkPIN"));//Check bad PIN is caught
            Assert.IsFalse(db.validateAdminLogin("JunkID", "0CC175B9C0F1B6A831C399E2697726"));//Check bad ID is caught
            Assert.IsFalse(db.validateAdminLogin("JunkID", "JunkPIN"));//Check bad ID and bad PIN are caught
        }

        [TestMethod]
        public void testValidateAdminAccessLevel() {
            DatabaseController.LoginDB db = new DatabaseController.LoginDB();
            Assert.AreEqual(db.GetAccessLevel("a"), 1);//Check that correct access level is returned
            Assert.AreEqual(db.GetAccessLevel("JunkID"), -1);//Check that bad admin ID is caught
        }
    }
}
