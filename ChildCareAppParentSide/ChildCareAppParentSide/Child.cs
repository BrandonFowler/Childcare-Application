using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;


namespace ChildCareAppParentSide {

        public class Child {

            public Child(string fn, string ln, Image Image) {
                this.firstName = fn;
                this.lastName = ln;
                this.image = Image;
               
            }// end constructor

            public string firstName { get; set; }

            public string lastName { get; set; }

            public Image image { get; set; }

        }//end Child(Class)
}
