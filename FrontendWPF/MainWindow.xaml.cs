using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new PageNew()); // Assurez-vous que PageNew est une 'Page'
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new PageParam()); // Utilisez le nom correct de la classe PageParam


        }
    }
}