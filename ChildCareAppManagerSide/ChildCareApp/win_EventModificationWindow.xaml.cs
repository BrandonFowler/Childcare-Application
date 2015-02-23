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
    /// Interaction logic for win_EventModificationWindow.xaml
    /// </summary>
    public partial class win_EventModificationWindow : Window {
        private String eventID;

        public win_EventModificationWindow() {
            InitializeComponent();
        }

        public win_EventModificationWindow(String eventID) {
            this.eventID = eventID;
            InitializeComponent();
        }
    }
}
