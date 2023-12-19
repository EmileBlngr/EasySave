using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FrontendWPF;
using Newtonsoft.Json;

namespace WpfApp1
{
    public partial class PageParam : Page
    {
        private static string currentLanguage = "fr-FR"; // Default to French
        private Dictionary<string, string> localizedResources;

        public delegate void LanguageChangedEventHandler(string newLanguage);
        public static event LanguageChangedEventHandler LanguageChanged;

        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set { currentLanguage = value; }
        }

        public PageParam()
        {
            InitializeComponent();
            UpdateLanguage(CurrentLanguage); // Use static property

            // Set the radio button based on the current language
            switch (CurrentLanguage)
            {
                case "en-EN":
                    radioEnglish.IsChecked = true;
                    break;
                case "fr-FR":
                    radioFrench.IsChecked = true;
                    break;
                case "es-ES":
                    radioSpanish.IsChecked = true;
                    break;
            }
        }


        private void btnApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            if (radioEnglish.IsChecked == true)
                CurrentLanguage = "en-EN";
            else if (radioFrench.IsChecked == true)
                CurrentLanguage = "fr-FR";
            else if (radioSpanish.IsChecked == true)
                CurrentLanguage = "es-ES";

            UpdateLanguage(CurrentLanguage); // Use static property

            // Trigger the global LanguageChanged event
            App.OnLanguageChanged(CurrentLanguage);
        }



        private void UpdateLanguage(string cultureCode)
        {
            // Form the relative path from the executable to the JSON files
            string relativePath = $"../../../../Backend/Data/Languages/{cultureCode}.json";
            string fullPath = Path.GetFullPath(relativePath, AppDomain.CurrentDomain.BaseDirectory);

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                localizedResources = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (localizedResources != null)
                {
                    txtLanguages.Text = localizedResources["Languages"];
                    radioEnglish.Content = localizedResources["English"];
                    radioFrench.Content = localizedResources["French"];
                    radioSpanish.Content = localizedResources["Spanish"];
                    txtLogFormat.Text = localizedResources["LogFormat"];
                    btnApplyChanges.Content = localizedResources["ApplyChanges"];
                    txtGroupBoxTitle.Text = localizedResources["GroupBoxTitle"]; // Update GroupBox title
                }
            }
            else
            {
                MessageBox.Show($"Language file not found: {fullPath}");
            }
        }



    }
}
