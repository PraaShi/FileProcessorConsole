// See https://aka.ms/new-console-template for more information
using System;
using System.Configuration;
using System.IO;

namespace FileProcessorConsoleApp
{
    class Program
    {
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

            ProcessFiles(inputFolder, correctFolder , errorFolder,sampleFolder);

            //Renaming
            string OldFilePath = @"C:\Users\1000075326\Downloads\example\OldFileName.txt";
            string NewFilePath = @"C:\Users\1000075326\Downloads\example\NewFileName.txt";

            if (File.Exists(OldFilePath))
            {
                File.Move(OldFilePath, NewFilePath);
                Console.WriteLine("File Renaming Successful");
            }

            //Deleting
            string DeletingFile = @"C:\Users\1000075326\Downloads\example\DeleteMe.txt";
            if (File.Exists(DeletingFile)) 
            {
                File.Delete(DeletingFile);
            }

            //Read and write
            string sample1Path = @"C:\Users\1000075326\Downloads\example\sample.txt";
            string sample2Path = @"C:\Users\1000075326\Downloads\example\sample2.txt";
            string content = File.ReadAllText(sample1Path);
            File.WriteAllText(sample2Path, content);


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
    }
}