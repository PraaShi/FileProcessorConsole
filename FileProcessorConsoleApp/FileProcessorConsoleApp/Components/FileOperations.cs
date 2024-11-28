using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace FileProcessorConsoleApp.Components
{
    internal class FileOperations
    {
        private readonly ILogger<FileOperations> _logger;

        public FileOperations(ILogger<FileOperations> logger)
        {
            _logger = logger;
        }

        public void ProcessFiles(string inputFolder, string correctFolder, string errorFolder, string sampleFolder)
        {
            _logger.LogInformation("Starting {MethodName}", nameof(ProcessFiles));
            try
            {
                string[] files = Directory.GetFiles(inputFolder);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    try
                    {
                        if (Path.GetExtension(file).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                        {
                            string content = File.ReadAllText(file);
                            _logger.LogInformation("Processing file: {FileName}", fileName);

                            string destinationPath = Path.Combine(correctFolder, fileName);
                            File.Move(file, destinationPath);
                            _logger.LogInformation("Moved to correct folder: {FileName}", fileName);
                        }
                        else
                        {
                            File.AppendAllText(sampleFolder, "Appended text using AppendAllText method.");
                            _logger.LogInformation("Content appended to sample.txt.");

                            string destinationPath = Path.Combine(errorFolder, fileName);
                            File.Move(file, destinationPath);
                            _logger.LogWarning("Moved to error folder: {FileName}", fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing file {FileName}", fileName);
                    }
                }
            }
            finally
            {
                _logger.LogInformation("Ending {MethodName}", nameof(ProcessFiles));
            }
        }

        public void RenameFile(string oldFilePath, string newFilePath)
        {
            _logger.LogInformation("Starting {MethodName}", nameof(RenameFile));
            try
            {
                if (File.Exists(oldFilePath))
                {
                    File.Move(oldFilePath, newFilePath);
                    _logger.LogInformation("File renamed from {OldFilePath} to {NewFilePath}.", oldFilePath, newFilePath);

                    string name = Path.GetFileName(newFilePath);
                    _logger.LogInformation("New file name: {FileName}", name);

                    FileInfo fileInfo = new FileInfo(newFilePath);
                    long fileSize = fileInfo.Length;
                    _logger.LogInformation("File size: {FileSize} bytes", fileSize);
                }
                else
                {
                    _logger.LogWarning("File not found: {OldFilePath}", oldFilePath);
                }
            }
            finally
            {
                _logger.LogInformation("Ending {MethodName}", nameof(RenameFile));
            }
        }

        public void DeleteFile(string filePath)
        {
            _logger.LogInformation("Starting {MethodName}", nameof(DeleteFile));
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("File deleted: {FilePath}", filePath);
                }
                else
                {
                    _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                }
            }
            finally
            {
                _logger.LogInformation("Ending {MethodName}", nameof(DeleteFile));
            }
        }

        public void ReadAndWrite(string sourceFilePath, string destinationFilePath)
        {
            _logger.LogInformation("Starting {MethodName}", nameof(ReadAndWrite));
            try
            {
                if (File.Exists(sourceFilePath))
                {
                    string content = File.ReadAllText(sourceFilePath);
                    File.WriteAllText(destinationFilePath, content);
                    _logger.LogInformation("Content copied from {SourceFilePath} to {DestinationFilePath}.", sourceFilePath, destinationFilePath);
                }
                else
                {
                    _logger.LogWarning("Source file not found: {SourceFilePath}", sourceFilePath);
                }
            }
            finally
            {
                _logger.LogInformation("Ending {MethodName}", nameof(ReadAndWrite));
            }
        }

        public string[] GetFilesByExtension(string directoryPath, string fileExtension)
        {
            _logger.LogInformation("Starting {MethodName}", nameof(GetFilesByExtension));
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath, fileExtension);
                    _logger.LogInformation("Found {FileCount} files with extension {FileExtension} in {DirectoryPath}.", files.Length, fileExtension, directoryPath);
                    return files;
                }
                else
                {
                    _logger.LogWarning("Directory not found: {DirectoryPath}", directoryPath);
                    return Array.Empty<string>();
                }
            }
            finally
            {
                _logger.LogInformation("Ending {MethodName}", nameof(GetFilesByExtension));
            }
        }

        public string CreateNewExecutionFile(string directoryPath, string baseFileName, string extension)
        {
            _logger.LogInformation("Starting {MethodName}", nameof(CreateNewExecutionFile));
            try
            {
                Directory.CreateDirectory(directoryPath);
                string[] files = Directory.GetFiles(directoryPath, $"{baseFileName}*{extension}");
                int nextNumber = 1;

                if (files.Length > 0)
                {
                    nextNumber = files
                        .Select(file =>
                        {
                            string fileName = Path.GetFileNameWithoutExtension(file);
                            string numberPart = fileName.Replace(baseFileName, "");
                            return int.TryParse(numberPart, out int num) ? num : 0;
                        })
                        .Max() + 1;
                }

                string newFileName = $"{baseFileName}{nextNumber}{extension}";
                string newFilePath = Path.Combine(directoryPath, newFileName);
                File.WriteAllText(newFilePath, $"This is the execution number {nextNumber}");
                _logger.LogInformation("Created new execution file: {NewFilePath}", newFilePath);

                return newFilePath;
            }
            finally
            {
                _logger.LogInformation("Ending {MethodName}", nameof(CreateNewExecutionFile));
            }
        }
    }
}
