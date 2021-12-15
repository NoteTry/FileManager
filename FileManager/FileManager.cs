using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    class FileManager
    {
        private string commandConsole;

        private string datacommand;

        private string path;

        private string dir = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("\\")) + "\\";

        private string nameFile;

        private string[] commands =
        {
            "close", "cd", "view", "copy", "info", "delete"
        };

        private FileInfo file;

        public FileManager()
        {
            Path = dir;
        }
        
        public string Path { get => path; set => path = value; }
        public string NameFile { get => nameFile; set => nameFile = value; }
        public string CommandConsole { get => commandConsole; set => commandConsole = value; }
        public string Datacommand { get => datacommand; set => datacommand = value; }

        public void start()
        {
            Console.Write($"FileManager: \"{Path}\" > ");
            GetCommandAndData(Console.ReadLine().ToString()); 
        }

        public void GetCommandAndData(string value)
        {
            if (value != "" && value != null)
            {
                CommandConsole = value;

                if (value.Contains(' '))
                {
                    CommandConsole = value.Substring(0, value.IndexOf(" "));
                    datacommand = value.Substring(value.IndexOf(" ")).Trim(' ');
                }

                if (isCommand())
                {
                    switch(commandConsole)
                    {
                        case "cd":
                                cd();
                            break;
                        case "view":
                                view();
                            break;
                        case "info":
                                info();
                            break;
                        case "copy":
                                copy();
                            break;
                        case "close":
                                close();
                            break;
                        case "delete":
                                delete();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Нет такой команды!");
                }


            }
        }

        private bool isCommand()
        {
            foreach (string item in commands)
            {
                if (CommandConsole == item)
                {
                    return true;
                }
            }

            return false;
        }
        /*
         * Просмотр Папок и Файлов
         */
        private void view()
        {
            List<string> ls = GetRecursFiles(Path);
            foreach (string fname in ls)
            {
                Console.WriteLine(fname);
            }
        }

        private List<string> GetRecursFiles(string start_path, int lenght = 0)
        {
            List<string> ls = new List<string>();
            try
            {
                getFolder(ls, start_path, lenght);
                getFile(ls, start_path, lenght);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ls;
        }

        private void getFolder(List<string> ls, string start_path, int lenght)
        {
            string[] folders = Directory.GetDirectories(start_path);
            foreach (string folder in folders)
            {
                var folderData = Directory.CreateDirectory(folder);    

                ls.Add(partStringDouble(lenght, "-> " + folderData.Name));
                
                if (lenght != 3)
                {
                    ls.AddRange(GetRecursFiles(folder, lenght + 1));
                }
            }
        }

        private void getFile(List<string> ls, string start_path, int lenght)
        {
            string[] files = Directory.GetFiles(start_path);
            foreach (string filename in files)
            {
                var dataFile = new FileInfo(filename);
                ls.Add(partStringDouble(lenght, "-> " + dataFile.Name));
            }
        }

        private string partStringDouble(int num, string str = null)
        {
            string strData = str;
            
            for (int i = 0; i < num; i++)
            {
                strData = string.Format("\t {0}", strData);
            }

            return strData;
        }

        /*
         * Переход по дерикториям
         */

        private void cd()
        {
            if (datacommand != "")
            {
                if (File.Exists(datacommand) || Directory.Exists(datacommand))
                {
                    Path = datacommand;
                }
                else
                {
                    Console.WriteLine("Файл или каталог не был обнаружен!");
                }
            }
        }
        /*
         * Копирование
         */
        private void copy()
        {
            if (datacommand != "" && isСonditions(datacommand))
            {
                if (Directory.CreateDirectory(Path).Exists) {

                    Console.WriteLine($"\tName: {getNameCopy(datacommand)}\n\tPath: {getPathCopy(datacommand)}");

                    if (getNameCopy(datacommand) != "" && Directory.CreateDirectory(getPathCopy(datacommand)).Exists)
                    {
                        
                        try
                        {
                            Console.Write($"\tКопировавть файл {getNameCopy(datacommand)} в папку {getPathCopy(datacommand)}?(y/n) - ");
                            char com = char.Parse(Console.ReadLine());
                            if(com == 'y')
                                File.Copy($"{Path}\\{getNameCopy(datacommand)}", $"{getPathCopy(datacommand)}{getNameCopy(datacommand)}");
                        }
                        catch (IOException copyError)
                        {
                            Console.WriteLine(copyError.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Нету именни или неправельный путь.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Неправельный путь к основному каталогу.");
                }
            }
            else
            {
                Console.WriteLine("Error: Неправельный синтаксис команды!");
            }
        }

        private bool isСonditions(string value)
        {
            return ((value.Substring(value.IndexOf("-f") + 2, 1) == " ") && (value.Substring(value.IndexOf("-p") + 2, 1) == " ") && (value.Substring(value.IndexOf("-p") - 1, 1) == " ")) ? true : false;
        }

        private string getNameCopy(string value)
        {
            return value.Substring(value.IndexOf("-f") + 2, value.IndexOf(" -p") - 2).Trim(' ');
        }

        private string getPathCopy(string value)
        {
            return value.Substring(value.IndexOf("-p") + 2).Trim(' ');
        }

        /*
         * Вывад информации
         */
        private void info()
        {
            if (File.Exists(Path))
            {
                var fi1 = new FileInfo(Path);
                Console.WriteLine($" Byte: {fi1.Length} \n Name: {fi1.Name} \n Creation time: {fi1.CreationTime} \n ");

            }
            
            if(Directory.CreateDirectory(Path).Exists)
            {
                var fi1 = Directory.CreateDirectory(Path);
                Console.WriteLine($" Name: {fi1.Name} \n Creation time: {fi1.CreationTime}");
            }
        }

        private void delete()
        {
            if (datacommand != "" && datacommand != dir)
            {
                
                try
                {
                    Console.Write($"\tУдалить файл или папку?(y/n) - ");
                    char com = char.Parse(Console.ReadLine());

                    if (com == 'y')
                        deleteFileOrFolders(datacommand.Trim());

                    Console.WriteLine("\tУдалено!");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }

                
            }
            else
            {
                Console.WriteLine($"Error: Пустая строка или основная деректория {datacommand}!");
            }
        }

        private void deleteFileOrFolders(string value)
        {
            if (File.Exists(value))
            {

                File.Delete(value.Trim());
            }

            if (Directory.CreateDirectory(value).Exists)
            {
                Directory.Delete(datacommand.Trim());
            }
        }

        private void close()
        {
            Environment.Exit(0);
        }
    }
}
