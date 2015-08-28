using System.Windows.Controls;

namespace AdminTools {
    class Child {
        public Child(string ID, string firstName, string lastName,
                        Image image, string birthday, string allergies, string medical, string path) {

            this.ID = ID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.image = image;
            this.birthday = birthday;
            this.medical = medical;
            this.allergies = allergies;
            this.path = path;
        }

        public string ID { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public Image image { get; set; }

        public string birthday { get; set; }

        public string medical { get; set; }

        public string allergies { get; set; }

        public string path { get; set; }
    }
}
