using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChildCareApplicationParentSide {
    public partial class frm_Login : Form {

        private bool IDBoxSelected = false;
        private bool PINBoxSelected = false;
        private Database db;

        public frm_Login() {
            InitializeComponent();
            this.db = new Database();
            this.ActiveControl = this.txt_IDEntry;
        }//end frm_Login

        private void frm_Login_Load(object sender, EventArgs e) {
            this.Controls.Add(txt_IDEntry);
            this.txt_IDEntry.KeyPress += new KeyPressEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.GotFocus += OnIDBoxFocus;
            this.Controls.Add(txt_PINEntry);
            this.txt_PINEntry.KeyPress += new KeyPressEventHandler(KeyPressedValidateNumber);
            this.txt_PINEntry.GotFocus += OnPINBoxFocus;
        }// end frm_Login_Load

        private void btn_Number1_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "1";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "1";
            }
        }//end btn_Number1_Click

        private void btn_Number2_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "2";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "2";
            }
        }//end btn_Number2_Click

        private void btn_Number3_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "3";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "3";
            }
        }//btn_Number3_Click

        private void btn_Number4_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "4";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "4";
            }
        }//end btn_Number4_Click

        private void btn_Number5_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "5";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "5";
            }
        }//end btn_Number5_Click

        private void btn_Number6_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "6";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "6";
            }
        }//end btn_Number6_Click

        private void btn_Number7_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "7";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "7";
            }
        }//end btn_Number7_Click

        private void btn_Number8_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "8";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "8";
            }
        }//end btn_Number8_Click

        private void btn_Number9_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "9";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "9";
            }
        }//end btn_Number9_Click

        private void btn_Number0_Click(object sender, EventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "0";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "0";
            }
        }//end btn_Number0_Click

        private void btn_Clear_Click(object sender, EventArgs e) {
            if (IDBoxSelected) {
                this.txt_IDEntry.Clear();
            }
            if (PINBoxSelected) {
                this.txt_PINEntry.Clear();
            }
        }//end btn_Clear_Click

        private void OnIDBoxFocus(object sender, EventArgs e) {
            this.IDBoxSelected = true;
            this.PINBoxSelected = false;
        }//end OnTDBoxFocus

        private void OnPINBoxFocus(object sender, EventArgs e) {
            this.PINBoxSelected = true;
            this.IDBoxSelected = false;
        }//end OnPINBoxFocus

        private void KeyPressedValidateNumber(Object o, KeyPressEventArgs e) {
            char keypress = e.KeyChar;
            if (char.IsDigit(keypress) || e.KeyChar == Convert.ToChar(Keys.Back)) {

            }
            else {
                MessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }
        }//end KeyPressedValidateNumber

        private void btn_Login_Click(object sender, EventArgs e) {
            string ID = txt_IDEntry.Text;
            string PIN = txt_PINEntry.Text;
            bool userFound = this.db.validateLogin(ID, PIN);

            if (userFound) {
                MessageBox.Show("User found");
            }
            else {
                MessageBox.Show("User ID or PIN does not exist");
            }

            /*
              Still need to account for admin login
            */

        }//end btn_Login_Click

    }
}
