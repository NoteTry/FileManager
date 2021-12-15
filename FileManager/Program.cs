using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileManager
{
    class Program
    {
        static void infoCommand()
        {
            PrintConsol printConsol = new PrintConsol(100);

            
            Console.WriteLine(printConsol.GetHashCode());
            ArrayList arlist = new ArrayList();

            arlist.Add(new string[] { "cd Path", "Переход по директориям." });
            arlist.Add(new string[] { "view", "Вывод папок и файлов."});
            arlist.Add(new string[] { "copy -f Name -p Path", "Копирование файлов, папок. (copy -f file.txt -p C:\\...)" });
            arlist.Add(new string[] { "info", "Вывод информации о файле или папке." });
            arlist.Add(new string[] { "delete (Путь к файлу или каталогу)", "Удалить файл или каталог." });
            arlist.Add(new string[] { "close", "Завершит работу." });

            Console.Clear();
            printConsol.PrintLine();

            foreach (string[] item in arlist)
            {
                printConsol.PrintRow(item[0], item[1]);
            }

            printConsol.PrintLine();
            
        }

        static void Main(string[] args)
        {
            FileManager manager = new FileManager();

            infoCommand();

            try
            {
                while (true)
                {
                    manager.start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Fatal - {e.Message}");
            }

            Console.ReadKey();
        }
    }
}
