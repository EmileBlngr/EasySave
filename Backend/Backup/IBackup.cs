using System;

namespace Backend.Backup
{
    public interface IBackup
    {
        public void UpdateProgress();
        public void ScanFiles();
        public void PerformBackup();
    }
}
