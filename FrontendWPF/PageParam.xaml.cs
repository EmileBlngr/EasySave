using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;



namespace WpfApp1
{
    public partial class PageParam : Page
    {
        public PageParam()
        {
            InitializeComponent(); //initialize components of my page so it can be displayed 
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Backend.Settings.Settings.GetInstance().SetBusinessSoftware(openFileDialog.FileName);

                
            }
        }
    }
}

