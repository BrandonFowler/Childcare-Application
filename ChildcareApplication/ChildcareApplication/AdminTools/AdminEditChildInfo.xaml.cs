using ChildcareApplication.AdminTools;
using DatabaseController;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_AdminEditChildInfo.xaml
    /// </summary>
    /// 

    public partial class win_AdminEditChildInfo : Window {
        private ChildInfoDatabase db;
        DataSet DS = new DataSet();
        private string ID;

        public win_AdminEditChildInfo(string parentID) {
            InitializeComponent();
            this.ID = parentID;
            this.db = new ChildInfoDatabase();
            cnv_ChildIcon.Background = new SolidColorBrush(Colors.Aqua); //setting canvas color so we can see it
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo(parentID);
            setChildBox();
            lst_ChildBox.SelectionChanged += ListBoxSelectionChanged;
        }


        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            bool formNotComplete = true;
            formNotComplete = CheckIfNull();

            if (formNotComplete == false) {

                bool regex = RegexValidation();
                if (regex) {


                    string cID, firstName, lastName, medical, allergies, birthday, filePath;

                    firstName = txt_FirstName.Text;
                    lastName = txt_LastName.Text;
                    birthday = dte_Birthday.SelectedDate.Value.ToString("yyyy-MM-dd");
                    medical = txt_Medical.Text;
                    allergies = txt_Allergies.Text;
                    cID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                    filePath = txt_FilePath.Text;

                    this.db.UpdateChildInfo(cID, firstName, lastName, birthday, medical, allergies, filePath);
                    ((Child)(lst_ChildBox.SelectedItem)).firstName = firstName;
                    ((Child)(lst_ChildBox.SelectedItem)).lastName = lastName;
                    ((Child)(lst_ChildBox.SelectedItem)).birthday = birthday;
                    ((Child)(lst_ChildBox.SelectedItem)).medical = medical;
                    ((Child)(lst_ChildBox.SelectedItem)).allergies = allergies;
                    ((Child)(lst_ChildBox.SelectedItem)).path = filePath;

                    lst_ChildBox.Items.Clear();
                    setChildBox();
                }
                // ClearFields();
            }
        }//end btn_Submit_Click


        private bool RegexValidation() {
            bool fname = RegExpressions.RegexName(txt_FirstName.Text);
            if (!fname)
                txt_FirstName.Focus();

            bool lname = RegExpressions.RegexName(txt_LastName.Text);
            if (!lname)
                txt_LastName.Focus();


            bool path = RegExpressions.RegexFilePath(txt_FilePath.Text);


            if (fname && lname && path)
                return true;

            return false;

        }
        private void btn_Delete_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you wish to delete this person?", "Deletion Conformation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                ConnectionsDB conDB = new ConnectionsDB();
                string cID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                string pID = txt_IDNumber.Text;
                this.db.DeleteChildInfo(cID);
                conDB.DeleteAllowedConnection(cID, pID);
                lst_ChildBox.Items.Clear();
                setChildBox();
                ClearFields();
            }


        }//end btn_Delete_Click

        private void DisableForm() {
            btn_Delete.IsEnabled = false;
            btn_Submit.IsEnabled = false;
            btn_ChangePicture.IsEnabled = false;
            btn_LinkChild.IsEnabled = false;
            btn_De_LinkChild.IsEnabled = false;
        }
        private void btn_MainMenu_Click(object sender, RoutedEventArgs e) {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Close();
        }//end btn_MainMenu_Click

        private void LoadParentInfo(string parentID) {
            txt_IDNumber.Text = parentID;

        }//end LoadParentInfo

        private bool CheckIfNull() {


            if (string.IsNullOrWhiteSpace(this.txt_FirstName.Text)) {
                MessageBox.Show("Please enter your first name.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_LastName.Text)) {
                MessageBox.Show("Please enter your last name.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.dte_Birthday.Text)) {
                MessageBox.Show("Please enter the birthday. MM/DD/YYYY");
                return true;
            }
            return false;
        }//end CheckIfNull

        private void ClearFields() {
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_Allergies.Clear();
            txt_Medical.Clear();
            txt_FilePath.Clear();
            dte_Birthday.Text = "01/01/2005";
        }//end ClarFields


        private void setChildBox() {
            string[,] childrenData = db.FindChildren(this.ID);

            if (childrenData == null) {
                return;
            }

            if (childrenData != null) {
                for (int x = 0; x < childrenData.GetLength(0); x++) {
                    Image image = buildImage(childrenData[x, 6], 60);
                    lst_ChildBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2],
                                            image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
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
                var fileInfo = new FileInfo(@"../../Pictures/default.jpg");
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
                txt_FirstName.Text = ((Child)(lst_ChildBox.SelectedItem)).firstName;
                txt_LastName.Text = ((Child)(lst_ChildBox.SelectedItem)).lastName;
                dte_Birthday.Text = ((Child)(lst_ChildBox.SelectedItem)).birthday;
                txt_Medical.Text = ((Child)(lst_ChildBox.SelectedItem)).medical;
                txt_Allergies.Text = ((Child)(lst_ChildBox.SelectedItem)).allergies;
                dte_Birthday.SelectedDate = DateTime.Parse(dte_Birthday.Text);
                txt_FilePath.Text = ((Child)(lst_ChildBox.SelectedItem)).path;

                string imageLink = ((Child)(lst_ChildBox.SelectedItem)).path;
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                cnv_ChildIcon.Background = ib;

            }
        }

        private void btn_AddChild_Click(object sender, RoutedEventArgs e) {
            int maxID = this.db.GetMaxChildID();
            ConnectionsDB conDB = new ConnectionsDB();

            maxID = maxID + 1;
            string mID = maxID.ToString();

            Image i;
            i = buildImage("../../../../Photos/default.jpg", 60);
            lst_ChildBox.Items.Add(new Child(mID, "First", "Last", i, "2005/01/01", "None", "None", "../../Pictures/default.jpg"));

            int connID = this.db.GetMaxConnectionID();

            connID = connID + 1;
            string connectionID = connID.ToString();

            string famID = GetFamilyID(ID);
            this.db.AddNewChild(mID, "First", "Last", "2005-01-01", "None", "None", "../../Pictures/default.jpg");

            conDB.UpdateAllowedConnections(connectionID, ID, mID, famID);

            lst_ChildBox.Items.Clear();
            setChildBox();
            lst_ChildBox.SelectedItem = lst_ChildBox.Items[lst_ChildBox.Items.Count - 1];
            txt_FirstName.Focus();

        }

        public string GetFamilyID(string pID) {
            string familyID = "";

            for (int x = 0; x < pID.Length - 1; x++) {
                familyID += pID[x];
            }

            return familyID;
        }

        private void btn_LinkChild_Click(object sender, RoutedEventArgs e) {
            if (lst_ChildBox.SelectedItem != null) {
                string cID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                int link = 0;
                Link_DeLinkChild linkDelinkChild = new Link_DeLinkChild(link, cID);
                linkDelinkChild.ShowDialog(); //0 = link

            }
        }

        private void btn_De_LinkChild_Click(object sender, RoutedEventArgs e) {
            if (lst_ChildBox.SelectedItem != null) {
                string cID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                int delink = 1;
                Link_DeLinkChild linkDelinkChild = new Link_DeLinkChild(delink, cID);
                linkDelinkChild.ShowDialog(); //1 = delink
                lst_ChildBox.Items.Clear();
                setChildBox();
                ClearFields();
            }
        }

        private void btn_LinkExistingChild_Click(object sender, RoutedEventArgs e) {
            string pID = txt_IDNumber.Text;
            ArrayList connectedChilderen = new ArrayList();
            connectedChilderen = getConnectedChilderen(connectedChilderen);
            LinkExistingChild linkExistingChild = new LinkExistingChild(pID, connectedChilderen);
            linkExistingChild.ShowDialog();
            lst_ChildBox.Items.Clear();
            setChildBox();
            ClearFields();
        }

        private ArrayList getConnectedChilderen(ArrayList list) {
            for (int i = 0; i < lst_ChildBox.Items.Count; i++) {
                list.Add(((Child)(lst_ChildBox.Items[i])).ID);
            }

            return list;
        }

        private void btn_ChangePicture_Click(object sender, RoutedEventArgs e) {
            if (lst_ChildBox.SelectedItem != null) {
               
                string imagePath = System.IO.Path.GetFullPath("../../Pictures");
                imagePath = imagePath.Replace(@"/", @"\");
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.FileName = "default"; // Default file name
                dlg.DefaultExt = ".jpg"; // Default file extension
                dlg.Filter = "Pictures (.jpg)|*.jpg"; // Filter files by extension 
                dlg.InitialDirectory = imagePath;
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results 
                if (result == true) {
                    // Open document 
                    string path = "../../Pictures/";
                    string filename = dlg.FileName;
                    string[] words = filename.Split('\\');

                    path += words[words.Length - 1];


                    try {
                        string imageLink = path;
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                        cnv_ChildIcon.Background = ib;
                        txt_FilePath.Text = path;
                    } catch (Exception) {
                        MessageBox.Show("Could not change picture to" + path);

                    }

                }

            }

        }//end btnChangePicture

        private void txt_GotFocus(object sender, RoutedEventArgs e) {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null) {
                textBox.SelectAll();
            }
        }

        private void txt_GotMouseCapture(object sender, MouseEventArgs e) {

            txt_GotFocus(sender, e);
        }

        private void txt_FirstName_GotFocus(object sender, RoutedEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void txt_LastName_GotFocus(object sender, RoutedEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void txt_Medical_GotFocus(object sender, RoutedEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void txt_Allergies_GotFocus(object sender, RoutedEventArgs e) {
            txt_GotFocus(sender, e);
        }

        private void txt_FirstName_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotMouseCapture(sender, e);
        }

        private void txt_LastName_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotMouseCapture(sender, e);
        }

        private void txt_Medical_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotMouseCapture(sender, e);
        }

        private void txt_Allergies_GotMouseCapture(object sender, MouseEventArgs e) {
            txt_GotMouseCapture(sender, e);
        }
    }
}
