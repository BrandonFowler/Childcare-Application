using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication;
using DatabaseController;

namespace ChildcareUnitTests {
    [TestClass]
    public class AddEditGuardianUnitTests {
        [TestMethod]
        public void TestCheckIfNull() {
            AdminTools.AdminEditParentInfo AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            Assert.IsFalse(AddEditParent.CheckIfNull());
            AddEditParent.txt_FirstName.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());
        }

        [TestMethod]
        public void TestRegexValidation() {
            AdminTools.AdminEditParentInfo AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            Assert.IsTrue(AddEditParent.RegexValidation());
            AddEditParent.txt_PhoneNumber.Text = "not a phone number";
            Assert.IsFalse(AddEditParent.RegexValidation());
        }
    }
}
