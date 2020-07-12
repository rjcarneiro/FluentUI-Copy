using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.System.Icons.Copier.App
{
    class Program
    {
        private const string PathPrefix = ".\assets";
        private const string FilePrefix = "ic_fluent_";
        private const string FileSuffix = ".svg";

        static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

        [Required]
        [Option("-i", Description = "The Fluent UI system icon")]
        public string Icon { get; }

        [Required]
        [Option("-d", Description = "The destination folder")]
        public string DestinationFolder { get; }

        [Option("-n", Description = "The new name of the file")]
        public string NewName { get; }

        [Option("-v", Description = "Runs the application in verbose mode")]
        public bool Verbose { get; }

        public void OnExecute()
        {
            try
            {
                var iconSplit = Icon.SplitCamelCase();
                string folder = GetPath(iconSplit);

                string fileName = $"{FilePrefix}{GetFileName(iconSplit)}{FileSuffix}";
                string fullFilePath = Path.Combine(PathPrefix, folder, "SVG", fileName);

                WriteMessage($"Trying to get file at {fullFilePath}");

                if (!File.Exists(fullFilePath))
                    throw new FileNotFoundException(fullFilePath);

                if (!Directory.Exists(DestinationFolder))
                {
                    WriteMessage($"Directoy {DestinationFolder} does not exist... creating...");
                    Directory.CreateDirectory(DestinationFolder);
                }

                string newFullFilePath = Path.Combine(DestinationFolder, fileName);

                WriteMessage($"Copying file to {newFullFilePath}");
                File.Copy(fullFilePath, newFullFilePath);

                if (!string.IsNullOrEmpty(NewName))
                {
                    WriteMessage($"Renaming to {NewName}");
                    File.Move(newFullFilePath, Path.Combine(DestinationFolder, NewName));
                }
            }
            catch (Exception e)
            {
                WriteException(e);
            }
        }

        private void WriteMessage(string message)
        {
            if (Verbose)
                Console.WriteLine(message);
        }

        private void WriteException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.GetBaseException().Message);
            if (Verbose)
                Console.WriteLine(e.StackTrace);
            Console.ResetColor();
        }

        private string GetFileName(ICollection<string> iconSplit) => string.Join("_", iconSplit).ToLower();

        private string GetPath(ICollection<string> iconSplit)
        {
            var builder = new StringBuilder();

            foreach (var icon in iconSplit)
            {
                if (int.TryParse(icon, out var _))
                    break;

                builder.Append($"{icon.FirstCharToUpper()} ");
            }

            // remove last char
            builder.Length--;

            return builder.ToString();
        }
    }
}