using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataProcessing
{
    internal class Program
    {
        static Information information = new Information();
        static string folderPath;
        static string pathString;
        const string mainFolder = "folder_b\\";
        static void Main(string[] args)
        {                  
            

            string[] txtFilesPaths = Directory.GetFiles("folder_a\\", "*.txt", SearchOption.AllDirectories);
            string[] txtCSVPaths = Directory.GetFiles("folder_a\\", "*.csv", SearchOption.AllDirectories);
            string[] allFilesPaths = Directory.GetFiles("folder_a\\", "*.*", SearchOption.AllDirectories);

            
            string[] filePaths = txtCSVPaths.Concat(txtFilesPaths).ToArray();

            filePaths.ToList().ForEach(x => allFilesPaths = allFilesPaths.Where(f => f != x).ToArray());

            information.invalid_files = allFilesPaths;

            folderPath = DateTime.Now.ToString("yyyy-dd-MM");

            pathString = Path.Combine(mainFolder, folderPath);

            Directory.CreateDirectory(pathString);

            int i = 1;

            foreach (string file in filePaths)
            {
                PaymentReport report = new PaymentReport();

                ReadFile(file, report,information);

                var jsonData = JsonSerializer.Serialize(report);

                File.WriteAllText($"folder_b\\{folderPath}\\output{i}.json", jsonData);

                File.Delete(file);

                
                information.parsed_files++;

                i++;
            }

            FileSystemWatcher watcher = new FileSystemWatcher("folder_a");
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;

            watcher.Created += (sender, e) =>
            {
                if(Path.GetExtension(e.FullPath) == ".csv" || Path.GetExtension(e.FullPath) == ".txt")
                {
                    PaymentReport report = new PaymentReport();
                    ReadFile(e.FullPath, report, information);

                    var jsonData = JsonSerializer.Serialize(report);

                    File.WriteAllText($"folder_b\\{folderPath}\\output{i}.json", jsonData);

                    File.Delete(e.FullPath);

                    information.parsed_files++;

                    i++;
                }
                else
                {
                    information.invalid_files.ToList().Add(e.FullPath);
                }
                
            };

            while (true) { 

                if(folderPath != DateTime.Now.ToString("yyyy-dd-MM"))
                {
                    File.WriteAllText($"folder_b\\{folderPath}\\meta.log", information.ToString());

                    information = new Information();

                    folderPath = DateTime.Now.ToString("yyyy-dd-MM");

                    pathString = Path.Combine(mainFolder, folderPath);

                    Directory.CreateDirectory(pathString);

                    i = 1;
                }

            }
        }
        
        static void ReadFile(string fileDicertory, PaymentReport report,Information information)
        {
            try
            {
                StreamReader sr = new StreamReader(fileDicertory);
                string line = sr.ReadLine();

                while (line != null)
                {
                    DataContext dataContext = new DataContext();
                    List<string> data = new List<string>();

                    DataNormalization(data, line);

                    if (Validate(data))
                    {
                        dataContext.AttachingData(data);

                        report.Add(dataContext);

                        information.parsed_lines++;
                    }
                    else
                        information.found_errors++;
                                        
                    line = sr.ReadLine();

                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        static void DataNormalization(List<string> data,string line)
        {
            MatchCollection matches = new Regex(@"“(.*?)”").Matches(line);

            foreach (Match match in matches)
            {
                data.Add(match.Groups[1].Value.Split(',').First());
                line = line.Replace(match.Groups[1].Value, "");
                line = line.Replace("\"\"", "");
                line = line.Replace("“”", "");
                line = line.Replace(",", "");
            }

            data.AddRange(line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        static bool Validate(List<string> data)
        {
            return data.Count == 7 && decimal.TryParse(data[3], out _) &&
            DateTime.TryParseExact(data[4], "yyyy-dd-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) &&
            long.TryParse(data[5], out _);
        }
    }
}
