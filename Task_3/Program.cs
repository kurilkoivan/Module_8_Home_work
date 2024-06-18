namespace Task_3
{
    internal class Program
    {
        //Доработайте программу из задания 1, используя ваш метод из задания 2.
        //При запуске программа должна:
        //Показать, сколько весит папка до очистки.Использовать метод из задания 2. 
        //Выполнить очистку.
        //Показать сколько файлов удалено и сколько места освобождено.
        //Показать, сколько папка весит после очистки.
        static void Main(string[] args)
        {
            string dirPath = @"D:\Programming\Skillfactory\C#_projects\Module_8\Module_8_FinalExercises\Module_8_FinalExercises\Task_3\FolderForTask3\TestFolder";
            int fileCount = 0;
            DirectoryInfo directory = new DirectoryInfo(dirPath);

            DirectorySize(directory, "Исходный размер папки: ", ref fileCount);
            int deletedFileCount = fileCount;

            if (directory.Exists)
            {
                long deletedSize = DirectoryAndFileSize(directory, ref fileCount);
                DeleteInFolder(directory);
                Console.WriteLine($"Освобождено: {deletedSize} байт. Удалено: {deletedFileCount} файлов");
                DirectorySize(directory, "Текущий размер папки: ", ref fileCount);
            }
            else
            {
                Console.WriteLine("Удаление невозможно");
            }


        }
        static void DeleteInFolder(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    if (file.Exists) file.Delete();
                    else throw new FileNotFoundException("Файла не существует");
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    if (dir.Exists) dir.Delete(true);
                    else throw new DirectoryNotFoundException("Папки не существует");
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        static void DirectorySize(DirectoryInfo directory, string message, ref int fileCount)
        {

            if (directory.Exists)
            {
                Console.WriteLine($"{message}{DirectoryAndFileSize(directory, ref fileCount)} байт");
            }
            else
            {
                Console.WriteLine("Папки не существует");
            }
        }

        static long DirectoryAndFileSize(DirectoryInfo directory, ref int fileCount)
        {
            long dirSize = 0;
            foreach (FileInfo file in directory.GetFiles())
            {
                dirSize += file.Length;
                fileCount++;
            }
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                dirSize += DirectoryAndFileSize(subDirectory, ref fileCount);
            }
            return dirSize;
        }
    }
}
