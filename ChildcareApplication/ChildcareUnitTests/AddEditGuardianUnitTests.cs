using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication;
using DatabaseController;

namespace ChildcareUnitTests {
    [TestClass]
    public class AddEditGuardianUnitTests {
        [TestMethod]
        public void TestGuardianCheckIfNull() {
            AdminTools.AdminEditParentInfo AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            Assert.IsFalse(AddEditParent.CheckIfNull());//Check everything is ok upon initial loading
            AddEditParent.txt_FirstName.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that empty first name is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_LastName.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that empty last name is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Email.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that empty email is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Address.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that empty address is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_City.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that empty city is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Zip.Text = "";
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that empty zip is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_FirstName.Text = null;
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that null first name is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_LastName.Text = null;
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that null last name is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Email.Text = null;
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that null email is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Address.Text = null;
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that null address is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_City.Text = null;
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that null city is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Zip.Text = null;
            Assert.IsTrue(AddEditParent.CheckIfNull());//Check that null zip is caught
        }

        [TestMethod]
        public void TestGuardianRegexValidation() {
            AdminTools.AdminEditParentInfo AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            Assert.IsTrue(AddEditParent.RegexValidation());//Check everything is ok upon initial loading
            AddEditParent.txt_PhoneNumber.Text = "not a phone number";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad phone number is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_FirstName.Text = "2131234JunkName";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad first name is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_LastName.Text = "2131234JunkName";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad last name is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Email.Text = "JunkEmail";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad email is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Address.Text = "2131234JunkAddress";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad address is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_City.Text = "21+12-4JunkCity^";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad city is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_Zip.Text = "2131234JunkZip";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad zip is caught
            AddEditParent = new AdminTools.AdminEditParentInfo("123450");
            AddEditParent.txt_FilePath.Text = "2131234JunkPath";
            Assert.IsFalse(AddEditParent.RegexValidation());//Check bad image path is caught
        }
    }
}
