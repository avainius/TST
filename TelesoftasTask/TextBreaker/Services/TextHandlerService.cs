using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBreaker.Interfaces;

namespace TextBreaker.Services
{
    public class TextHandlerService : ITextHandler
    {
        public List<string> BreakLine(string text, int maxCharCount) => throw new NotImplementedException();
        public void BreakText(string path, int maxCharCount, string outputPath = null) => throw new NotImplementedException();
    }
}
