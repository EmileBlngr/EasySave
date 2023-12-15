using System.IO;
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

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LogsButton.Click += LogsButton_Click;
        }

        private void NewSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new PageNew()); 
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new PageParam()); 


        }
        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            try
            {
                if (System.IO.Directory.Exists(logPath))
                {
                    // Le dossier existe, ouvrir l'explorateur de fichiers sur ce chemin
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = "explorer.exe",
                        Arguments = logPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    // Si le dossier n'existe pas, informer l'utilisateur
                    MessageBox.Show("Le dossier des logs n'existe pas dans le répertoire de l'application.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
  
        }



    }
}