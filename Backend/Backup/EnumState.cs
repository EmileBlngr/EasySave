namespace Backend.Backup
{
    /// <summary>
    /// Enumerates the possible states of a backup operation.
    /// </summary>
    public enum EnumState
    {
        Paused,
        Cancelled,
        Finished,
        InProgress,
        Failed,
        NotStarted
    }
}
