using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace ChildcareUnitTests {
    [TestClass]
    public class AddEditChildUnitTests {

        [TestMethod]
        public void TestEditChildCheckIfNull() {
            AdminTools.AdminEditChildInfo addEditChild = new AdminTools.AdminEditChildInfo("123450");
            addEditChild.lst_ChildBox.SelectedItem = addEditChild.lst_ChildBox.Items[0];
            Assert.IsFalse(addEditChild.CheckIfNull());
            addEditChild.txt_FirstName.Text = "";
            Assert.IsTrue(addEditChild.CheckIfNull());
            addEditChild = new AdminTools.AdminEditChildInfo("123450");
            addEditChild.lst_ChildBox.SelectedItem = addEditChild.lst_ChildBox.Items[0];
            addEditChild.txt_LastName.Text = "";
            Assert.IsTrue(addEditChild.CheckIfNull());
            addEditChild = new AdminTools.AdminEditChildInfo("123450");
            addEditChild.txt_FirstName.Text = null;
            Assert.IsTrue(addEditChild.CheckIfNull());
            addEditChild = new AdminTools.AdminEditChildInfo("123450");
            addEditChild.lst_ChildBox.SelectedItem = addEditChild.lst_ChildBox.Items[0];
            addEditChild.txt_LastName.Text = null;
            Assert.IsTrue(addEditChild.CheckIfNull());
        }

        [TestMethod]
        public void TestEditChildGetFamilyID() {
            AdminTools.AdminEditChildInfo addEditChild = new AdminTools.AdminEditChildInfo("123450");
            Assert.AreEqual(addEditChild.GetFamilyID("123450"), "12345");
            Assert.AreNotEqual(addEditChild.GetFamilyID("123450"), "123450");
            Assert.AreNotEqual(addEditChild.GetFamilyID("123450"), "23450");
            Assert.AreNotEqual(addEditChild.GetFamilyID("123450"), "");
        }

        [TestMethod]
        public void TestEditChildGetConnectedChildren() {
            AdminTools.AdminEditChildInfo addEditChild = new AdminTools.AdminEditChildInfo("123450");
            ArrayList list = new ArrayList();
            list = addEditChild.GetConnectedChildren(list);
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 3);
            Assert.AreNotEqual(list.Count, 0);
        }

        [TestMethod]
        public void TestEditChildClearFields() {
            AdminTools.AdminEditChildInfo addEditChild = new AdminTools.AdminEditChildInfo("123450");
            addEditChild.lst_ChildBox.SelectedItem = addEditChild.lst_ChildBox.Items[0];
            addEditChild.ClearFields();
            Assert.AreEqual(addEditChild.txt_FirstName.Text, "");
            Assert.AreEqual(addEditChild.txt_LastName.Text, "");
            Assert.AreEqual(addEditChild.txt_Allergies.Text, "");
            Assert.AreEqual(addEditChild.txt_Medical.Text, "");
            Assert.AreEqual(addEditChild.txt_FilePath.Text, "");
            Assert.AreEqual(addEditChild.dte_Birthday.Text, "1/1/2005");
        }

        [TestMethod]
        public void TestDeLinkChildCheckIfNull() {
            AdminTools.Link_DeLinkChild DeLink = new AdminTools.Link_DeLinkChild(0, "000001");
            Assert.IsTrue(DeLink.CheckIfNull());
            DeLink.txt_GuardianID.Text = "123450";
            Assert.IsTrue(DeLink.CheckIfNull());
            DeLink.txt_GuardianID2.Text = "123451";
            Assert.IsFalse(DeLink.CheckIfNull());
        }

        [TestMethod]
        public void TestDeLinkMakeFamilyID() {
            AdminTools.Link_DeLinkChild DeLink = new AdminTools.Link_DeLinkChild(0, "000001");
            Assert.AreEqual(DeLink.MakeFamilyID("123450"), "12345");
            Assert.AreNotEqual(DeLink.MakeFamilyID("123450"), "123450");
            Assert.AreNotEqual(DeLink.MakeFamilyID("123450"), "23450");
            Assert.AreNotEqual(DeLink.MakeFamilyID("123450"), "");
        }

        [TestMethod]
        public void TestDeLinkCheckIfSame() {
            AdminTools.Link_DeLinkChild DeLink = new AdminTools.Link_DeLinkChild(0, "000001");
            Assert.IsTrue(DeLink.CheckIfSame("", ""));
            Assert.IsTrue(DeLink.CheckIfSame("123450", "123450"));
            Assert.IsFalse(DeLink.CheckIfSame("123450", "123451"));
            Assert.IsFalse(DeLink.CheckIfSame("123450", "0123450"));
        }

        [TestMethod]
        public void TestDeLinkCheckIfNumbers() {
            AdminTools.Link_DeLinkChild DeLink = new AdminTools.Link_DeLinkChild(0, "000001");
            Assert.IsTrue(DeLink.CheckIfNumbers("123450", "123451"));
            Assert.IsTrue(DeLink.CheckIfNumbers("1", "0"));
            Assert.IsFalse(DeLink.CheckIfNumbers("123450Junk", "123451"));
            Assert.IsFalse(DeLink.CheckIfNumbers("123450", "123451Junk"));
            Assert.IsFalse(DeLink.CheckIfNumbers("Junk", "Junk"));
            Assert.IsFalse(DeLink.CheckIfNumbers("", ""));
        }

        [TestMethod]
        public void TestLinkGetFamilyID() {
            ChildcareApplication.AdminTools.LinkExistingChild Link = 
                new ChildcareApplication.AdminTools.LinkExistingChild("123450", new ArrayList());
            Assert.AreEqual(Link.GetFamilyID("123450"), "12345");
            Assert.AreNotEqual(Link.GetFamilyID("123450"), "123450");
            Assert.AreNotEqual(Link.GetFamilyID("123450"), "23450");
            Assert.AreNotEqual(Link.GetFamilyID("123450"), "");
        }

        [TestMethod]
        public void TestGetMaxChildID() {
            DatabaseController.ChildInfoDatabase db = new DatabaseController.ChildInfoDatabase();
            Assert.AreNotEqual(db.GetMaxChildID(), 0);
            Assert.IsNotNull(db.GetMaxChildID());
        }

        [TestMethod]
        public void TestGetMaxConnectionID() {
            DatabaseController.ChildInfoDatabase db = new DatabaseController.ChildInfoDatabase();
            Assert.AreNotEqual(db.GetMaxConnectionID(), 0);
            Assert.IsNotNull(db.GetMaxConnectionID());
        }

        [TestMethod]
        public void TestFindChildren() {
            DatabaseController.ChildInfoDatabase db = new DatabaseController.ChildInfoDatabase();
            string[,] childList = db.FindChildren("123450");
            Assert.IsNotNull(childList);
            Assert.AreEqual(childList.GetLength(0), 3);
        }
    }
}
