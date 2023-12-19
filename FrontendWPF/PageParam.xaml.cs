using System.IO;
using System.Windows;
using System.Windows.Controls;
using Backend.Settings;
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
        public bool IsJsonChecked { get; set; }
        public bool IsTxtChecked { get; set; }
        public bool IsXmlChecked { get; set; }
        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set { currentLanguage = value; }
        }

        public PageParam()
        {
            DataContext = this;

            IsJsonChecked = Settings.GetInstance().LogSettings.WriteToJson;
            IsTxtChecked = Settings.GetInstance().LogSettings.WriteToTxt;
            IsXmlChecked = Settings.GetInstance().LogSettings.WriteToXml;

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
            txtMaxFileSizeInput.Text = Settings.GetInstance().MaxParallelTransferSizeKB.ToString();
            txtBusinessSoftwareInput.Text = Settings.GetInstance().GetBusinessSoftware();
            foreach (string extensionToEncrypt in Settings.GetInstance().ExtensionsToEncrypt)

            {
                listEncryptFileExtensions.Items.Add(extensionToEncrypt);
            }

            foreach (string PrioritaryExtension in Settings.GetInstance().PriorityExtensionsToBackup)
            {
                listPrioritaryFileExtensions.Items.Add(PrioritaryExtension);
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
            if (int.TryParse(txtMaxFileSizeInput.Text, out int maxFileSize))
            {
                Settings.GetInstance().SetMaxParallelTransferSizeKB(maxFileSize);
            }

            Settings.GetInstance().SetBusinessSoftware(txtBusinessSoftwareInput.Text);

            Settings.GetInstance().LogSettings.WriteToJson = IsJsonChecked;
            Settings.GetInstance().LogSettings.WriteToTxt = IsTxtChecked;
            Settings.GetInstance().LogSettings.WriteToXml = IsXmlChecked;

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

        private void btnAddEncryptExtension_Click(object sender, RoutedEventArgs e)
        {
            string newExtension = txtEncryptFileExtensionInput.Text.Trim();
            if (!string.IsNullOrEmpty(newExtension))
            {
                listEncryptFileExtensions.Items.Add(newExtension);
                Settings.GetInstance().AddExtensionsToEncrypt(newExtension);
                txtEncryptFileExtensionInput.Clear();
            }
        }

        private void btnRemoveEncryptExtension_Click(object sender, RoutedEventArgs e)
        {
            if (listEncryptFileExtensions.SelectedIndex != -1)
            {
                Settings.GetInstance().RemoveExtensionToEncrypt(listEncryptFileExtensions.SelectedItem.ToString());
                listEncryptFileExtensions.Items.RemoveAt(listEncryptFileExtensions.SelectedIndex);
            }
        }
        private void btnAddPrioritaryExtension_Click(object sender, RoutedEventArgs e)
        {
            string newExtension = txtPrioritaryFileExtensionInput.Text.Trim();
            if (!string.IsNullOrEmpty(newExtension))
            {
                listPrioritaryFileExtensions.Items.Add(newExtension);
                Settings.GetInstance().AddPriorityExtensionToBackup(newExtension);
                txtPrioritaryFileExtensionInput.Clear();
            }
        }

        private void btnRemovePrioritaryExtension_Click(object sender, RoutedEventArgs e)
        {
            if (listPrioritaryFileExtensions.SelectedIndex != -1)
            {
                Settings.GetInstance().RemovePriorityExtensionToBackup(listPrioritaryFileExtensions.SelectedItem.ToString());
                listPrioritaryFileExtensions.Items.RemoveAt(listPrioritaryFileExtensions.SelectedIndex);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.Content.ToString() == ".JSON")
                    IsJsonChecked = true;
                else if (checkBox.Content.ToString() == ".TXT")
                    IsTxtChecked = true;
                else if (checkBox.Content.ToString() == ".XML")
                    IsXmlChecked = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.Content.ToString() == ".JSON")
                    IsJsonChecked = false;
                else if (checkBox.Content.ToString() == ".TXT")
                    IsTxtChecked = false;
                else if (checkBox.Content.ToString() == ".XML")
                    IsXmlChecked = false;
            }
        }

    }
}
