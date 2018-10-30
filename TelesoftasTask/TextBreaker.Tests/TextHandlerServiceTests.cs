using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using TextBreaker.Interfaces;
using TextBreaker.Services;

namespace TextBreaker.Tests
{
    [TestClass]
    public class TextHandlerServiceTests
    {
        ITextHandler textHandler;
        string text;
        string dir;
        string path;
        string outputPath;

        [TestInitialize]
        public void Setup()
        {
            textHandler = new TextHandlerService();
            text = "Words can be like bricks - bad for your teeth.";
            dir = $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\";
            path = $"{dir}TestFile.test";
            outputPath = $"{dir}OutputTestFile.test";
        }

        [TestMethod]
        public void BreakLine_TriesToBreakLine_BreakesLineCorrectly()
        {
            var maxLineLength = 5;
            var expectedResult = new List<string>
            {
                "Words",
                "can",
                "be",
                "like",
                "brick",
                "s -",
                "bad",
                "for",
                "your",
                "teeth",
                "."
            };

            var actualResult = textHandler.BreakLine(text, maxLineLength);

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakLine_TriesToBreakTelesoftasExample1Line_BreakesLineCorrectly()
        {
            var maxLineLength = 7;
            text = "šiuolaikiškas ir mano žodis";
            var expectedResult = new List<string>
            {
                "šiuolai",
                "kiškas",
                "ir mano",
                "žodis"
            };

            var actualResult = textHandler.BreakLine(text, maxLineLength);

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakLine_TriesToBreakTelesoftasExample2Line_BreakesLineCorrectly()
        {
            var maxLineLength = 13;
            text = "žodis žodis žodis";
            var expectedResult = new List<string>
            {
                "žodis žodis",
                "žodis"
            };

            var actualResult = textHandler.BreakLine(text, maxLineLength);

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakText_GetsAndBreaksTextFromFile_BreakesTextCorrectly()
        {
            var actualResult = new List<string>();
            var maxLineLength = 6;
            var expectedResult = new List<string>
            {
                "Words",
                "can be",
                "like",
                "bricks",
                "- bad",
                "for",
                "your",
                "teeth."
            };
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            File.AppendAllText(path, text);

            textHandler.BreakText(path, maxLineLength, outputPath);

            Assert.IsTrue(File.Exists(outputPath));
            using(var stream = new StreamReader(outputPath))
            {
                while (!stream.EndOfStream)
                {
                    actualResult.Add(stream.ReadLine());
                }
            }

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakText_GetsAndBreaksTextFromFile_ReturnedTextMatchesOutputInFileAndIsCorrect()
        {
            var actualResult = new List<string>();
            var maxLineLength = 8;
            var expectedResult = new List<string>
            {
                "Words",
                "can be",
                "like",
                "bricks -",
                "bad for",
                "your",
                "teeth."
            };
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            File.AppendAllText(path, text);

            var result = textHandler.BreakText(path, maxLineLength, outputPath);

            Assert.IsTrue(File.Exists(outputPath));
            using (var stream = new StreamReader(outputPath))
            {
                while (!stream.EndOfStream)
                {
                    actualResult.Add(stream.ReadLine());
                }
            }

            AssertListUniformity(expectedResult, actualResult);

            AssertListUniformity(expectedResult, result);
        }

        [TestMethod]
        public void GetFileText_RetrievesTextLineFromFile_RetrieveSuccess()
        {
            var expectedResult = new List<string>() { text };
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            File.AppendAllText(path, text);

            var actualResult = textHandler.GetFileText(path);

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetFileText_RetrievesTextLinesFromFile_RetrieveSuccess()
        {
            var expectedResult = new List<string>()
            {
                "Lots of values to return.",
                "Better be a good reader.",
                "Not that it's hard to make one."
            };

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            File.AppendAllLines(path, expectedResult);

            var actualResult = textHandler.GetFileText(path);

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakLongWord_BreaksWordExceedingLineLength_Success()
        {
            var actualWord = "nebeprisikiškiakopūstėliaudamiesi";
            var maxCharCount = 7;
            var expectedResult = new List<string>()
            {
                "nebepri",
                "sikiški",
                "akopūst",
                "ėliauda",
                "miesi"
            };

            var actualResult = textHandler.BreakLongWord(actualWord, maxCharCount);

            AssertListUniformity(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetFileText_TriesToGetTextFromNonExistingFile_ThrowsFileNotFoundException()
        {
            if (File.Exists(path)) File.Delete(path);
            textHandler.GetFileText(path);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(path)) File.Delete(path);
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }

        private void AssertListUniformity(List<string> expectedResult, List<string> actualResult)
        {
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            for (var i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i], $"Line {i} did not match. Expected: '{expectedResult[i]}'; Actual: '{actualResult[i]}';");
            }
        }
    }
}
