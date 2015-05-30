using ChildcareApplication.AdminTools;
using DatabaseController;
using MessageBoxUtils;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdminTools {
    public partial class AdminEditParentInfo : Window {

        private GuardianInfoDB db;
        private bool formError;
        public AdminEditParentInfo(string parentID) {

            InitializeComponent();
            AddStates();
            this.db = new GuardianInfoDB();
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo(parentID);
            this.MouseDown += WindowMouseDown;
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {

            bool formNotComplete = true;
            formNotComplete = CheckIfNull();

            //save all information to database
            if (formNotComplete == false) {
                bool regex = RegexValidation();

                if (regex) {
                    string pID, firstName, lastName, address, address2, city, state, zip, email, phone, path;

                    pID = txt_IDNumber.Text;
                    firstName = txt_FirstName.Text;
                    lastName = txt_LastName.Text;

                    phone = txt_PhoneNumber.Text;
                    email = txt_Email.Text;

                    address = txt_Address.Text;
                    address2 = txt_Address2.Text;
                    city = txt_City.Text;
                    state = cbo_State.Text;
                    zip = txt_Zip.Text;
                    path = txt_FilePath.Text;

                    this.db.UpdateParentInfo(pID, firstName, lastName, phone, email, address, address2, city, state, zip, path);
                }

            }
        }

        internal bool RegexValidation() {
            formError = true;
            bool fname = false, lname = false, phone = false, email = false, address = false, city = false, zip = false, path = false;
            if (formError) {
                fname = RegExpressions.RegexName(txt_FirstName.Text);
                if (!fname) {
                    txt_FirstName.Focus();
                    formError = false;
                }
            }
            if (formError) {
                lname = RegExpressions.RegexName(txt_LastName.Text);
                if (!lname) {
                    txt_LastName.Focus();
                    formError = false;
                }
            }
            if (formError) {
                phone = RegExpressions.RegexPhoneNumber(txt_PhoneNumber.Text);
                if (!phone) {
                    txt_PhoneNumber.Focus();
                    formError = false;
                }
            }
            if (formError) {
                email = RegExpressions.RegexEmail(txt_Email.Text);
                if (!email) {
                    txt_Email.Focus();
                    formError = false;
                }
            }
            if (formError) {
                address = RegExpressions.RegexAddress(txt_Address.Text);
                if (!address) {
                    txt_Address.Focus();
                    formError = false;
                }
            }
            if (formError) {
                city = RegExpressions.RegexCity(txt_City.Text);
                if (!city) {
                    txt_City.Focus();
                    formError = false;
                }
            }
            if (formError) {
                zip = RegExpressions.RegexZIP(txt_Zip.Text);
                if (!zip) {
                    txt_Zip.Focus();
                    formError = false;
                }
            }

            if (formError) {
                path = RegExpressions.RegexFilePath(txt_FilePath.Text);
                if (!path) {
                    formError = false;
                }
            }

            if (fname && lname && phone && email && address && city && zip && path)
                return true;

            return false;

        }

        private void btn_Finish_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void btn_AddChild_Click(object sender, RoutedEventArgs e) {

            string pID = txt_IDNumber.Text;
            AdminEditChildInfo AdminEditChildInfo = new AdminEditChildInfo(pID);
            AdminEditChildInfo.Show();
            this.Close();

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

            MessageBoxResult messageBoxResult = WPFMessageBox.Show("Are you sure you wish to delete this person?", "Deletion Conformation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                string pID = txt_IDNumber.Text;
                this.db.DeleteParentInfo(pID);
                ClearFields();
                DisableForm();
            }

        }

        private void DisableForm() {
            btn_EditChild.IsEnabled = false;
            btn_Delete.IsEnabled = false;
            btn_SubmitInfo.IsEnabled = false;
            btn_ChangePicture.IsEnabled = false;
            btn_changePIN.IsEnabled = false;
        }

        internal void ClearFields() {
            txt_Address.Clear();
            txt_Address2.Clear();
            txt_City.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_IDNumber.Clear();
            txt_PhoneNumber.Clear();
            txt_Zip.Clear();
            txt_Email.Clear();
            txt_FilePath.Clear();
        }

        internal bool CheckIfNull() {

            if (string.IsNullOrWhiteSpace(this.txt_Address.Text)) {
                WPFMessageBox.Show("Please enter your address.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_City.Text)) {
                WPFMessageBox.Show("Please enter your city.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_Zip.Text)) {
                WPFMessageBox.Show("Please enter your zip.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_FirstName.Text)) {
                WPFMessageBox.Show("Please enter your first name.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_LastName.Text)) {
                WPFMessageBox.Show("Please enter your last name.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.cbo_State.Text)) {
                WPFMessageBox.Show("Please enter your state.");
                return true;
            } else if (string.IsNullOrWhiteSpace(this.txt_Email.Text)) {
                WPFMessageBox.Show("Please enter your e-mail.");
                return true;
            }
            return false;
        }

        private void LoadParentInfo(string parentID) {

            txt_IDNumber.Text = parentID;
            DataSet DS = new DataSet();
            DS = this.db.GetParentInfoDS(parentID);
            int count = DS.Tables[0].Rows.Count;
            if (count > 0) {
                string deletionDate = "";
                deletionDate = DS.Tables[0].Rows[0][12].ToString();

                if (String.IsNullOrEmpty(deletionDate)) {
                    txt_FirstName.Text = DS.Tables[0].Rows[0][2].ToString();
                    txt_LastName.Text = DS.Tables[0].Rows[0][3].ToString();

                    txt_PhoneNumber.Text = DS.Tables[0].Rows[0][4].ToString();
                    txt_Email.Text = DS.Tables[0].Rows[0][5].ToString();

                    txt_Address.Text = DS.Tables[0].Rows[0][6].ToString();
                    txt_Address2.Text = DS.Tables[0].Rows[0][7].ToString();
                    txt_City.Text = DS.Tables[0].Rows[0][8].ToString();
                    txt_Zip.Text = DS.Tables[0].Rows[0][10].ToString();
                    cbo_State.Text = DS.Tables[0].Rows[0][9].ToString();

                    txt_FilePath.Text = DS.Tables[0].Rows[0][11].ToString();
                    string imageLink = DS.Tables[0].Rows[0][11].ToString();

                    if (File.Exists(imageLink)) {
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                        cnv_ParentIcon.Background = ib;
                    } else {
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(@"" + "C:/Users/Public/Documents" + "/Childcare Application/Pictures/default.jpg", UriKind.Relative));
                        cnv_ParentIcon.Background = ib;
                    }
                } else {
                    ClearFields();
                    DisableForm();
                    WPFMessageBox.Show("This Parent has already been deleted.");
                }
            }

        }

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

        }

        private void btn_ChangePicture_Click(object sender, RoutedEventArgs e) {

            string imagePath = @"" + "C:/Users/Public/Documents" + "/Childcare Application/Pictures"; //TAG: pictures access
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
                string path = "C:\\Users\\Public\\Documents" + "\\Childcare Application\\Pictures\\"; //TAG: pictures access
                string filename = dlg.FileName;
                string[] words = filename.Split('\\');

                path += words[words.Length - 1];

                if (File.Exists(path)) {
                    try {
                        string imageLink = path;
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
                        cnv_ParentIcon.Background = ib;
                        txt_FilePath.Text = path;
                    } catch (Exception) {
                        WPFMessageBox.Show("Could not change picture to" + path);

                    }
                } else {
                    WPFMessageBox.Show("The picture you specified is not in the Pictures folder in the Childcare Application folder in your documents folder!");
                }
            }
        }

        private void SelectAllGotFocus(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)sender;
            Dispatcher.BeginInvoke((Action)(tb.SelectAll));
        }
        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Key_Up_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

        private void btn_changePIN_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult messageBoxResult = WPFMessageBox.Show("Are you sure you wish to permanently change this person's PIN?", "PIN Chnage Conformation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                string pID = txt_IDNumber.Text;
                AdminChangeGuardianPIN adminChangeGuardianPIN = new AdminChangeGuardianPIN(pID);
                adminChangeGuardianPIN.ShowDialog();
            }
        }
    }
}
