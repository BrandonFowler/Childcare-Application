using System;
using System.ComponentModel;

namespace ChildCareAppParentSide {

    public class DateAndTime : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime now;

        public DateAndTime() {
            now = DateTime.Now;
        }//end constructor

        public DateTime Now {
            get { return now; }
            private set {
                now = value;

                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("Now"));
                }
            }
        }//end DateTime(Embedded)

        public void Update() {
            Now = DateTime.Now;
        }//end Update

    }//end DateAndTime(Class)
}
