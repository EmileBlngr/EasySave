using System;

namespace Backend.Model.Backup
{
    public interface IBackup
    {
        public void UpdateProgress();
        public void ScanFiles();
        public void PerformBackup();
    }
}
