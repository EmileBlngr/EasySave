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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using FrontendWPF;
using System.Security.AccessControl;


namespace WpfApp1
{
    public partial class PageNew : Page
    {
        private Dictionary<string, string> localizedResources;

        public PageNew()
        {
            InitializeComponent();
            UpdateLanguage(App.CurrentLanguage); // Use global language setting
            App.LanguageChanged += UpdateLanguage; // Subscribe to the global event
        }

        private void PageNew_Unloaded(object sender, RoutedEventArgs e)
        {
            App.LanguageChanged -= UpdateLanguage; // Unsubscribe from the global event
        }




        private void UpdateLanguage(string cultureCode)
        {
            string relativePath = $"../../../../Backend/Data/Languages/{cultureCode}.json";
            string fullPath = System.IO.Path.GetFullPath(relativePath, AppDomain.CurrentDomain.BaseDirectory);

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                localizedResources = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (localizedResources != null)
                {
                    gbNewBackup.Header = localizedResources["NewBackupHeader"];
                    saveName.Text = localizedResources["saveName"];
                    saveType.Text = localizedResources["saveType"];
                    totalSave.Content = localizedResources["totalSave"];
                    diffSave.Content = localizedResources["diffSave"];
                    browse.Content = localizedResources["browse"];
                    sourcePath.Text = localizedResources["sourcePath"];
                    destinationPath.Text = localizedResources["destinationPath"];
                    browse2.Content = localizedResources["browse2"];
                    createSave.Content = localizedResources["createSave"];

                }
            }
            else
            {
                MessageBox.Show($"Language file not found: {fullPath}");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Your existing code
        }
    }




}