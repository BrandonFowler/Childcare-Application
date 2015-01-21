using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace MockUp
{
    public partial class MainWindow : Form
    {
        private Database db;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.db = new Database();
        }

        private void EnterID_Click(object sender, EventArgs e)
        {
            Selection.Items.Clear();
            string id = this.IDInput.Text;
            this.IDInput.Text = "";
            String[] names = db.findChildren(id);
            int i = 0;
            while (i < names.Length) {
                Selection.Items.Add(names[i]);
                i++;
            }
        }

        private void Selection_SelectedIndexChanged(object sender, EventArgs e) {
            CheckIn.Visible = true;
            CheckOut.Visible = true;
        }

        private void CheckIn_Click(object sender, EventArgs e) {
            string selection = Selection.SelectedItems[0].Text;
            bool checkedIn = this.db.checkIn(selection);
            Selection.Items.Clear();
            if (checkedIn){
                MessageBox.Show(selection + " checked in");
            }
            else {
                MessageBox.Show("Check in failure");
            }
        }

        private void CheckOut_Click(object sender, EventArgs e) {
            string selection = Selection.SelectedItems[0].Text;
            bool checkedIn = this.db.checkOut(selection);
            Selection.Items.Clear();
            if (checkedIn) {
                MessageBox.Show(selection + " checked out");
            }
            else {
                MessageBox.Show("Check out failure");
            }
        }

    }
}
