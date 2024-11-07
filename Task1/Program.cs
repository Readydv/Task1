using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке для очистки:");
        string folderPath = Console.ReadLine();

        if (string.IsNullOrEmpty(folderPath))
        {
            Console.WriteLine("Некорректный путь. Пожалуйста, укажите путь к папке.");
            return;
        }

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Указанная папка не существует.");
                return;
            }

            CleanOldFilesAndFolders(folderPath);
            Console.WriteLine("Очистка завершена.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Ошибка доступа: нет прав для доступа к указанной папке.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static void CleanOldFilesAndFolders(string folderPath)
    {
        TimeSpan expirationTime = TimeSpan.FromMinutes(30);
        DateTime currentTime = DateTime.Now;

        // Удаляем файлы
        foreach (var file in Directory.GetFiles(folderPath))
        {
            DateTime lastAccessTime = File.GetLastAccessTime(file);

            if (currentTime - lastAccessTime > expirationTime)
            {
                File.Delete(file);
                Console.WriteLine($"Удален файл: {file}");
            }
        }

        // Удаляем директории
        foreach (var directory in Directory.GetDirectories(folderPath))
        {
            DateTime lastAccessTime = Directory.GetLastAccessTime(directory);

            if (currentTime - lastAccessTime > expirationTime)
            {
                Directory.Delete(directory, true);
                Console.WriteLine($"Удалена папка: {directory}");
            }
        }
    }
}
