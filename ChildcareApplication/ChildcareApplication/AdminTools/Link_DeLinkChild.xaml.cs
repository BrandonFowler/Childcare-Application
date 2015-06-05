﻿using ChildcareApplication.AdminTools;
using DatabaseController;
using MessageBoxUtils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminTools {

    public partial class Link_DeLinkChild : Window {
        private ChildInfoDatabase db;
        int linked;
        string childID;

        public Link_DeLinkChild(int link, string cID) {
            linked = link;
            childID = cID;
            InitializeComponent();
            this.db = new ChildInfoDatabase();

            this.MouseDown += WindowMouseDown;
            txt_GuardianID.Focus();
        }

        private void btn_Enter_Click(object sender, RoutedEventArgs e) {
            ConnectionsDB conDB = new ConnectionsDB();
            GuardianInfoDB gdb = new GuardianInfoDB();
            string fID = "", pID = "";
            bool formNotComplete = CheckIfNull();
            if (!formNotComplete)//form is completed
             {
                bool sameID = CheckIfSame(txt_GuardianID.Text, txt_GuardianID2.Text);
                bool regexID = RegExpressions.RegexID(txt_GuardianID.Text);
                if (sameID && regexID)//both IDand PIN are the same vlues
                 {
                    pID = txt_GuardianID.Text;

                    MakeFamilyID(pID);
                    if (linked == 0) {//link child

                        int connID = this.db.GetMaxConnectionID();
                        connID = connID + 1;

                        string connectionID = string.Format("{0:000000}", connID);
                        fID = MakeFamilyID(pID);
                        bool guardianExists = false;
                        guardianExists = gdb.GuardianNotDeletedAndExists(pID);
                        if (guardianExists)
                            conDB.UpdateAllowedConnections(connectionID, pID, childID, fID);
                        else
                            WPFMessageBox.Show("Guardian with ID: " + pID + " does not exist.");
                    } else if (linked == 1) {//delink child
                        bool connExists = conDB.ConnectionExists(pID, childID);
                        bool guardianExists = gdb.GuardianNotDeletedAndExists(pID);
                        if (connExists && guardianExists)
                            conDB.DeleteAllowedConnection(childID, pID);

                        else
                            WPFMessageBox.Show("No connection to the Guardian with ID: " + pID + " exists or no Guardian with that ID exists.");
                    }
                    this.Close();
                }
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        internal string MakeFamilyID(string ID) {
            string familyID = "";

            for (int x = 0; x < ID.Length - 1; x++) {
                familyID += ID[x];
            }
            return familyID;
        }

        internal bool CheckIfNull() {
            if (string.IsNullOrWhiteSpace(this.txt_GuardianID.Text)) {
                WPFMessageBox.Show("Please enter the ID number.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_GuardianID2.Text)) {
                WPFMessageBox.Show("Please enter the ID number a second time.");
                return true;
            }

            return false;
        }

        internal bool CheckIfSame(string str1, string str2) {
            if (str1.Equals(str2))
                return true;
            else {
                WPFMessageBox.Show("Your ID numbers do not match. Please re-enter");

                return false;
            }

        }

        internal bool CheckIfNumbers(string str1, string str2) {
            int parseNum1, parseNum2;

            bool isNum1 = int.TryParse(str1, out parseNum1);
            bool isNum2 = int.TryParse(str2, out parseNum2);

            if (isNum1 && isNum2)
                return true;
            else {
                WPFMessageBox.Show("Your ID numbers are not numbers only. Please re-enter.");

                return false;
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void SelectAllGotFocus(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)sender;
            Dispatcher.BeginInvoke((Action)(tb.SelectAll));
        }

        private void Key_Up_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }
    }
}
