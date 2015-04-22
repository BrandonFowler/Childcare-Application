using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChildcareUnitTests {
    [TestClass]
    public class AddEditChildUnitTests {
        [TestMethod]
        public void TestChildCheckIfNull() {
            AdminTools.AdminEditChildInfo AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.lst_ChildBox.SelectedItem = AddEditChild.lst_ChildBox.Items[0];
            Assert.IsFalse(AddEditChild.CheckIfNull());//Check everything is ok upon initial loading
            AddEditChild.txt_FirstName.Text = "";
            Assert.IsTrue(AddEditChild.CheckIfNull());//Check that empty first name is caught
            AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.lst_ChildBox.SelectedItem = AddEditChild.lst_ChildBox.Items[0];
            AddEditChild.txt_LastName.Text = "";
            Assert.IsTrue(AddEditChild.CheckIfNull());//Check that empty last name is caught
            AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.txt_FirstName.Text = null;
            Assert.IsTrue(AddEditChild.CheckIfNull());//Check that null first name is caught
            AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.lst_ChildBox.SelectedItem = AddEditChild.lst_ChildBox.Items[0];
            AddEditChild.txt_LastName.Text = null;
            Assert.IsTrue(AddEditChild.CheckIfNull());//Check that null last name is caught
        }

        [TestMethod]
        public void TestChildRegexValidation() {
            AdminTools.AdminEditChildInfo AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.lst_ChildBox.SelectedItem = AddEditChild.lst_ChildBox.Items[0];
            Assert.IsTrue(AddEditChild.RegexValidation());//Check everything is ok upon initial loading
            AddEditChild.txt_FirstName.Text = "2131234JunkName";
            Assert.IsFalse(AddEditChild.RegexValidation());//Check bad first name is caught
            AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.lst_ChildBox.SelectedItem = AddEditChild.lst_ChildBox.Items[0];
            AddEditChild.txt_LastName.Text = "2131234JunkName";
            Assert.IsFalse(AddEditChild.RegexValidation());//Check bad last name is caught
            AddEditChild = new AdminTools.AdminEditChildInfo("123450");
            AddEditChild.lst_ChildBox.SelectedItem = AddEditChild.lst_ChildBox.Items[0];
            AddEditChild.txt_FilePath.Text = "2131234JunkPath";
            Assert.IsFalse(AddEditChild.RegexValidation());//Check bad image path is caught
        }
    }
}
