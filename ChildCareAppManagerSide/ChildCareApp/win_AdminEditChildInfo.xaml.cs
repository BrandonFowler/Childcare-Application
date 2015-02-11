using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Collections;
using System.IO;

namespace ChildCareApp {
    /// <summary>
    /// Interaction logic for win_AdminEditChildInfo.xaml
    /// </summary>
    /// 

    public partial class win_AdminEditChildInfo : Window {
        private ChildInfoDatabse db;
        private int childIndex;
        DataSet DS = new DataSet();
        private string ID;

        public win_AdminEditChildInfo(string parentID) {
            InitializeComponent();
            this.ID = parentID;
            this.db = new ChildInfoDatabse();
            cnv_ChildIcon.Background = new SolidColorBrush(Colors.Aqua); //setting canvas color so we can see it
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo(parentID);
            //string childID =  GetChildID(parentID); 
            //LoadChildInfo(parentID); 
            setChildBox();
            lst_ChildBox.SelectionChanged += ListBoxSelectionChanged;
        }


        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            bool formNotComplete = true;
            formNotComplete = CheckIfNull();

            if (formNotComplete == false)
            {
                string cID, firstName, lastName, year, medical, allergies;


                firstName = txt_FirstName.Text;
                lastName = txt_LastName.Text;


                year = txt_Year.Text; 

                

                medical = txt_Medical.Text;
                allergies = txt_Allergies.Text;
                cID = ((Child)(lst_ChildBox.SelectedItem)).ID;

                //cID = DS.Tables[0].Rows[childIndex][0].ToString();
                this.db.UpdateChildInfo(cID, firstName, lastName, year, medical, allergies);
                ((Child)(lst_ChildBox.SelectedItem)).firstName = firstName;
                ((Child)(lst_ChildBox.SelectedItem)).lastName = lastName;
                ((Child)(lst_ChildBox.SelectedItem)).birthday = year;
                ((Child)(lst_ChildBox.SelectedItem)).medical = medical;
                ((Child)(lst_ChildBox.SelectedItem)).allergies = allergies;

                lst_ChildBox.Items.Clear();
                setChildBox();

               // ClearFields();
            }
        }//end btn_Submit_Click


        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

            bool? delete;
            win_DeleteConformation DeleteConformation = new win_DeleteConformation();
            delete = DeleteConformation.ShowDialog();

            if ((bool)delete == true)
            {
               ///string cID = DS.Tables[0].Rows[childIndex][0].ToString();
                string cID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                this.db.DeleteChildInfo(cID);
                /*ClearFields();
                DS.Clear();
                string parentID = txt_IDNumber.Text;
                childIndex = 0; 
                //LoadChildInfo(parentID); */
                //DisableForm();
                lst_ChildBox.Items.Clear();
                setChildBox();
            }


        }//end btn_Delete_Click

        private void DisableForm()
        { 
            btn_Delete.IsEnabled = false;
            btn_Submit.IsEnabled = false; 
        }
        private void btn_MainMenu_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }//end btn_MainMenu_Click

        private void LoadParentInfo(string parentID) {
            txt_IDNumber.Text = parentID;

        }//end LoadParentInfo

        private string GetChildID(string parentID)
        {

            //sql statement to get chilIDd from parentID
            string cID = "";
            return cID;
        }//end LoadParentInfo

        private void LoadChildInfo(string parentID)
        {

            //DS.Clear(); 
            DS = this.db.GetChildInfo(parentID);
            int count = DS.Tables[0].Rows.Count;
            if (count > 0)
            {
                childIndex = 0;
                FillTextBox(); 
            }
            if (count > 0)
            {

            }
        }//LoadChildInfo

        private void FillTextBox() {

            txt_FirstName.Text = DS.Tables[0].Rows[childIndex][1].ToString();
            txt_LastName.Text = DS.Tables[0].Rows[childIndex][2].ToString();
            txt_Year.Text = DS.Tables[0].Rows[childIndex][3].ToString();
            txt_Medical.Text = DS.Tables[0].Rows[childIndex][5].ToString();
            txt_Allergies.Text = DS.Tables[0].Rows[childIndex][4].ToString();
          
            /*string imageLink = DS.Tables[0].Rows[0][6].ToString();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(imageLink, UriKind.Relative));
            cnv_ChildIcon.Background = ib; */

        }

        private bool CheckIfNull() {

           /* if (string.IsNullOrWhiteSpace(this.txt_Address.Text))
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
            }*/

            if (string.IsNullOrWhiteSpace(this.txt_FirstName.Text))
            {
                MessageBox.Show("Please enter your first name.");
                return true;
            }

            else if (string.IsNullOrWhiteSpace(this.txt_LastName.Text))
            {
                MessageBox.Show("Please enter your last name.");
                return true;
            }


           /* else if (string.IsNullOrWhiteSpace(this.cbo_State.Text))
            {
                MessageBox.Show("Please enter your state.");
                return true;
            }*/

            else if ( string.IsNullOrWhiteSpace(this.txt_Year.Text))
            {
                MessageBox.Show("Please enter the birthday. MM/DD/YYYY");
                return true;
            }
            return false;
        }//end CheckIfNull

        private void ClearFields()
        {
             txt_FirstName.Clear();
            txt_LastName.Clear();

            txt_IDNumber.Clear();
            txt_Allergies.Clear();

            txt_Medical.Clear();

            txt_Year.Clear();
            
        }//end ClarFields

        

        private void btn_Next_Click(object sender, RoutedEventArgs e) {

            int count = DS.Tables[0].Rows.Count;
            string pID = txt_IDNumber.Text; 

            childIndex = (childIndex + 1) % count;
            //LoadChildInfo(pID);  
            FillTextBox(); 
        }//end addStates

        private void setChildBox() {
            string[,] childrenData = db.findChildren(this.ID);

            if (childrenData == null) {
                return;
            }

            if (childrenData != null) {
                for (int x = 0; x < childrenData.GetLength(0); x++) {
                    Image image = buildImage(childrenData[x, 6], 60);
                    lst_ChildBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2],
                                            image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5]));
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
                var fileInfo = new FileInfo(@"../../../../Photos/default.jpg");
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
                txt_Year.Text = ((Child)(lst_ChildBox.SelectedItem)).birthday;
                txt_Medical.Text = ((Child)(lst_ChildBox.SelectedItem)).medical;
                txt_Allergies.Text = ((Child)(lst_ChildBox.SelectedItem)).allergies;
            }
            
        
        }
    }//end class


}//end nameSpace
