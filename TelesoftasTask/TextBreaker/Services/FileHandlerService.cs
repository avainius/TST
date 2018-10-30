using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBreaker.Interfaces;

namespace TextBreaker.Services
{
    public class FileHandlerService : IFileHandler
    {
        public void AppendTextToFile(string text, string path) => throw new NotImplementedException();
        public void CreateDirIfNotExists(string dir) => throw new NotImplementedException();
        public void CreateFileIfNotExists(string path) => throw new NotImplementedException();
    }
}
