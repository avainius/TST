using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBreaker.Interfaces
{
    public interface ITextHandler
    {
        void BreakText(string path, int maxCharCount, string outputPath = null);
        List<string> BreakLine(string text, int maxCharCount);
    }
}