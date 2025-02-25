using System;
using System.IO;
using System.IO.Compression;
using System.Globalization;

class Program
{
    static void Main()
    {
        string baseDir = BASE_DIRECTORY;

        // Get last month’s name and year
        DateTime lastMonth = DateTime.Now.AddMonths(-1);
        Console.WriteLine(lastMonth);
        string monthYear = lastMonth.ToString("MMyyyy");
        string folderName = $"{monthYear}Parkland";
        string folderPath = Path.Combine(baseDir, folderName);
        string zipFilePath = Path.Combine(baseDir, $"{folderName}.zip");

        try
        {
            // Create the folder
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Get all matching files
            string[] files = Directory.GetFiles(baseDir, "PRK*");

            // Move files if they match last month’s pattern
            bool filesMoved = false;
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (fileName.Contains(monthYear))
                {
                    string destination = Path.Combine(folderPath, fileName);
                    File.Move(file, destination);
                    filesMoved = true;
                }
            }

            // If no files were moved, delete the empty folder and exit
            if (!filesMoved)
            {
                Console.WriteLine($"No files found for {monthYear}. Skipping.");
                Directory.Delete(folderPath);
                return;
            }

            // Zip the folder
            ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            // Delete the original folder after zipping
            Directory.Delete(folderPath, true);

            Console.WriteLine($"Successfully archived files into {zipFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}