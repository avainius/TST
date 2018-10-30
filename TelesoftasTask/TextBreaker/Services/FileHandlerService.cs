using System.Collections.Generic;
using System.IO;
using TextBreaker.Interfaces;

namespace TextBreaker.Services
{
    public class FileHandlerService : IFileHandler
    {
        public void AppendLinesToFile(string path, List<string> lines)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"File at '{path}' not found");
            File.AppendAllLines(path, lines);
        }

        public void CreateDirIfNotExists(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public void CreateFileIfNotExists(string path)
        {
            if (!File.Exists(path)) File.Create(path).Close();
        }
    }
}
