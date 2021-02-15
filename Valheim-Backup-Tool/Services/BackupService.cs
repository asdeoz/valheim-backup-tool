using Newtonsoft.Json;
using System;
using System.IO;
using Valheim_Backup_Tool.Interfaces;
using Valheim_Backup_Tool.Models;

namespace Valheim_Backup_Tool.Services
{
    public class BackupService : IBackupService
    {
        private const string CONFIG_FILE_NAME = "valheim-backup-tool.config";
        private readonly string _defaultValheimLocation;

        public BackupService(string defaultValheimLocation)
        {
            _defaultValheimLocation = defaultValheimLocation;
        }

        public void CreateOrUpdateConfigFile()
        {
            try
            {
                Console.WriteLine("Please enter the path where you want to backup your files:");
                var path = Console.ReadLine();

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Sorry, this path doesn't exist. Try again.");
                    return;
                }

                var config = GetCurrentConfiguration();
                config.BackupLocation = Path.GetFullPath(path);

                SaveConfiguration(config);
            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex);
            }
        }

        public void PrintCurrentConfiguration()
        {
            try
            {
                var config = GetCurrentConfiguration();
                Console.WriteLine($"The Valheim location is set to [{config.ValheimLocation}]");
                Console.WriteLine($"The backup location is set to [{config.BackupLocation}]");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex);
            }
        }

        public void SetValheimLocationConfiguration()
        {
            try
            {
                var config = GetCurrentConfiguration();
                Console.WriteLine($"The current Valheim location is set to [{config.ValheimLocation}]");
                Console.WriteLine("It is NOT recommended to change this from the default value.");
                Console.WriteLine("Do you still want to continue? [y/N]");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Y) return;

                Console.WriteLine("Please enter the path where your Valheim files are:");
                var path = Console.ReadLine();

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Sorry, this path doesn't exist. Try again.");
                    return;
                }

                config.ValheimLocation = Path.GetFullPath(path);

                SaveConfiguration(config);
            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex);
            }
        }

        public void ResetValheimLocationConfiguration()
        {
            try
            {
                var config = GetCurrentConfiguration();
                config.ValheimLocation = _defaultValheimLocation;
                SaveConfiguration(config);
            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex);
            }
        }

        public void BackupFiles()
        {
            try
            {
                var config = GetCurrentConfiguration();
                if (!Directory.Exists(config.ValheimLocation))
                {
                    Console.WriteLine("Your Valheim location doesn't exist, try to reset it to the default value.");
                    return;
                }

                if (!Directory.Exists(config.BackupLocation))
                {
                    Console.WriteLine("Your backup location doesn't exist, try to set it before running the backup option.");
                    return;
                }

                var datestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                var backupLocation = Path.Combine(config.BackupLocation, datestamp);

                if (Directory.Exists(backupLocation))
                {
                    Console.WriteLine("This backup location already exists, exiting.");
                    return;
                }

                CopyFiles(config.ValheimLocation, backupLocation);
                Console.WriteLine();
                Console.WriteLine($"All your files were backed up in [{backupLocation}]");
            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex);
            }
        }

        #region Private Helpers
        private Configuration GetCurrentConfiguration()
        {
            var configFilePath = GetConfigFilePath();
            Configuration config;

            if (File.Exists(configFilePath))
            {
                var configFileContent = File.ReadAllText(configFilePath);
                config = JsonConvert.DeserializeObject<Configuration>(configFileContent);
            }
            else
            {
                config = new Configuration
                {
                    ValheimLocation = _defaultValheimLocation
                };
            }

            return config;
        }

        private void SaveConfiguration(Configuration config)
        {
            var configFilePath = GetConfigFilePath();
            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config));
            Console.WriteLine($"Config file saved correctly at [{configFilePath}]");
        }

        private string GetConfigFilePath()
        {
            var current = Directory.GetCurrentDirectory();
            return Path.Combine(current, CONFIG_FILE_NAME);
        }

        private void PrintErrorMessage(Exception ex)
        {
            Console.WriteLine($"Something went wrong trying to create the config file.");
            Console.WriteLine($"Error Message: {ex.Message}");
            Console.WriteLine();
        }

        private void CopyFiles(string origin, string destination)
        {
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            var files = Directory.GetFiles(origin);
            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file);
                var fileDestination = Path.Combine(destination, fileName);
                File.Copy(file, fileDestination, true);
            }

            var directories = Directory.GetDirectories(origin);
            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);
                var directoryDestination = Path.Combine(destination, directoryName);
                CopyFiles(directory, directoryDestination);
            }
        }
        #endregion
    }
}
