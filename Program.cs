using System;
using System.IO;
using System.Text.Json;
using System.Threading;
namespace JsonFileReader
{
    class Program
    {
        // Directory to watch
        private static readonly string inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "input");

        static void Main(string[] args)
        {
            // Create input folder if not exist
            if (!Directory.Exists(inputFolder))
            {
                Directory.CreateDirectory(inputFolder);
            }

            // Set up FileSystemWatcher to monitor the folder
            FileSystemWatcher fileWatcher = new FileSystemWatcher
            {
                Path = inputFolder,
                Filter = "*.json", // Monitor only .json files
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            // Event to handle file changes
            fileWatcher.Created += OnNewFileCreated;
            fileWatcher.EnableRaisingEvents = true;

            Console.WriteLine($"Watching for new .json files in: {inputFolder}");
            Console.WriteLine("Press 'q' to quit the application.");
            
            // Keep the application running
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    break;
            }
        }

        // Event handler for new file created in the folder
        private static void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"\nNew file detected: {e.FullPath}");

            try
            {
                // Read the JSON file
                string jsonString = File.ReadAllText(e.FullPath);

                // Deserialize the JSON file into a dynamic object
                var jsonObject = JsonSerializer.Deserialize<dynamic>(jsonString);

                // Print the content of the JSON file
                Console.WriteLine("\nFile Content:");
                Console.WriteLine(JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or parsing the file: {ex.Message}");
            }
        }
    }
}