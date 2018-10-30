using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBreaker.Interfaces
{
    public interface IFileHandler
    {
        void CreateDirIfNotExists(string dir);
        void CreateFileIfNotExists(string path);
        void AppendTextToFile(string text, string path);
    }
}
