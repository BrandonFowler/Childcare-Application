using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChildCareAppParentSide {
   
    public partial class win_AdminEditParentInfo : Window {

        private AdminDatabase db;
        private string parentID;
        private bool isTablet;

        public win_AdminEditParentInfo(string parentID, bool isTablet) {
            InitializeComponent();
            AddStates();
            this.parentID = parentID;
            this.isTablet = isTablet;
            this.db = new AdminDatabase();
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo();
            setUpParentDisplay();
            this.txt_Address.GotFocus += FocusedTextBox;
            this.txt_FirstName.GotFocus += FocusedTextBox;
            this.txt_LastName.GotFocus += FocusedTextBox;
            this.txt_PhoneNumber.GotFocus += FocusedTextBox;
            this.txt_imageName.GotFocus += FocusedTextBox;
            this.txt_LastName.GotFocus += FocusedTextBox;
            this.txt_Email.GotFocus += FocusedTextBox;
            this.txt_City.GotFocus += FocusedTextBox;
            this.txt_Zip.GotFocus += FocusedTextBox;
            if (isTablet) {
                btn_Keyboard.Visibility = Visibility.Visible;
                btn_Keyboard.IsEnabled = true;
            }
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {

            bool formNotComplete = true; 
            formNotComplete = CheckIfNull(); 

            if (formNotComplete == false)
            {
                string pID, firstName, lastName, address, city, state, zip, email, phone, imageName;
                pID = txt_IDNumber.Text; 
                firstName = txt_FirstName.Text;
                lastName = txt_LastName.Text;
                phone = txt_PhoneNumber.Text; 
                email = txt_Email.Text; 
                address = txt_Address.Text;
                city = txt_City.Text;
                state = cbo_State.Text; 
                zip = txt_Zip.Text;
                imageName = txt_imageName.Text;
                this.db.updateParentInfo(pID, firstName, lastName, phone, email, address, city, state, zip, imageName);
                setUpParentDisplay();
            }
        }//end btn_Submit_Click

        private void btn_Finish_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close(); 
        }//end btn_Finish_Click

        private void btn_AddChild_Click(object sender, RoutedEventArgs e) {

            string pID = txt_IDNumber.Text; 
            win_AdminEditChildInfo AdminEditChildInfo = new win_AdminEditChildInfo(pID, this.isTablet);
            AdminEditChildInfo.WindowState = WindowState.Maximized;
            AdminEditChildInfo.Show();
            this.Close();

        }//end btn_AddChild_Click

        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

            bool? delete;

            win_Confirmation DeleteConformation = new win_Confirmation("Are you sure you would like to delete this guardian?");
            delete = DeleteConformation.ShowDialog();
            if ((bool)delete == true)
            {
                string pID = txt_IDNumber.Text;
                this.db.deleteParentInfo(pID);
                ClearFields();
                DisableForm(); 
            }


        }//end btn_Delete_Click

        private void DisableForm() {
            btn_EditChild.IsEnabled = false;
            btn_Delete.IsEnabled = false;
            btn_SubmitInfo.IsEnabled = false; 
        }//end DisableForm

        private void ClearFields() {
            txt_Address.Clear();
            txt_City.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_IDNumber.Clear();
            txt_PhoneNumber.Clear();
            txt_Zip.Clear();
            txt_Email.Clear();
            txt_IDNumber.Clear();
            txt_imageName.Clear();
        }//end ClearFields

        private bool CheckIfNull() {

            if (string.IsNullOrWhiteSpace(this.txt_Address.Text))
            {
                MessageBox.Show("Please enter your address.");
                return true; 
            }

            else if (string.IsNullOrWhiteSpace(this.txt_City.Text))
            {
                MessageBox.Show("Please enter your city.");
                return true; 
            }

            else if (string.IsNullOrWhiteSpace(this.txt_Zip.Text))
            {
                MessageBox.Show("Please enter your zip.");
                return true;     
            }

            else if (string.IsNullOrWhiteSpace(this.txt_FirstName.Text))
            {
                MessageBox.Show("Please enter your first name.");
                return true;
            }

            else if (string.IsNullOrWhiteSpace(this.txt_LastName.Text))
            {
                MessageBox.Show("Please enter your last name.");
                return true;
            }

            else if (string.IsNullOrWhiteSpace(this.cbo_State.Text))
            {
                MessageBox.Show("Please enter your state.");
                return true;
            }

            else if (string.IsNullOrWhiteSpace(this.txt_Email.Text))
            {
                MessageBox.Show("Please enter your e-mail.");
                return true;
            }
            return false; 
        }//end checkIfNull

        private void LoadParentInfo() {

            txt_IDNumber.Text = this.parentID;
            DataSet DS = new DataSet();
            DS = this.db.getParentInfo(this.parentID);
            int count = DS.Tables[0].Rows.Count;
            if(count > 0)
            {
                 txt_FirstName.Text = DS.Tables[0].Rows[0][2].ToString();
                 txt_LastName.Text =  DS.Tables[0].Rows[0][3].ToString();
                 txt_PhoneNumber.Text =  DS.Tables[0].Rows[0][4].ToString();
                 txt_Email.Text =  DS.Tables[0].Rows[0][5].ToString();
                 txt_Address.Text =  DS.Tables[0].Rows[0][6].ToString();
                 txt_City.Text =  DS.Tables[0].Rows[0][8].ToString();
                 txt_Zip.Text =  DS.Tables[0].Rows[0][10].ToString();
                 cbo_State.Text =  DS.Tables[0].Rows[0][9].ToString();
                 string imagePath = DS.Tables[0].Rows[0][11].ToString();
                 int pos = imagePath.LastIndexOf("\\") + 1;
                 imagePath = imagePath.Substring(pos, imagePath.Length - pos);
                 txt_imageName.Text = imagePath;
            }

        }//end LoadParentInfo
        private void AddStates() {

            cbo_State.SelectedIndex = 46;
            
            cbo_State.Items.Add("AL");
            cbo_State.Items.Add("AK");
            cbo_State.Items.Add("AZ");
            cbo_State.Items.Add("AR");
            cbo_State.Items.Add("CA");
            cbo_State.Items.Add("CO");
            cbo_State.Items.Add("CT");
            cbo_State.Items.Add("DE");
            cbo_State.Items.Add("FL");
            cbo_State.Items.Add("GA");

            cbo_State.Items.Add("HI");
            cbo_State.Items.Add("ID");
            cbo_State.Items.Add("IL");
            cbo_State.Items.Add("IN");
            cbo_State.Items.Add("IA");
            cbo_State.Items.Add("KS");
            cbo_State.Items.Add("KY");
            cbo_State.Items.Add("LA");
            cbo_State.Items.Add("ME");
            cbo_State.Items.Add("MD");

            cbo_State.Items.Add("MA");
            cbo_State.Items.Add("MI");
            cbo_State.Items.Add("MN");
            cbo_State.Items.Add("MS");
            cbo_State.Items.Add("MO");
            cbo_State.Items.Add("MT");
            cbo_State.Items.Add("NE");
            cbo_State.Items.Add("NV");
            cbo_State.Items.Add("NH");
            cbo_State.Items.Add("NJ");

            cbo_State.Items.Add("NM");
            cbo_State.Items.Add("NY");
            cbo_State.Items.Add("NC");
            cbo_State.Items.Add("ND");
            cbo_State.Items.Add("OH");
            cbo_State.Items.Add("OK");
            cbo_State.Items.Add("OR");
            cbo_State.Items.Add("PA");
            cbo_State.Items.Add("RI");
            cbo_State.Items.Add("SC");

            cbo_State.Items.Add("SD");
            cbo_State.Items.Add("TN");
            cbo_State.Items.Add("TX");
            cbo_State.Items.Add("UT");
            cbo_State.Items.Add("VT");
            cbo_State.Items.Add("VA");
            cbo_State.Items.Add("WA");
            cbo_State.Items.Add("WV");
            cbo_State.Items.Add("WI");
            cbo_State.Items.Add("WY");

        }//end AddStates

        public void setUpParentDisplay() {
            string imagePath = "../../../../Photos/";
            lbl_Parent.Content = txt_FirstName.Text + " " + txt_LastName.Text;
            img_ParentPic.Source = (buildImage(imagePath + txt_imageName.Text, 150)).Source;
        }//end setUpParentDisplay

        private Image buildImage(string path, int size) {
            Image image = new Image();
            image.Width = size;

            try {
                BitmapImage bitmapImage = new BitmapImage();
                var fileInfo = new FileInfo(@"" + path);
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            } catch {
                BitmapImage bitmapImage = new BitmapImage();
                var fileInfo = new FileInfo(@"../../../../Photos/default.jpg");
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            return image;
        }//end buildImage

        private void FocusedTextBox(object sender, EventArgs e) {
            if (isTablet) {
                startKeyBoard();
            }
        }//end OnTDBoxFocus

        private void btn_Keyboard_Click(object sender, RoutedEventArgs e) {
            startKeyBoard();
        }//end btn_KeyBoard_Click

        private void startKeyBoard() {
            Version win8version = new Version(6, 2, 9200, 0);

            if (Environment.OSVersion.Version >= win8version) {
                string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                string keyboardPath = Path.Combine(progFiles, "TabTip.exe");
                Process.Start(keyboardPath);
            }
        }//end startKeyBoard

    }//end win_AdminEditParentInfo(class)
}//end nameSpace
