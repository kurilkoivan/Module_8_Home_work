using System.Reflection.PortableExecutable;
using System.Text;
using Task_4.Modules;

namespace Task_4
{
    internal class Program
    {
        /*
         * Написать программу-загрузчик данных из бинарного формата в текст.

         На вход программа получает бинарный файл, предположительно, это база данных студентов.

            Свойства сущности Student:

            Имя — Name (string);
            Группа — Group (string);
            Дата рождения — DateOfBirth (DateTime).
            Средний балл — (decimal).
            Ваша программа должна:

            Cчитать данные о студентах из файла;
            Создать на рабочем столе директорию Students.
            Внутри раскидать всех студентов из файла по группам (каждая группа-отдельный текстовый файл), в файле группы студенты перечислены построчно в формате "Имя, дата рождения, средний балл".
        */

        static void Main(string[] args)
        {     
            string filePath = @"D:\Programming\Skillfactory\C#_projects\Module_8_FinalExercises\Module_8_FinalExercises\Task_4\DataFolder\students.dat";
            string filePathForFolderStudents = @"C:\Users\evgen\OneDrive\Рабочий стол\Students";

            Console.WriteLine("Для записи всех студентов нажмите 'ENTER'");
            Console.ReadLine();
            WriteStudentsInFile(FillStudenstList(), filePath);

            Console.Clear();
            Console.WriteLine("Данные успешно записаны");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Чтобы посмотреть всех студентов нажмите 'ENTER'");
            Console.ReadLine();
            Console.Clear();

            List<Student> studentsToRead = ShowAllStudentsList(filePath);
            
            foreach(Student student in studentsToRead)
            {
                Console.WriteLine($"Имя: {student.Name}\tГруппа: {student.Group}\t\tДата рождения: {student.DateOfBirth.ToString("dd.MM.yyyy")}\t Средний балл: {student.AverageScore}");
            }
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Чтобы создать папку 'Students' нажмите 'ENTER'");
            Console.ReadLine();
            Console.Clear();
            FolderStudentsCreate(filePathForFolderStudents);
            Console.ReadLine();
            Console.Clear();          
            Console.WriteLine("Чтобы распределить студентов по группам нажмите 'ENTER'");
            Console.ReadLine();
            SelectGroups(studentsToRead);
            Console.Clear();
            Console.WriteLine("Все студенты успешно распределены по группам");
            Console.ReadLine();
        }

        static List<Student> FillStudenstList()
        {
            
            List<Student> students = new List<Student>()
            {
                new Student{Name="Иванов Иван", Group="Группа1", DateOfBirth=new DateTime(2007,01,23), AverageScore=4.6M},
                new Student{Name="Олег Олегов", Group="Группа2", DateOfBirth=new DateTime(2006,05,15), AverageScore=3.1M},
                new Student{Name="Игорь Игорев", Group="Группа3", DateOfBirth=new DateTime(2006,10,01), AverageScore=3.9M},
                new Student{Name="Алена Аленова", Group="Группа1", DateOfBirth=new DateTime(2007,12,14), AverageScore=4.1M},
                new Student{Name="Ирина Иринова", Group="Группа2", DateOfBirth=new DateTime(2006,04,21), AverageScore=3.2M},
                new Student{Name="Анна Аннова", Group="Группа3", DateOfBirth=new DateTime(2007,02,03), AverageScore=4.0M},
                new Student{Name="Николай Николаев", Group="Группа1", DateOfBirth=new DateTime(2006,07,18), AverageScore=4.9M},
                new Student{Name="Надежда Надеждина", Group="Группа2", DateOfBirth=new DateTime(2007,11,27), AverageScore=3.4M},
                new Student{Name="Алексей Алексеев", Group="Группа3", DateOfBirth=new DateTime(2007,08,01), AverageScore=3.3M},
            };
            return students;
        }

        static void WriteStudentsInFile(List<Student>students,string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(students.Count);

                    foreach (Student student in students)
                    {
                        bw.Write(student.Name);
                        bw.Write(student.Group);
                        bw.Write(student.DateOfBirth.ToBinary());
                        bw.Write(student.AverageScore);
                    }
                }
            }
        }

        static List<Student> ShowAllStudentsList(string filePath)
        {
            if (File.Exists(filePath))
            {
                List<Student> result = new();

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader binaryReader = new BinaryReader(fs))
                    {
                        int count = binaryReader.ReadInt32();

                        for (int i = 0; i < count; i++)
                        {
                            Student student = new Student();
                            student.Name = binaryReader.ReadString();
                            student.Group = binaryReader.ReadString();
                            student.DateOfBirth = DateTime.FromBinary(binaryReader.ReadInt64());
                            student.AverageScore = binaryReader.ReadDecimal();

                            result.Add(student);
                        } 
                    }
                }
                   
                return result;
            }
            else
            {
                return new List<Student>();
            }
            
        }

        static void FolderStudentsCreate(string filePathForFolderStudents)
        {
            DirectoryInfo dirStudents = new DirectoryInfo(filePathForFolderStudents);

            if (!dirStudents.Exists)
            {
                dirStudents.Create();
                Console.WriteLine($"Папка {dirStudents.Name} успешно создана на Рабочем столе");
            }
            else
            {
                Console.WriteLine($"Папка {dirStudents.Name} уже существует на Рабочем столе");
            }
        }

        static void FilesInFolderCreate(ref List<FileInfo> files)
        {
            files=new List<FileInfo>()
            {
            new FileInfo(@"C:\Users\evgen\OneDrive\Рабочий стол\Students\Группа1.txt"),
            new FileInfo(@"C:\Users\evgen\OneDrive\Рабочий стол\Students\Группа2.txt"),
            new FileInfo(@"C:\Users\evgen\OneDrive\Рабочий стол\Students\Группа3.txt"),
            };

            foreach (FileInfo file in files)
            {
                FileCreate(file);
            }

        }
        static void FileCreate(FileInfo file)
        {
            if (!file.Exists)
            {
                file.Create();
                Console.WriteLine($"Файл {file.Name} успешно создан в папке Students");
            }
            else
            {
                Console.WriteLine($"Файл {file.Name} уже существует в папке Students");
            }
        }

        static void WriteToFile(string group, List<Student> students)
        {
            using (FileStream fileStream = new FileStream($"C:\\Users\\evgen\\OneDrive\\Рабочий стол\\Students\\{group}.txt", FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    
                    foreach (var student in students.Where(student => student.Group == group))
                    {
                        string studentInfo = $"Имя: {student.Name}\t\t\tДата рождения: {student.DateOfBirth.ToString("dd.MM.yyyy")}\t\t\tСредний балл: {student.AverageScore}\n";
                        binaryWriter.Write(studentInfo);
                    }
                }
            }
        }

        static void SelectGroups(List<Student> students)
        {
            WriteToFile("Группа1", students);
            WriteToFile("Группа2", students);
            WriteToFile("Группа3", students);
        }     
    }
}
