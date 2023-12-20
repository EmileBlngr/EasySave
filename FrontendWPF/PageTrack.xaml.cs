using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Backend.Backup;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    public partial class PageTrack : Page
    {
        private BackupManager backupManager;

        public PageTrack(BackupManager backupManager)
        {
            InitializeComponent();
            this.backupManager = backupManager;

            // Subscribe to the Loaded event
            this.Loaded += PageTrack_Loaded;
        }
        private void LoadBackups()
        {
            lvBackups.Items.Clear();

            if (backupManager.BackupList.Count == 0)
            {
                // No backups, show the message
                txtNoBackups.Visibility = Visibility.Visible;
            }
            else
            {
                // There are backups, hide the message and list them
                txtNoBackups.Visibility = Visibility.Collapsed;

                foreach (var backup in backupManager.BackupList)
                {
                    StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal };
                    TextBlock nameText = new TextBlock { Text = backup.Name, Width = 100 };
                    ProgressBar progressBar = new ProgressBar { Width = 100 };
                    // Configure progress bar, buttons, etc.

                    // Add panel to ListView
                    lvBackups.Items.Add(panel);
                }
            }
        }


        private void PageTrack_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBackups();
            lvBackups.Items.Clear();

            foreach (ABackup backup in backupManager.BackupList)
            {
                StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal };
                TextBlock nameText = new TextBlock { Text = backup.Name, Width = 100 };
                TextBlock stateText = new TextBlock { Text = backup.State.State.ToString(), Width = 100 };
                TextBlock progressText = new TextBlock { Text = backup.Progress.ToString(), Width = 100 };
                ProgressBar progressBar = new ProgressBar { Width = 100, Minimum = 0, Maximum = 1};
                Binding progressBinding = new Binding("Progress")
                {
                    Source = backup,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                progressBar.SetBinding(ProgressBar.ValueProperty, progressBinding);
                Button startButton = new Button { Content = "▶", Width = 33 };
                Button pauseButton = new Button { Content = "⏸", Width = 33 };
                Button stopButton = new Button { Content = "■", Width = 33 };   



                startButton.Click += (s, e) => Task.Run(() => backup.ResumeBackup());
                pauseButton.Click += (s, e) => Task.Run(() => backup.PauseBackup());
                stopButton.Click += (s, e) => Task.Run(() => backup.CancelBackup());


                panel.Children.Add(nameText);
                panel.Children.Add(progressBar);
                panel.Children.Add(startButton);
                panel.Children.Add(pauseButton);
                panel.Children.Add(stopButton);
                panel.Children.Add(stateText);
                panel.Children.Add(progressText);
                

                lvBackups.Items.Add(panel);
            }
        }

    }
}
