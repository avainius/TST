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
            var expectedResult = new List<string>();
            expectedResult.Add("Words");
            expectedResult.Add("can b");
            expectedResult.Add("e lik");
            expectedResult.Add("e bri");
            expectedResult.Add("cks -");
            expectedResult.Add("bad f");
            expectedResult.Add("or yo");
            expectedResult.Add("ur te");
            expectedResult.Add("eth.");

            var actualResult = textHandler.BreakLine(text, maxLineLength);

            AssertListUniformityValues(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakText_GetsAndBreaksTextFromFile_BreakesTextCorrectly()
        {
            var actualResult = new List<string>();
            var maxLineLength = 6;
            var expectedResult = new List<string>();
            expectedResult.Add("Words");
            expectedResult.Add("can be");
            expectedResult.Add("like b");
            expectedResult.Add("ricks");
            expectedResult.Add("- bad");
            expectedResult.Add("for yo");
            expectedResult.Add("ur tee");
            expectedResult.Add("th.");
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

            AssertListUniformityValues(expectedResult, actualResult);
        }

        [TestMethod]
        public void BreakText_GetsAndBreaksTextFromFile_ReturnedTextMatchesOutputInFileAndIsCorrect()
        {
            var actualResult = new List<string>();
            var maxLineLength = 8;
            var expectedResult = new List<string>();
            expectedResult.Add("Words ca");
            expectedResult.Add("n be lik");
            expectedResult.Add("e bricks");
            expectedResult.Add("- bad fo");
            expectedResult.Add("r your t");
            expectedResult.Add("eeth.");
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

            AssertListUniformityValues(expectedResult, actualResult);

            AssertListUniformityValues(expectedResult, result);
        }

        [TestMethod]
        public void GetFileText_RetrievesTextLineFromFile_RetrieveSuccess()
        {
            var actualResult = new List<string>();
            var expectedResult = new List<string>() { text };
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            File.AppendAllText(path, text);

            actualResult = textHandler.GetFileText(path);

            AssertListUniformityValues(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetFileText_RetrievesTextLinesFromFile_RetrieveSuccess()
        {
            var actualResult = new List<string>();
            var expectedResult = new List<string>()
            {
                "Lots of values to return.",
                "Better be a good reader.",
                "Not that it's hard to make one."
            };

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            File.AppendAllLines(path, expectedResult);

            actualResult = textHandler.GetFileText(path);

            AssertListUniformityValues(expectedResult, actualResult);
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

        private void AssertListUniformityValues(List<string> expectedResult, List<string> actualResult)
        {
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            for (var i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i], $"Line {i} did not match. Expected: '{expectedResult[i]}'; Actual: '{actualResult[i]}';");
            }
        }
    }
}
