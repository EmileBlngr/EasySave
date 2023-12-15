using Microsoft.Win32;
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
using Backend.Backup;


namespace WpfApp1
{
    public partial class PageParam : Page
    {
        public PageParam()
        {
            InitializeComponent(); // Ceci est essentiel pour initialiser les composants de la page
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Backend.Settings.Settings.GetInstance().SetIgnoreFile(openFileDialog.FileName);

                txtIgnoredFile.Text = Backend.Settings.Settings.GetInstance().GetIgnoredFile();
            }
        }
    }
}

