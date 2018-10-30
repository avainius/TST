using System;
using System.Collections.Generic;
using System.IO;
using TextBreaker.Interfaces;

namespace TextBreaker.Services
{
    public class TextHandlerService : ITextHandler
    {
        public List<string> BreakLine(string text, int maxCharCount)
        {
            var result = new List<string>();
            var chars = text.ToCharArray();
            var length = text.Length;
            if(length <= maxCharCount) { result.Add(text); return result; }

            var i = 0;
            while(i < length)
            {
                while (string.IsNullOrWhiteSpace(text.Substring(i, 1))) i++;
                if (text.Substring(i).Length < maxCharCount)
                    result.Add(text.Substring(i).TrimStart().TrimEnd());
                else
                    result.Add(text.Substring(i, maxCharCount).TrimStart().TrimEnd());
                i += maxCharCount;
            }

            return result;
        }

        public List<string> BreakText(string path, int maxCharCount, string outputPath = null)
        {
            var result = new List<string>();
            var fileText = string.Concat(GetFileText(path));
            result = BreakLine(fileText, maxCharCount);

            if (string.IsNullOrEmpty(outputPath)) return result;

            if (!File.Exists(outputPath))
                File.Create(outputPath).Close();
            using (var stream = new StreamWriter(outputPath))
            {
                result.ForEach(stream.WriteLine);
            }

            return result;
        }

        public List<string> GetFileText(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"Couldn't locate file at '{path}'");
            var result = new List<string>();
            using (var stream = new StreamReader(path))
            {
                while (!stream.EndOfStream)
                {
                    result.Add(stream.ReadLine());
                }
            }

            return result;
        }
    }
}
