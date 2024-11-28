using System;
using FileProcessorConsoleApp.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileProcessorConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependency injection and logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddTransient<FileOperations>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application started.");

            var fileOps = serviceProvider.GetRequiredService<FileOperations>();

            string baseFolder = "C:\\Users\\1000075326\\Downloads\\example";
            string inputFolder = Path.Combine(baseFolder, "Input");
            string correctFolder = Path.Combine(baseFolder, "Correct");
            string errorFolder = Path.Combine(baseFolder, "Error");
            string sampleFile = Path.Combine(baseFolder, "sample.txt");

            Directory.CreateDirectory(inputFolder);
            Directory.CreateDirectory(correctFolder);
            Directory.CreateDirectory(errorFolder);

            fileOps.ProcessFiles(inputFolder, correctFolder, errorFolder, sampleFile);

            fileOps.RenameFile(
                Path.Combine(baseFolder, "OldFileName.txt"),
                Path.Combine(baseFolder, "NewFileName.txt")
            );

            fileOps.DeleteFile(Path.Combine(baseFolder, "DeleteMe.txt"));

            fileOps.ReadAndWrite(
                Path.Combine(baseFolder, "sample.txt"),
                Path.Combine(baseFolder, "sample2.txt")
            );

            var txtFiles = fileOps.GetFilesByExtension(baseFolder, "*.txt");
            foreach (var file in txtFiles)
            {
                logger.LogInformation("Found file: {FilePath}", file);
            }

            string newFile = fileOps.CreateNewExecutionFile(baseFolder, "ExecutionFile", ".txt");
            logger.LogInformation("Created new execution file: {NewFile}");

            logger.LogInformation("Application completed.");
        }
    }
}
