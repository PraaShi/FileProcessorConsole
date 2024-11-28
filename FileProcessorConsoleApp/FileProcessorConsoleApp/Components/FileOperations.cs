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
            string[] files = Directory.GetFiles(inputFolder);
            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                try
                {
                    if (Path.GetExtension(file).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        string content = File.ReadAllText(file); // Read
                        _logger.LogInformation("Processing file: {FileName}", fileName);
                        _logger.LogDebug("File content: {Content}", content);

                        string destinationPath = Path.Combine(correctFolder, fileName);
                        File.Move(file, destinationPath);
                        _logger.LogInformation("Moved to correct folder: {FileName}", fileName);
                    }
                    else
                    {
                        File.AppendAllText(sampleFolder, "Appended text using AppendAllText method."); // Write
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

        public void RenameFile(string oldFilePath, string newFilePath)
        {
            try
            {
                if (File.Exists(oldFilePath))
                {
                    File.Move(oldFilePath, newFilePath);
                    _logger.LogInformation("File renamed from {OldFilePath} to {NewFilePath}.", oldFilePath, newFilePath);

                    string name = Path.GetFileName(newFilePath); // Get file name
                    _logger.LogInformation("New file name: {FileName}", name);

                    FileInfo fileInfo = new FileInfo(newFilePath);
                    long fileSize = fileInfo.Length; // File size
                    _logger.LogInformation("File size: {FileSize} bytes", fileSize);
                }
                else
                {
                    _logger.LogWarning("File not found: {OldFilePath}", oldFilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renaming file from {OldFilePath} to {NewFilePath}", oldFilePath, newFilePath);
            }
        }

        public void DeleteFile(string filePath)
        {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            }
        }

        public void ReadAndWrite(string sourceFilePath, string destinationFilePath)
        {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from {SourceFilePath} or writing to {DestinationFilePath}", sourceFilePath, destinationFilePath);
            }
        }

        public string[] GetFilesByExtension(string directoryPath, string fileExtension)
        {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving files from directory: {DirectoryPath}", directoryPath);
                return Array.Empty<string>();
            }
        }

        public string CreateNewExecutionFile(string directoryPath, string baseFileName, string extension)
        {
            try
            {
                Directory.CreateDirectory(directoryPath); // Ensure directory exists
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new execution file in {DirectoryPath}", directoryPath);
                return string.Empty;
            }
        }
    }
}
