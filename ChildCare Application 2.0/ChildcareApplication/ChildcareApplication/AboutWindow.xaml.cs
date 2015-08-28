using System.Windows;
using System.Windows.Input;

namespace ChildcareApplication {
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window {
        public AboutWindow() {
            InitializeComponent();
            this.MouseDown += WindowMouseDown;
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
