using System.Collections.Generic;

namespace TextBreaker.Interfaces
{
    public interface IFileHandler
    {
        void CreateDirIfNotExists(string dir);
        void CreateFileIfNotExists(string path);
        void AppendLinesToFile(string path, List<string> lines);
    }
}
