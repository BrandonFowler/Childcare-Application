using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication;
using DatabaseController;

namespace ChildcareUnitTests {
    [TestClass]
    public class AddEditGuardianUnitTests {
        [TestMethod]
        public void TestGuardianCheckIfNull() {
            AdminTools.AdminEditParentInfo addEditParent = new AdminTools.AdminEditParentInfo("123450");
            Assert.IsFalse(addEditParent.CheckIfNull());
            addEditParent.txt_FirstName.Text = "";
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_LastName.Text = "";
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Email.Text = "";
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Address.Text = "";
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_City.Text = "";
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Zip.Text = "";
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_FirstName.Text = null;
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_LastName.Text = null;
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Email.Text = null;
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Address.Text = null;
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_City.Text = null;
            Assert.IsTrue(addEditParent.CheckIfNull());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Zip.Text = null;
            Assert.IsTrue(addEditParent.CheckIfNull());
        }

        [TestMethod]
        public void TestGuardianRegexValidation() {
            AdminTools.AdminEditParentInfo addEditParent = new AdminTools.AdminEditParentInfo("123450");
            Assert.IsTrue(addEditParent.RegexValidation());
            addEditParent.txt_PhoneNumber.Text = "not a phone number";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_FirstName.Text = "2131234JunkName";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_LastName.Text = "2131234JunkName";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Email.Text = "JunkEmail";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Address.Text = "2131234JunkAddress";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_City.Text = "21+12-4JunkCity^";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_Zip.Text = "2131234JunkZip";
            Assert.IsFalse(addEditParent.RegexValidation());
            addEditParent = new AdminTools.AdminEditParentInfo("123450");
            addEditParent.txt_FilePath.Text = "2131234JunkPath";
            Assert.IsFalse(addEditParent.RegexValidation());
        }

        [TestMethod]
        public void TestEditParentClearFields() {
            AdminTools.AdminEditParentInfo addEditChild = new AdminTools.AdminEditParentInfo("123450");
            addEditChild.ClearFields();
            Assert.AreEqual(addEditChild.txt_FirstName.Text, "");
            Assert.AreEqual(addEditChild.txt_LastName.Text, "");
            Assert.AreEqual(addEditChild.txt_Address.Text, "");
            Assert.AreEqual(addEditChild.txt_Address2.Text, "");
            Assert.AreEqual(addEditChild.txt_FilePath.Text, "");
            Assert.AreEqual(addEditChild.txt_City.Text, "");
            Assert.AreEqual(addEditChild.txt_IDNumber.Text, "");
            Assert.AreEqual(addEditChild.txt_PhoneNumber.Text, "");
            Assert.AreEqual(addEditChild.txt_Zip.Text, "");
            Assert.AreEqual(addEditChild.txt_Email.Text, "");
        }

        [TestMethod]
        public void TestNewParentCheckIfNull() {
            AdminTools.NewParentLogin newParent = new AdminTools.NewParentLogin();
            Assert.IsTrue(newParent.CheckIfNull());
            newParent.txt_ParentID1.Text = "123450";
            Assert.IsTrue(newParent.CheckIfNull());
            newParent.txt_ParentID2.Text = "123451";
            Assert.IsTrue(newParent.CheckIfNull());
            newParent.psw_ParentPIN1.Password = "1234";
            Assert.IsTrue(newParent.CheckIfNull());
            newParent.psw_ParentPIN2.Password = "1234";
            Assert.IsFalse(newParent.CheckIfNull());
        }

        [TestMethod]
        public void TestNewParentCheckIfSame() {
            AdminTools.NewParentLogin newParent = new AdminTools.NewParentLogin();
            Assert.IsTrue(newParent.CheckIfSame("", ""));
            Assert.IsTrue(newParent.CheckIfSame("123450", "123450"));
            Assert.IsFalse(newParent.CheckIfSame("123450", "123451"));
            Assert.IsFalse(newParent.CheckIfSame("123450", "0123450"));
        }

        [TestMethod]
        public void TestGetParentName() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.GetParentName("123450"));
            Assert.AreEqual(db.GetParentName("123450"), "Bilbo Baggins");
        }

        [TestMethod]
        public void TestGetAddress1() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.GetAddress1("123450"));
            Assert.AreEqual(db.GetAddress1("123450"), "123 Bag End");
        }

        [TestMethod]
        public void TestGetAddress2() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.GetAddress2("123450"));
            Assert.AreEqual(db.GetAddress2("123450"), "Hole A");
        }

        [TestMethod]
        public void TestGetAddress3() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.GetAddress3("123450"));
            Assert.AreEqual(db.GetAddress3("123450"), "Shiree, ME 99999");
        }

        [TestMethod]
        public void TestGetPhoneNumber() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.GetPhoneNumber("123450"));
            Assert.AreEqual(db.GetPhoneNumber("123450"), "555-123-4567");
        }

        [TestMethod]
        public void TestGetPhotoPath() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.GetPhotoPath("123450"));
            Assert.AreEqual(db.GetPhotoPath("123450"), "../../Pictures/Bilbo_Baggins.jpg");
        }

        [TestMethod]
        public void TestGuardianIDExists() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsTrue(db.GuardianIDExists("123450"));
            Assert.IsFalse(db.GuardianIDExists("9999999999"));
        }

        [TestMethod]
        public void TestCheckIfFamilyExists() {
            DatabaseController.GuardianInfoDB db = new DatabaseController.GuardianInfoDB();
            Assert.IsNotNull(db.CheckIfFamilyExists("12345"));
            Assert.AreEqual(db.CheckIfFamilyExists("12345"), "12345");
        }
    }
}
