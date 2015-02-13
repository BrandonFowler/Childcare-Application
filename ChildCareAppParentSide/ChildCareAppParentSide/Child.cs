using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;


namespace ChildCareAppParentSide {

        public class Child {

            public Child(string firstName, string lastName, Image Image, string ID, string birthday) {
                this.firstName = firstName;
                this.lastName = lastName;
                this.birthday = birthday;
                this.image = Image;
                this.ID = ID;
               
            }// end constructor

            public string firstName { get; set; }

            public string lastName { get; set; }

            public Image image { get; set; }

            public string ID { get; set; }

            public string birthday { get; set; }

        }//end Child(Class)
}
