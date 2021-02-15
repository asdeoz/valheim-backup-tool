using System;
using Valheim_Backup_Tool.Enums;
using Valheim_Backup_Tool.Interfaces;

namespace Valheim_Backup_Tool.Services
{
    public class MenuHelper : IMenuHelper
    {
        private readonly string _defaultValheimPath;

        public MenuHelper(string defaultValheimPath)
        {
            _defaultValheimPath = defaultValheimPath;
        }

        public MenuOptions ShowMenuAndGetOption()
        {
            var exit = false;
            var selectedOption = MenuOptions.UnselectedOption;

            do
            {
                PrintMenu(_defaultValheimPath);

                var key = Console.ReadKey();

                Console.WriteLine();

                if (int.TryParse(key.KeyChar.ToString(), out var option) && Enum.IsDefined(typeof(MenuOptions), option))
                {
                    selectedOption = (MenuOptions)option;
                    exit = true;
                }
                else Console.WriteLine($"Please, select an option from the list by pressing the number of the option.{Environment.NewLine}");
            } while (!exit);

            return selectedOption;
        }

        private void PrintMenu(string defaultValheimPath)
        {
            Console.WriteLine();
            Console.WriteLine("What do you want to do? (Enter the number of the option to run)");
            Console.WriteLine("1 - Backup your Valheim files");
            Console.WriteLine("2 - Set backup location");
            Console.WriteLine("3 - Show current config values");
            Console.WriteLine($"4 - Manual set Valheim location (DEFAULT: {defaultValheimPath}");
            Console.WriteLine("5 - Reset Valheim location");
            Console.WriteLine("6 - Exit");
        }
    }
}
