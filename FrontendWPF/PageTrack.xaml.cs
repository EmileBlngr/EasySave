using System.Windows;
using System.Windows.Controls;
using Backend.Backup;

namespace WpfApp1
{
    public partial class PageTrack : Page
    {
        private BackupManager backupManager;

        public PageTrack()
        {
            InitializeComponent();
            backupManager = new BackupManager(); // Assume this is already populated with backups
        }

        private void PageTrack_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var backup in backupManager.BackupList)
            {
                StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
                TextBlock nameText = new TextBlock() { Text = backup.Name, Width = 100 };
                ProgressBar progressBar = new ProgressBar() { Value = backup.State.Progress, Width = 100 };
                Button startButton = new Button() { Content = "▶", Width = 33 };
                Button pauseButton = new Button() { Content = "⏸", Width = 33 };
                Button stopButton = new Button() { Content = "■", Width = 33 };
                Button logButton = new Button() { Content = "📃", Width = 33 };

                // Set click events for each button
                startButton.Click += (s, e) => backup.PerformBackup();
                pauseButton.Click += (s, e) => backup.PauseBackup();
                stopButton.Click += (s, e) => backup.CancelBackup();
                // logButton.Click event needs to be defined to show the log.

                panel.Children.Add(nameText);
                panel.Children.Add(progressBar);
                panel.Children.Add(startButton);
                panel.Children.Add(pauseButton);
                panel.Children.Add(stopButton);
                panel.Children.Add(logButton);

              //lvBackups.Items.Add(new ListViewItem() { Content = panel });
            }
        }
    }
}
