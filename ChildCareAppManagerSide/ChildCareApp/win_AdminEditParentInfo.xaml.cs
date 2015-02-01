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

namespace ChildCareApp {
    /// <summary>
    /// Interaction logic for win_AdminEditParentInfo.xaml
    /// </summary>
    public partial class win_AdminEditParentInfo : Window {
        public win_AdminEditParentInfo(string parentID) {
            InitializeComponent();
            AddStates();
            cnv_ParentIcon.Background = new SolidColorBrush(Colors.Aqua); //setting canvas color so we can see it
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo(parentID);
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {

            bool formNotComplete = true; 
            formNotComplete = CheckIfNull(); 

            //save all information to database
            if (formNotComplete == false)
            {
                string firstName, lastName, middle, address, city, state, zip, email;

                firstName = txt_FirstName.Text;
                lastName = txt_LastName.Text;
                middle = txt_MiddleInitial.Text;

                email = txt_Email.Text; 

                address = txt_Address.Text;
                city = txt_City.Text;
                state = cbo_State.Text; //dont know if this will work yet
                zip = txt_Zip.Text;


               //ClearFields();
            }
        }//end btn_Submit_Click

        private void btn_Finish_Click(object sender, RoutedEventArgs e) {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); 
        }//end btn_Finish_Click

        private void btn_AddChild_Click(object sender, RoutedEventArgs e) {


        }//end btn_AddChild_Click

        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

            bool? delete;
            win_DeleteConformation DeleteConformation = new win_DeleteConformation();
            delete = DeleteConformation.ShowDialog();
           
        }//end btn_Delete_Click

        private void ClearFields() {
            txt_Address.Clear();
            txt_City.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();
            txt_MiddleInitial.Clear();
            txt_IDNumber.Clear();
            txt_PhoneNumber.Clear();
            txt_Zip.Clear();
            txt_Email.Clear(); 
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

            else if (string.IsNullOrWhiteSpace(this.txt_MiddleInitial.Text))
            {
                MessageBox.Show("Please enter your middle initial.");
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

        private void LoadParentInfo(string parentID) {

            txt_IDNumber.Text = parentID;
            //txt_FirstName.Text = LoadParentInfoDatabase.GetFirstName(parentID); 

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


    }//end class
}//end nameSpace
