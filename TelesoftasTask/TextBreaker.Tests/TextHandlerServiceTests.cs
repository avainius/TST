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
            expectedResult.Add("or you");
            expectedResult.Add("r tee");
            expectedResult.Add("th");

            var actualResult = textHandler.BreakLine(text, maxLineLength);

            Assert.AreEqual(expectedResult.Count, actualResult.Count, "Result line count did not match expected line count");
            for(var i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i], $"Line {i} did not match. Expected: '{expectedResult[i]}'; Actual: '{actualResult[i]}';");
            }
        }

        [TestMethod]
        public void BreakText_GetsAndBreaksTextFromFile_BreakesTextCorrectly()
        {
            var actualResult = new List<string>();
            var maxLineLength = 6;
            var expectedResult = new List<string>();
            expectedResult.Add("Words ");
            expectedResult.Add("can be");
            expectedResult.Add("like b");
            expectedResult.Add("bricks");
            expectedResult.Add("- bad ");
            expectedResult.Add("for yo");
            expectedResult.Add("ur tee");
            expectedResult.Add("th");
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

            Assert.AreEqual(expectedResult.Count, actualResult.Count, "Result line count did not match expected line count");
            for (var i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i], $"Line {i} did not match. Expected: '{expectedResult[i]}'; Actual: '{actualResult[i]}';");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(path)) File.Delete(path);
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }
    }
}
