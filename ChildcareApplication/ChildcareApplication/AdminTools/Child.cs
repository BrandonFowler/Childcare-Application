using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Controls;


namespace ChildCareApp {
    class Child {
        public Child(string ID, string firstName, string lastName,
                        Image image, string birthday, string medical, string allergies, string path) {
						
				this.ID = ID;
                this.firstName = firstName;
                this.lastName = lastName;
                this.image = image;
                this.birthday = birthday;
				this.medical = medical;
				this.allergies = allergies;
                this.path = path; 
               
            }// end constructor
			
			public string ID { get; set; }

            public string firstName { get; set; }

            public string lastName { get; set; }

            public Image image { get; set; }

            public string birthday { get; set;}
			
			public string medical { get; set;}
			
			public string allergies { get; set;}

            public string path { get; set; }
    }
}
