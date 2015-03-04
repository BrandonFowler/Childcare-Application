using System.Windows.Controls;


namespace ChildCareAppParentSide {

        public class Child {

            public Child(string firstName, string lastName, Image Image, string ID, string birthday, string allergies, string medical) {
                this.firstName = firstName;
                this.lastName = lastName;
                this.birthday = birthday;
                this.image = Image;
                this.ID = ID;
                this.medical = medical;
                this.allergies = allergies;
               
            }// end constructor

            public string firstName { get; set; }

            public string lastName { get; set; }

            public Image image { get; set; }

            public string ID { get; set; }

            public string birthday { get; set; }

            public string medical { get; set; }

            public string allergies { get; set; }

        }//end Child(Class)
}
