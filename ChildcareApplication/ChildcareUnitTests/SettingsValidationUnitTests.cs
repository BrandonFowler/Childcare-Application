using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChildcareApplication.AdminTools;

namespace ChildcareUnitTests {
    [TestClass]
    public class SettingsValidationUnitTests {
        [TestMethod]
        public void TestValidBillingDate() {
            Assert.IsFalse(SettingsValidation.ValidBillingDate("-1"));
            Assert.IsFalse(SettingsValidation.ValidBillingDate(""));
            Assert.IsFalse(SettingsValidation.ValidBillingDate(null));
            Assert.IsFalse(SettingsValidation.ValidBillingDate("a"));
            Assert.IsFalse(SettingsValidation.ValidBillingDate("30"));
            Assert.IsFalse(SettingsValidation.ValidBillingDate("0"));
            Assert.IsTrue(SettingsValidation.ValidBillingDate("1"));
            Assert.IsTrue(SettingsValidation.ValidBillingDate("28"));
            Assert.IsTrue(SettingsValidation.ValidBillingDate("10"));
        }

        [TestMethod]
        public void TestPositiveInteger() {
            Assert.IsFalse(SettingsValidation.PositiveInteger("-1"));
            Assert.IsFalse(SettingsValidation.PositiveInteger(""));
            Assert.IsFalse(SettingsValidation.PositiveInteger(null));
            Assert.IsFalse(SettingsValidation.PositiveInteger("a"));
            Assert.IsTrue(SettingsValidation.PositiveInteger("30"));
            Assert.IsFalse(SettingsValidation.PositiveInteger("0"));
            Assert.IsTrue(SettingsValidation.PositiveInteger("1"));
            Assert.IsTrue(SettingsValidation.PositiveInteger("28"));
            Assert.IsTrue(SettingsValidation.PositiveInteger("10"));
        }

        [TestMethod]
        public void TestValidAge() {
            Assert.IsFalse(SettingsValidation.ValidAge("", ""));
            Assert.IsFalse(SettingsValidation.ValidAge("-1", "-1"));
            Assert.IsFalse(SettingsValidation.ValidAge("-10", "-2"));
            Assert.IsFalse(SettingsValidation.ValidAge("-2", "-10"));
            Assert.IsFalse(SettingsValidation.ValidAge("0", "-1"));
            Assert.IsFalse(SettingsValidation.ValidAge("4", "2"));
            Assert.IsFalse(SettingsValidation.ValidAge("-1", "0"));
            Assert.IsTrue(SettingsValidation.ValidAge("0", "0"));
            Assert.IsFalse(SettingsValidation.ValidAge(null, null));
            Assert.IsTrue(SettingsValidation.ValidAge("0", "4"));
            Assert.IsTrue(SettingsValidation.ValidAge("1", "2"));
        }

        [TestMethod]
        public void TestValidHours() {
            Assert.IsFalse(SettingsValidation.ValidHours("", ""));
            Assert.IsFalse(SettingsValidation.ValidHours("a", ""));
            Assert.IsFalse(SettingsValidation.ValidHours("", "a"));
            Assert.IsFalse(SettingsValidation.ValidHours("a", "a"));
            Assert.IsFalse(SettingsValidation.ValidHours("2", "2"));
            Assert.IsFalse(SettingsValidation.ValidHours("2", "3"));
            Assert.IsFalse(SettingsValidation.ValidHours("0", "2"));
            Assert.IsFalse(SettingsValidation.ValidHours("2", "0"));
            Assert.IsFalse(SettingsValidation.ValidHours("-2", "1"));
            Assert.IsFalse(SettingsValidation.ValidHours("-12:00:00", "-14:00:00"));
            Assert.IsFalse(SettingsValidation.ValidHours("-14:00:00", "-12:00:00"));
            Assert.IsFalse(SettingsValidation.ValidHours("2:00:00", "1:00:00"));
            Assert.IsFalse(SettingsValidation.ValidHours("23:00:00", "25:00:00"));
            Assert.IsFalse(SettingsValidation.ValidHours("25:00:00", "23:00:00"));
            Assert.IsFalse(SettingsValidation.ValidHours(null, null));
            Assert.IsTrue(SettingsValidation.ValidHours("00:00:00", "00:00:00"));
            Assert.IsFalse(SettingsValidation.ValidHours("20:00:00", "24:00:00"));
            Assert.IsTrue(SettingsValidation.ValidHours("2:00:00", "5:00:00"));
            Assert.IsTrue(SettingsValidation.ValidHours("00:00:00", "2:00:00"));
            Assert.IsTrue(SettingsValidation.ValidHours("12:00:00", "23:59:59"));
        }
    }
}
