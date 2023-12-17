using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Sources;
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



            /* 
            ResourceDictionary dict = new ResourceDictionary();

             this.Resources.MergedDictionaries.Clear();

            switch 
            {

            }

              add (dict)*/

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
            System.Diagnostics.Debug.WriteLine("LogsButton_Click called");

            string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            try
            {
                if (System.IO.Directory.Exists(logPath))
                {
                    System.Diagnostics.Debug.WriteLine("Directory exists, attempting to open.");

                 
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = "explorer.exe",
                        Arguments = logPath,
                        UseShellExecute = true
                    });
                }
                else
                {

                    MessageBox.Show("Le dossier des logs n'existe pas dans le répertoire de l'application.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                } 
            }
            catch (Exception ex)
            {
                

                MessageBox.Show($"Impossible d'ouvrir le dossier des logs : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }



}
