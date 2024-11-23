using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popis
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger()
        {
            string logFileName = $"log_{DateTime.Now:yyyyMMdd_HHmm}.csv";
            _logFilePath = Path.Combine(Properties.Settings.Default.LogFileDirectory, logFileName);

            using (var writer = new StreamWriter(_logFilePath, false))
            {
                writer.WriteLine("Barkod,Naziv,Kolicina"); // Header row
            }
        }

        public void Log(string barkod, string naziv, double amountChange)
        {
            var lines = File.ReadAllLines(_logFilePath).ToList();

            var header = lines[0]; // Header row
            var dataLines = lines.Skip(1).ToList(); // All data rows
            bool rowUpdated = false;

            for (int i = 0; i < dataLines.Count; i++)
            {
                var fields = dataLines[i].Split(',');
                if (fields.Length >= 2 && fields[0] == barkod) // Check Barkod field
                {
                    // Update the existing row's Kolicina
                    fields[2] = (double.Parse(fields[2]) + amountChange).ToString(); // Update Kolicina
                    dataLines[i] = string.Join(",", fields); // Reconstruct the row
                    rowUpdated = true;
                    break;
                }
            }

            if (!rowUpdated)
            {
                dataLines.Add($"{barkod},{naziv},{amountChange}");
            }

            // Write the updated content back to the file
            File.WriteAllLines(_logFilePath, new[] { header }.Concat(dataLines));

/*
            using (var writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine($"{barkod},{naziv},{amountChange}");
            }

 */
        }

        public string getPathNameForNewFile()
        {
            
            SaveFileDialog path = new SaveFileDialog();
           
            if (path.ShowDialog() == true)
            {
                return path.FileName;
            }
            return "";
        }
    }
}
