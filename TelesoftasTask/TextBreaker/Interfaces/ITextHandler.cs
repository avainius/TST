using System.Collections.Generic;

namespace TextBreaker.Interfaces
{
    public interface ITextHandler
    {
        List<string> BreakText(string path, int maxCharCount, string outputPath = null);
        List<string> BreakLine(string text, int maxCharCount);
        List<string> GetFileText(string path);
    }
}