using System.Windows;
using System.Windows.Controls;
using Backend.Backup;

namespace WpfApp1
{

    /// <summary>
    /// PageTrack is a class representing the backup tracking page.
    /// </summary>
    public partial class PageTrack : Page
    {
        private BackupManager backupManager;

        /// <summary>
        /// Constructor for the PageTrack class.
        /// </summary>
        /// <param name="backupManager">The backup manager used by the page.</param>
        public PageTrack(BackupManager backupManager)
        {
            InitializeComponent();
            this.backupManager = backupManager;

            // Subscribe to the Loaded event
            this.Loaded += PageTrack_Loaded;
        }

        /// <summary>
        /// Loads the list of backups on the page.
        /// </summary>
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

        /// <summary>
        /// Updates the progress bar based on the state of the backup.
        /// </summary>
        /// <param name="backup">The relevant backup.</param>
        /// <param name="progressBar">The associated progress bar.</param>
        /// <param name="progressText">The text displaying the progress.</param>
        private void UpdateProgressBar(ABackup backup, ProgressBar progressBar, TextBlock progressText)
        {
            float progressPercentage = backup.State.Progress * 100; 
            string progressString = $"{progressPercentage:F0}%";

            Dispatcher.Invoke(() =>
            {
                progressBar.Value = progressPercentage;
                progressText.Text = progressString; 
            });
        }

        /// <summary>
        /// Event triggered when an instance of PageTrack is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageTrack_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBackups();
            lvBackups.Items.Clear();

            foreach (ABackup backup in backupManager.BackupList)
            {
                Grid grid = new Grid();
                // Define rows for different elements
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // For backup status text
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // For backup information
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // For progress bar
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // For progress text

                // Backup Status Text
                TextBlock backupStatusText = new TextBlock
                {
                    Text = "Backup not started",
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                Grid.SetColumn(backupStatusText, 0);
                Grid.SetColumnSpan(backupStatusText, 2); // Span across both columns
                Grid.SetRow(backupStatusText, 0); // Set status text to the first row

                // Backup Information
                StackPanel infoPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Left };
                TextBlock nameText = new TextBlock { Text = $"Name: {backup.Name}", FontWeight = FontWeights.Bold };
                TextBlock sourceText = new TextBlock { Text = $"Source path: {backup.SourceDirectory}" };
                TextBlock targetText = new TextBlock { Text = $"Destination path: {backup.TargetDirectory}" };
                string backupType = backup is BackupFull ? "Full" : backup is BackupDifferential ? "Differential" : "Unknown";
                TextBlock typeText = new TextBlock { Text = $"Type: {backupType}", FontWeight = FontWeights.Bold };

                infoPanel.Children.Add(nameText);
                infoPanel.Children.Add(typeText);
                infoPanel.Children.Add(sourceText);
                infoPanel.Children.Add(targetText);
                Grid.SetColumn(infoPanel, 0);
                Grid.SetRow(infoPanel, 1); // Set infoPanel to the second row

                // Buttons
                StackPanel buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
                Button startButton = new Button { Content = "▶", Width = 33, Style = (Style)Resources["ButtonStyle"] };
                Button pauseButton = new Button { Content = "⏸", Width = 33, Style = (Style)Resources["ButtonStyle"] };
                Button stopButton = new Button { Content = "■", Width = 33, Style = (Style)Resources["ButtonStyle"] };
                

                buttonPanel.Children.Add(startButton);
                buttonPanel.Children.Add(pauseButton);
                buttonPanel.Children.Add(stopButton);
                Grid.SetColumn(buttonPanel, 1); // Place buttons in the second column
                Grid.SetRow(buttonPanel, 1); // Set buttonPanel to the second row

                // ProgressBar
                ProgressBar progressBar = new ProgressBar
                {
                    Value = 0,
                    Maximum = 100,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                Grid.SetColumnSpan(progressBar, 2); // Span across both columns
                Grid.SetRow(progressBar, 2); // Set progressBar to the third row

                // Progress Text
                TextBlock progressText = new TextBlock
                {
                    Text = "0%",
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(progressText, 0);
                Grid.SetRow(progressText, 3); // Set progressText to the fourth row            

                /// Button Click Events
                startButton.Click += (s, e) =>
                {
                    Task.Run(() =>
                    {
                        backup.ResumeBackup();
                        UpdateProgressBar(backup, progressBar, progressText);
                                               
                    });
                    Dispatcher.Invoke(() => backupStatusText.Text = "Backup in progress");
                };


                pauseButton.Click += (s, e) =>
                {
                    Task.Run(() =>
                    {
                        backup.PauseBackup();
                    });
                    Dispatcher.Invoke(() => backupStatusText.Text = "Backup paused");
                };

                stopButton.Click += (s, e) =>
                {
                    Task.Run(() =>
                    {
                        backup.CancelBackup();
                    });
                    Dispatcher.Invoke(() => backupStatusText.Text = "Backup cancelled");
                };


                // ProgressUpdated Event
                backup.ProgressUpdated += (s, args) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        float progressPercentage = backup.State.Progress * 100;
                        progressBar.Value = progressPercentage;
                        progressText.Text = $"{progressPercentage:F0}%";

                        switch (backup.State.State)
                        {
                            case EnumState.InProgress:
                                backupStatusText.Text = "Backup in progress";
                                break;
                            case EnumState.Paused:
                                backupStatusText.Text = "Backup paused";
                                break;
                            case EnumState.Cancelled:
                                backupStatusText.Text = "Backup cancelled";
                                break;
                            case EnumState.Finished:
                                backupStatusText.Text = "Backup finished";
                                break;
                            case EnumState.NotStarted:
                                backupStatusText.Text = "Backup not started";
                                break;
                            case EnumState.Failed:
                                backupStatusText.Text = "Backup error";
                                break;
                            default:
                                
                                break;
                        }
                    });
                };


                // Add elements to the grid
                grid.Children.Add(backupStatusText);
                grid.Children.Add(infoPanel);
                grid.Children.Add(buttonPanel);
                grid.Children.Add(progressBar);
                grid.Children.Add(progressText);

                // Add the grid to the ListView
                lvBackups.Items.Add(grid);
            }
        }
    }
}
