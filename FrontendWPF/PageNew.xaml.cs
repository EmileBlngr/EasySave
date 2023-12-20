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
using Backend.Backup;
using Microsoft.Win32; 


namespace WpfApp1
{
    public partial class PageNew : Page
    {

        
        private Dictionary<string, string> localizedResources;
        private BackupManager backupManager;

        public PageNew()
        {
            InitializeComponent();
            UpdateLanguage(App.CurrentLanguage); // Use global language setting
            App.LanguageChanged += UpdateLanguage; // Subscribe to the global event
            backupManager = new BackupManager(); // Initialize the BackupManager

        }

        private void PageNew_Unloaded(object sender, RoutedEventArgs e)
        {
            App.LanguageChanged -= UpdateLanguage; // Unsubscribe from the global event
        }


        private void btnCreateBackup_Click(object sender, RoutedEventArgs e) // Corrected method name
        {
            string backupName = txtBackupName.Text;
            string sourceDirectory = txtSourceDirectory.Text;
            string targetDirectory = txtTargetDirectory.Text;
            string backupType = cmbBackupType.SelectedValue as string;

            if (string.IsNullOrEmpty(backupName) || string.IsNullOrEmpty(sourceDirectory) ||
                string.IsNullOrEmpty(targetDirectory) || string.IsNullOrEmpty(backupType))
            {
                MessageBox.Show("Please fill in all fields and select a backup type.");
                return;
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(backupName) || string.IsNullOrWhiteSpace(sourceDirectory) || string.IsNullOrWhiteSpace(targetDirectory))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!Directory.Exists(sourceDirectory))
            {
                MessageBox.Show("Source directory does not exist.");
                return;
            }

            if (!Directory.Exists(targetDirectory))
            {
                MessageBox.Show("Target directory does not exist.");
                return;
            }

            // Translate backup type to a type recognized by the backend
            string backendBackupType = backupType == localizedResources["totalSave"] ? "1" : "2";

            // Add and perform backup
            try
            {
                backupManager.AddBackup(backendBackupType, backupName, sourceDirectory, targetDirectory);
                backupManager.PerformAllBackups();
                MessageBox.Show("Backup created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create backup: {ex.Message}");
            }
        }
        private void BtnBrowseSource_Click(object sender, RoutedEventArgs e)
        {
            // Utilisation de OpenFileDialog pour sélectionner un fichier dans le dossier désiré
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false; // permet de sélectionner des dossiers
            openFileDialog.CheckFileExists = false; // laisse choisir des dossiers qui n'ont pas de fichiers
            openFileDialog.CheckPathExists = true; // vérifie que le chemin existe
            openFileDialog.FileName = "Dossier sélection"; // texte pour la sélection de dossier

            if (openFileDialog.ShowDialog() == true)
            {
                txtSourceDirectory.Text = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            }
        }

        private void BtnBrowseTarget_Click(object sender, RoutedEventArgs e)
        {
            // Utilisation de OpenFileDialog pour sélectionner un fichier dans le dossier désiré
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false; // permet de sélectionner des dossiers
            openFileDialog.CheckFileExists = false; // laisse choisir des dossiers qui n'ont pas de fichiers
            openFileDialog.CheckPathExists = true; // vérifie que le chemin existe
            openFileDialog.FileName = "Dossier sélection"; // texte pour la sélection de dossier

            if (openFileDialog.ShowDialog() == true)
            {
                txtTargetDirectory.Text = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            }
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
                    btnBrowseSource.Content = localizedResources["browse"];
                    sourcePath.Text = localizedResources["sourcePath"];
                    destinationPath.Text = localizedResources["destinationPath"];
                    btnBrowseTarget.Content = localizedResources["browse2"];
                    btnCreateBackup.Content = localizedResources["createSave"];

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