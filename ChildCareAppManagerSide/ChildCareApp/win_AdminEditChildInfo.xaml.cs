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

namespace ChildCareApp {
    /// <summary>
    /// Interaction logic for win_AdminEditChildInfo.xaml
    /// </summary>
    /// 

    public partial class win_AdminEditChildInfo : Window {
        private ChildInfoDatabse db;
        private int childIndex;
        DataSet DS = new DataSet();

        public win_AdminEditChildInfo(string parentID) {
            InitializeComponent();
            AddStates();
            this.db = new ChildInfoDatabse();
            cnv_ChildIcon.Background = new SolidColorBrush(Colors.Aqua); //setting canvas color so we can see it
            btn_Delete.Background = new SolidColorBrush(Colors.Red);
            LoadParentInfo(parentID);
            //string childID =  GetChildID(parentID); 
            LoadChildInfo(parentID); 
        }


        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            bool formNotComplete = true;
            formNotComplete = CheckIfNull();

            if (formNotComplete == false)
            {
                string cID, firstName, lastName, middle, address, city, state, zip, month, day, year, medical, allergies;


                firstName = txt_FirstName.Text;
                lastName = txt_LastName.Text;


                year = txt_Year.Text; 

                address = txt_Address.Text;
                city = txt_City.Text;
                state = cbo_State.Text; //dont know if this will work yet
                zip = txt_Zip.Text;

                medical = txt_Medical.Text;
                allergies = txt_Allergies.Text;

                cID = DS.Tables[0].Rows[childIndex][0].ToString();
                this.db.UpdateChildInfo(cID, firstName, lastName, year, medical, allergies);

               // ClearFields();
            }
        }//end btn_Submit_Click


        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

            bool? delete;
            win_DeleteConformation DeleteConformation = new win_DeleteConformation();
            delete = DeleteConformation.ShowDialog();

            if ((bool)delete == true)
            {
               /* string cID = DS.Tables[0].Rows[childIndex][0].ToString();
                this.db.DeleteChildInfo(cID);
                ClearFields();
                DS.Clear();
                string parentID = txt_IDNumber.Text;
                childIndex = 0; 
                //LoadChildInfo(parentID); */
                DisableForm();
            }


        }//end btn_Delete_Click

        private void DisableForm()
        {
            btn_Next.IsEnabled = false; 
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
            txt_Address.Clear();
            txt_City.Clear();
            txt_FirstName.Clear();
            txt_LastName.Clear();

            txt_IDNumber.Clear();
            txt_Allergies.Clear();

            txt_Medical.Clear();

            txt_Year.Clear();
            txt_Zip.Clear();
        }//end ClarFields

        private void AddStates()
        {

            cbo_State.SelectedIndex = 46;//shoulkd be sent to sql statement for states

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

        private void btn_Next_Click(object sender, RoutedEventArgs e) {

            int count = DS.Tables[0].Rows.Count;
            string pID = txt_IDNumber.Text; 

            childIndex = (childIndex + 1) % count;
            //LoadChildInfo(pID);  
            FillTextBox(); 
        }//end addStates
    }//end class
}//end nameSpace
