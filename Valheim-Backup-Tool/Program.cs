using System;
using Valheim_Backup_Tool.Enums;
using Valheim_Backup_Tool.Services;

namespace Valheim_Backup_Tool
{
    class Program
    {
        static void Main()
        {
            var pathFinder = new PathFinder();
            var valheimDefault = pathFinder.GetValheimDefaultFolderPath();

            MenuOptions option;

            var menuHelper = new MenuHelper(valheimDefault);
            var backupService = new BackupService(valheimDefault);

            Console.WriteLine("Hello to the Valheim Backup Tool!");

            do
            {
                option = menuHelper.ShowMenuAndGetOption();

                switch (option)
                {
                    case MenuOptions.BackupFiles:
                        backupService.BackupFiles();
                        break;
                    case MenuOptions.SetBackupLocation:
                        backupService.CreateOrUpdateConfigFile();
                        break;
                    case MenuOptions.ShowCurrentConfig:
                        backupService.PrintCurrentConfiguration();
                        break;
                    case MenuOptions.SetValheimLocation:
                        backupService.SetValheimLocationConfiguration();
                        break;
                    case MenuOptions.ResetValheimLocation:
                        backupService.ResetValheimLocationConfiguration();
                        break;
                    case MenuOptions.UnselectedOption:
                    case MenuOptions.Exit:
                    default:
                        break;
                }

                Console.WriteLine();
            } while (option != MenuOptions.Exit);

            Console.WriteLine("Thank you for using the Valheim Backup Tool! :)");
        }
    }
}
