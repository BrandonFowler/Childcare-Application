using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;


namespace ChildCareAppParentSide {

        public class Child {

            public Child(string name, Image Image) {
               this.name = name;
               this.image = Image;
               
            }// end constructor

            public string name { get; set; }

            public Image image { get; set; }

        }//end Child(Class)
}
