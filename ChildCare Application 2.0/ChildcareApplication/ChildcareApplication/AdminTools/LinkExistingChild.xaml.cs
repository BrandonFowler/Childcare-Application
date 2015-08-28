using AdminTools;
using DatabaseController;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ChildcareApplication.AdminTools {
    /// <summary>
    /// Interaction logic for LinkExistingChild.xaml
    /// </summary>
    public partial class LinkExistingChild : Window {

        private ChildInfoDatabase db;
        DataSet DS = new DataSet();
        private string ID;
        private ArrayList connectedChildren;
        public LinkExistingChild(string pID, ArrayList connectedChildren) {
            InitializeComponent();
            this.db = new ChildInfoDatabase();
            this.ID = pID;
            this.connectedChildren = connectedChildren;
            setChildBox();
            this.MouseDown += WindowMouseDown;
        }

        private void setChildBox() {
            string fID = GetFamilyID(this.ID);
            string[,] childrenData = db.FindFamilyChildren(fID, this.ID);

            if (childrenData == null) {
                return;
            }

            if (childrenData != null) {
                for (int x = 0; x < childrenData.GetLength(0); x++) {
                    if (!connectedChildren.Contains(childrenData[x, 0]))//child already has a link to this parent
                    {
                        Image image = buildImage(childrenData[x, 6], 60);
                        lst_ChildBox.Items.Add(new Child(childrenData[x, 0], childrenData[x, 1], childrenData[x, 2],
                                                image, childrenData[x, 3], childrenData[x, 4], childrenData[x, 5], childrenData[x, 6]));
                    }
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
                var fileInfo = new FileInfo(@"" + "C:/Users/Public/Documents" + "Childcare Application/Pictures/default.jpg"); //TAG: pictures access
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileInfo.FullName);
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            return image;
        }//end buildImage	


        internal string GetFamilyID(string pID) {
            string familyID = "";

            for (int x = 0; x < pID.Length - 1; x++) {
                familyID += pID[x];
            }

            return familyID;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void btn_LinkChild_Click(object sender, RoutedEventArgs e) {
            if (lst_ChildBox.SelectedItem != null) {
                string childID = ((Child)(lst_ChildBox.SelectedItem)).ID;
                int connID = this.db.GetMaxConnectionID();
                connID = connID + 1;
                string connectionID = string.Format("{0:000000}", connID);
                string fID = GetFamilyID(this.ID);
                this.db.UpdateExistingChilderen(connectionID, this.ID, childID, fID);
                this.connectedChildren.Add(childID);
            }
            lst_ChildBox.Items.Clear();
            setChildBox();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
