namespace MockUp
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IDInput = new System.Windows.Forms.TextBox();
            this.EnterID = new System.Windows.Forms.Button();
            this.Selection = new System.Windows.Forms.ListView();
            this.Children = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CheckIn = new System.Windows.Forms.Button();
            this.CheckOut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IDInput
            // 
            this.IDInput.Location = new System.Drawing.Point(172, 31);
            this.IDInput.Multiline = true;
            this.IDInput.Name = "IDInput";
            this.IDInput.Size = new System.Drawing.Size(155, 24);
            this.IDInput.TabIndex = 0;
            // 
            // EnterID
            // 
            this.EnterID.Location = new System.Drawing.Point(24, 31);
            this.EnterID.Name = "EnterID";
            this.EnterID.Size = new System.Drawing.Size(122, 36);
            this.EnterID.TabIndex = 1;
            this.EnterID.Text = "Enter ID";
            this.EnterID.UseVisualStyleBackColor = true;
            this.EnterID.Click += new System.EventHandler(this.EnterID_Click);
            // 
            // Selection
            // 
            this.Selection.AccessibleName = "";
            this.Selection.BackColor = System.Drawing.SystemColors.Menu;
            this.Selection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Children});
            this.Selection.Location = new System.Drawing.Point(61, 98);
            this.Selection.Name = "Selection";
            this.Selection.Size = new System.Drawing.Size(507, 133);
            this.Selection.TabIndex = 2;
            this.Selection.UseCompatibleStateImageBehavior = false;
            this.Selection.View = System.Windows.Forms.View.Details;
            this.Selection.SelectedIndexChanged += new System.EventHandler(this.Selection_SelectedIndexChanged);
            // 
            // Children
            // 
            this.Children.Text = "Children";
            this.Children.Width = 300;
            // 
            // CheckIn
            // 
            this.CheckIn.Location = new System.Drawing.Point(61, 262);
            this.CheckIn.Name = "CheckIn";
            this.CheckIn.Size = new System.Drawing.Size(110, 41);
            this.CheckIn.TabIndex = 3;
            this.CheckIn.Text = "Check In";
            this.CheckIn.UseVisualStyleBackColor = true;
            this.CheckIn.Visible = false;
            this.CheckIn.Click += new System.EventHandler(this.CheckIn_Click);
            // 
            // CheckOut
            // 
            this.CheckOut.Location = new System.Drawing.Point(256, 262);
            this.CheckOut.Name = "CheckOut";
            this.CheckOut.Size = new System.Drawing.Size(112, 41);
            this.CheckOut.TabIndex = 4;
            this.CheckOut.Text = "Check Out";
            this.CheckOut.UseVisualStyleBackColor = true;
            this.CheckOut.Visible = false;
            this.CheckOut.Click += new System.EventHandler(this.CheckOut_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 339);
            this.Controls.Add(this.CheckOut);
            this.Controls.Add(this.CheckIn);
            this.Controls.Add(this.Selection);
            this.Controls.Add(this.EnterID);
            this.Controls.Add(this.IDInput);
            this.Name = "MainWindow";
            this.Text = "MainWndow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IDInput;
        private System.Windows.Forms.Button EnterID;
        private System.Windows.Forms.ListView Selection;
        private System.Windows.Forms.ColumnHeader Children;
        private System.Windows.Forms.Button CheckIn;
        private System.Windows.Forms.Button CheckOut;


    }
}

