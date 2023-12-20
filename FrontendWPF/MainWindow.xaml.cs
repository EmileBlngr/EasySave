using FrontendWPF;
using Newtonsoft.Json;
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
        private Dictionary<string, string> localizedResources;

        public MainWindow()
        {

            InitializeComponent();
            UpdateLanguage(App.CurrentLanguage); // Use global language setting
            App.LanguageChanged += UpdateLanguage; // Subscribe to the global event

            LogsButton.Click += LogsButton_Click;

        }
        private void PageParam_LanguageChanged(string newLanguage)
        {
            UpdateLanguage(newLanguage);
        }


        private void UpdateLanguage(string cultureCode)
        {
            // Form the relative path from the executable to the JSON files
            string relativePath = $"../../../../Backend/Data/Languages/{cultureCode}.json";
            string fullPath = System.IO.Path.GetFullPath(relativePath, AppDomain.CurrentDomain.BaseDirectory);

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                localizedResources = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (localizedResources != null)
                {

                    NewSaveButton.Content = localizedResources["NewBackup"];
                    LogsButton.Content = localizedResources["AccessLogs"];
                    SettingsButton.Content = localizedResources["Settings"];
                    TrackSavesButton.Content = localizedResources["TrackSaves"];
                    CloseButton.Content = localizedResources["CloseButton"];
                    mySaves.Text = localizedResources["mySaves"];
                    newSave.Text = localizedResources["newSave"];
                    estimatedTime.Text = localizedResources["estimatedTime"];

                }
            }
            else
            {
                MessageBox.Show($"Language file not found: {fullPath}");
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the application when the CloseButton is clicked
            Application.Current.Shutdown();
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

        private void TrackSavesButton_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = null;
        }

    }



}
