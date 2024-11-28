// See https://aka.ms/new-console-template for more information
using System;
using System.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FileProcessorConsoleApp
{
    class Program
    {
        private static ILogger logger;
        static void Main(string[] args)
        {

           string baseFolder = ConfigurationManager.AppSettings["BaseFolder"];

            string inputFolder = Path.Combine(baseFolder, "Input");
            string correctFolder = Path.Combine(baseFolder, "Correct");
            string errorFolder = Path.Combine(baseFolder, "Error");
            string sampleFolder = Path.Combine(baseFolder, "sample.txt");

            Directory.CreateDirectory(inputFolder); // Checking weather the input directory exist
            Directory.CreateDirectory(correctFolder); //Checking wheather the destination directory exist
            Directory.CreateDirectory(errorFolder);

            Console.WriteLine("File Processing Processed");

            ProcessFiles(inputFolder, correctFolder, errorFolder,sampleFolder);

            //---------Renaming
            RenameTheFileName();

            //--------Deleting
            DeleteFile();

            //--------Read and write
            ReadAndWrite();

            //---------Get all the files with specific location
            string fileExtension = "*.txt";
            string path = "C:\\Users\\1000075326\\Downloads\\example";

            string[] files = Directory.GetFiles(path,fileExtension);
            Console.WriteLine("Below is the list of text files");
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }

            Console.WriteLine("ENTER YEAR:");
            string yearInput = Console.ReadLine();

            Console.WriteLine("ENTER Month:");
            string monthInput = Console.ReadLine();

            try
            {
                if(int.TryParse(yearInput,out int year) && int.TryParse(monthInput, out int month))
                {
                    string result = GetLastDateOfMonth(year, month);
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("INVALID INPUT");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured");
            }

            //Creating a new file and Incrementing on every iteration
            string DirectoryPath = "C:\\Users\\1000075326\\Downloads\\example";
            string BaseFileName = "ExecutionFile";
            string Extension = ".txt";

            try
            {
                Directory.CreateDirectory(DirectoryPath);
                string newFilePath = CreateNewExecutionFile(DirectoryPath, BaseFileName, Extension);
                Console.WriteLine(newFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Completed");
            Console.ReadKey();
     }

        static void ProcessFiles(string inputFolder, string correctFolder, string errorFolder, string sampleFolder)
        {
            string[] files = Directory.GetFiles(inputFolder);
            foreach (var file in files) 
            {
                string fileName = Path.GetFileName(file);
                try
                {
                    if (Path.GetExtension(file).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        string content = File.ReadAllText(file); //Read
                        Console.WriteLine("Here is the content in the file");
                        Console.WriteLine(content);
                        string destinationPath = Path.Combine(correctFolder, fileName);
                        File.Move(file, destinationPath);
                        Console.WriteLine($"Moved to correct folder : {fileName}");
                        Console.WriteLine("the file name is {}");
                        
                    }
                    else
                    {
                        File.AppendAllText(sampleFolder,"this is the text that is appended by AppendAllText method"); //write
                        Console.WriteLine("the content is loaded in the sample.txt file");
                        string destinationPath = Path.Combine(errorFolder, fileName);
                        File.Move(file, destinationPath);
                        Console.WriteLine($"Moved to error folder : {fileName}");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
        }

        static string GetLastDateOfMonth(int year,int month)
        {
            if(year > 0 && month >=1 && month <= 12)
            {
                int lastDay = DateTime.DaysInMonth(year, month);
                DateTime lastDate = new DateTime(year, month, lastDay);
                return $"the last date of the month is {lastDate}";
            }
            else
            {
                return "Invalid month or year";
            }
        }

        static string CreateNewExecutionFile(string directoryPath, string baseFileName,string extension)
        {
            string[] files = Directory.GetFiles(directoryPath,$"{baseFileName}*{extension}");
            int nextNumber = 1;
            
            if(files.Length > 0) 
            {
                nextNumber = files.Select
                    (file =>
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string numberPart = fileName.Replace(baseFileName, "");
                        return int.TryParse(numberPart,out int num) ? num : 0;
                    })
                    .Max() + 1;
            }
            string newFileName = $"{baseFileName}{nextNumber}{extension}";
            string newFilePath = Path.Combine(directoryPath, newFileName);
            File.WriteAllText(newFilePath, $"This is the execution number {nextNumber}");
            return newFilePath;
        }

        static void RenameTheFileName()
        {
            string OldFilePath = @"C:\Users\1000075326\Downloads\example\OldFileName.txt";
            string NewFilePath = @"C:\Users\1000075326\Downloads\example\NewFileName.txt";

            if (File.Exists(OldFilePath))
            {
                File.Move(OldFilePath, NewFilePath);
                Console.WriteLine("File Renaming Successful");
            }


            string name = Path.GetFileName(NewFilePath); //Getting File Name
            Console.WriteLine(name);

            FileInfo fileInfo = new FileInfo(NewFilePath);
            long fileSize = fileInfo.Length; //File Size
            Console.WriteLine("the size of the file is: ");
            Console.WriteLine(fileSize);
        }

        static void DeleteFile()
        {
            string DeletingFile = @"C:\Users\1000075326\Downloads\example\DeleteMe.txt";
            if (File.Exists(DeletingFile))
            {
                File.Delete(DeletingFile);
                Console.WriteLine("File Deleted Successfully");
            }
        }

        static void ReadAndWrite()
        {
            string sample1Path = @"C:\Users\1000075326\Downloads\example\sample.txt";
            string sample2Path = @"C:\Users\1000075326\Downloads\example\sample2.txt";
            string content = File.ReadAllText(sample1Path);
            File.WriteAllText(sample2Path, content);
        }
    }
}