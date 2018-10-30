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
            if(text.Length <= maxCharCount) { result.Add(text); return result; }
            var words = text.Split(' ');
            foreach(var currentWord in  words)
            {
                if(currentWord.Length > maxCharCount)
                {
                    var brokenWord = BreakLongWord(currentWord, maxCharCount);
                    brokenWord.ForEach(result.Add);
                    continue;
                }

                if (result.Count == 0)
                {
                    result.Add(currentWord);
                    continue;
                }

                var lastWord = result[result.Count - 1];
                var potentialLastLineLength = lastWord.Length + currentWord.Length + 1;
                if (potentialLastLineLength <= maxCharCount)
                {
                    result[result.Count - 1] = $"{lastWord} {currentWord}";
                    continue;
                }

                result.Add(currentWord);
            }

            return result;
        }

        public List<string> BreakLongWord(string word, int maxCharCount)
        {
            if (word.Length <= maxCharCount) return new List<string>() { word };
            var result = new List<string>();
            var i = 0;
            while(i < word.Length)
            {
                if(word.Substring(i).Length <= maxCharCount)
                {
                    result.Add(word.Substring(i));
                    break;
                }
                else
                    result.Add(word.Substring(i, maxCharCount));
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
