using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChildCareAppParentSide {
    
    public partial class win_AdminEditChildInfo : Window {
        private AdminDatabase db;
        DataSet DS = new DataSet();
        private string ID;
        private bool isTablet;
        private string imagePath;
        private Settings settings;

        public win_AdminEditChildInfo(string parentID, bool isTablet) {
            InitializeComponent();
            this.settings = Settings.Instance;
            this.ID = parentID;
            this.db = new AdminDatabase();
            this.isTablet = isTablet;
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo(parentID);
            setChildBox();
            lst_ChildBox.SelectionChanged += ListBoxSelectionChanged;
            this.txt_FirstName.GotFocus += FocusedTextBox;
            this.txt_LastName.GotFocus += FocusedTextBox;
            this.txt_Medical.GotFocus += FocusedTextBox;
            this.txt_Allergies.GotFocus += FocusedTextBox;
            this.txt_imageName.GotFocus += FocusedTextBox;
            if (isTablet) {
                btn_Keyboard.Visibility = Visibility.Visible;
                btn_Keyboard.IsEnabled = true;
            }
        }//end win_AdminEditChildInfo

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            bool formNotComplete = true;
            formNotComplete = CheckIfNull();

            if (formNotComplete == false)
            {
                string cID, firstName, lastName, medical, allergies, birthday, imagePath;

                firstName = txt_FirstName.Text;
                lastName = txt_LastName.Text;
                birthday = dte_Birthday.SelectedDate.Value.ToString("yyyy-MM-dd"); 
                medical = txt_Medical.Text;
                allergies = txt_Allergies.Text;
                imagePath = txt_imageName.Text;
                cID = ((Child)(lst_ChildBox.SelectedItem)).ID;

                this.db.updateChildInfo(cID, firstName, lastName, birthday, medical, allergies, imagePath);
                ((Child)(lst_ChildBox.SelectedItem)).firstName = firstName;
                ((Child)(lst_ChildBox.SelectedItem)).lastName = lastName;
                ((Child)(lst_ChildBox.SelectedItem)).birthday = birthday;
                ((Child)(lst_ChildBox.SelectedItem)).medical = medical;
                ((Child)(lst_ChildBox.SelectedItem)).allergies = allergies;
                ClearFields();
                lst_ChildBox.Items.Clear();
                setChildBox();
                img_childPic.Source = null;

            }
        }//end btn_Submit_Click


        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

            bool? delete;
            win_Confirmation DeleteConformation = new win_Confirmation("Are you sure you would like to delete this child?");
            delete = DeleteConformation.ShowDialog();

            if ((bool)delete == true) {
               
                string cID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                this.db.deleteChildInfo(cID, ID);
                lst_ChildBox.Items.Clear();
                ClearFields();
                setChildBox();
            }
        }//end btn_Delete_Click

        private void DisableForm(){ 
            btn_Delete.IsEnabled = false;
            btn_Submit.IsEnabled = false; 
        }

        private void btn_MainMenu_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close(); 
        }//end btn_MainMenu_Click

        private void LoadParentInfo(string parentID) {
            txt_IDNumber.Text = parentID;

        }//end LoadParentInfo

        private bool CheckIfNull() {


            if (string.IsNullOrWhiteSpace(this.txt_FirstName.Text)){
                MessageBox.Show("Please enter your first name.");
                return true;
            }

            else if (string.IsNullOrWhiteSpace(this.txt_LastName.Text)){
                MessageBox.Show("Please enter your last name.");
                return true;
            }


            else if ( string.IsNullOrWhiteSpace(this.dte_Birthday.Text)){
                MessageBox.Show("Please enter the birthday. MM/DD/YYYY");
                return true;
            }
            return false;
        }//end CheckIfNull

        private void ClearFields(){
            txt_FirstName.Clear();
            txt_LastName.Clear();

            txt_IDNumber.Clear();
            txt_Allergies.Clear();

            txt_Medical.Clear();
            dte_Birthday.Text = "01/01/2005";
            txt_imageName.Clear();
        }//end ClearFields

        private void setChildBox() {
            string[,] childrenData = db.findChildren(this.ID);

            if (childrenData == null) {
                return;
            }

            if (childrenData != null) {
                for (int x = 0; x < childrenData.GetLength(0); x++) {
                    Image image = buildImage(childrenData[x, 6], 60);
                    this.imagePath = childrenData[x, 6];
                    lst_ChildBox.Items.Add(new Child(childrenData[x, 1], childrenData[x, 2], image, childrenData[x, 0],
                                                    childrenData[x, 3], childrenData[x, 4], childrenData[x, 5]));
                }
            }
            
        }//end setUpCheckInBox

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
                var fileInfo = new FileInfo(@""+settings.photoPath+"/default.jpg");
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            return image;
        }//end buildImage	

        private void ListBoxSelectionChanged(object sender, System.EventArgs e) {

            if (lst_ChildBox.SelectedItem != null) {
                txt_IDNumber.Text = this.ID;
                txt_FirstName.Text = ((Child)(lst_ChildBox.SelectedItem)).firstName;
                txt_LastName.Text = ((Child)(lst_ChildBox.SelectedItem)).lastName;
                dte_Birthday.Text = ((Child)(lst_ChildBox.SelectedItem)).birthday;
                txt_Medical.Text = ((Child)(lst_ChildBox.SelectedItem)).medical;
                txt_Allergies.Text = ((Child)(lst_ChildBox.SelectedItem)).allergies;
                dte_Birthday.SelectedDate = DateTime.Parse(dte_Birthday.Text);
                img_childPic.Source = ((Child)(lst_ChildBox.SelectedItem)).image.Source;
                img_childPic.Width = 150;
                txt_imageName.Text = this.imagePath;
            }      
        }

        private void btn_AddChild_Click(object sender, RoutedEventArgs e)
        {
            int maxID = this.db.getMaxID();
            int connID = this.db.getMaxConnectionID();
            if (maxID == -1 || connID == -1){
                MessageBox.Show("Unable to add new child");
            }
            maxID = maxID + 1;
            string mID = maxID.ToString();  

            Image i; 
            i = buildImage(@""+settings.photoPath+"/default.jpg", 60); 
            lst_ChildBox.Items.Add(new Child("First", "Last", i, mID, "2005/01/01", "None", "None"));

            connID = connID + 1;
            string connectionID = connID.ToString();

            this.db.addNewChild(mID, "First", "Last", "2005-01-01", "None", "None", settings.photoPath + "/default.jpg");
            this.db.updateAllowedConnections(connectionID, ID, mID);
 
            lst_ChildBox.Items.Clear();
            setChildBox();
        }

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

        private void btn_linkToGuardian_Click(object sender, RoutedEventArgs e) {
            if (lst_ChildBox.SelectedItem != null) {
                string childID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                string guardianID = getID();
                if (!String.IsNullOrEmpty(guardianID)) {
                    int max = db.getMaxConnectionID();
                    max++;
                    db.updateAllowedConnections(Convert.ToString(max), guardianID, childID);
                    MessageBox.Show("Child has been linked");
                }
                else {
                    MessageBox.Show("Chould not retrieve valid guardian ID");
                }
            }
        }//end btn_linkToAnotherGuardian

        private string getID() {
            win_EnterID enterID = new win_EnterID(this.isTablet);
            enterID.WindowState = WindowState.Maximized;
            bool? done = enterID.ShowDialog();
            return Convert.ToString(enterID.getID());
        }

        private void btn_chooseImage_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            string path = Path.GetFullPath(settings.photoPath);
            path = path.Replace(@"/", @"\");
            dlg.InitialDirectory = path;
            bool? result = dlg.ShowDialog();
            if (result == true) {
                string filename = dlg.FileName;
                filename = filename.Replace(@"\", @"/");
                int pos = filename.LastIndexOf("/") + 1;
                filename = filename.Substring(pos, filename.Length - pos);
                this.txt_imageName.Text = settings.photoPath + "/" + filename;
            }
        }//end getID

    }//end win_AdminEditChildInfo(class)


}//end nameSpace
