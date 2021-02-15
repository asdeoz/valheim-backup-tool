namespace Valheim_Backup_Tool.Interfaces
{
    public interface IBackupService
    {
        void CreateOrUpdateConfigFile();
        void PrintCurrentConfiguration();
        void SetValheimLocationConfiguration();
        void ResetValheimLocationConfiguration();
        void BackupFiles();
    }
}