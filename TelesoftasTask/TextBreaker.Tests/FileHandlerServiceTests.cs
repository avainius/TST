using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TextBreaker.Interfaces;
using TextBreaker.Services;

namespace TextBreaker.Tests
{
    [TestClass]
    public class FileHandlerServiceTests
    {
        IFileHandler fileHandler;
        string dir;
        string path;
        string textToAppend;

        [TestInitialize]
        public void Setup()
        {
            fileHandler = new FileHandlerService();
            dir = $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\";
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
            path = $"{dir}TestFile.test";
            if (File.Exists(path)) File.Delete(path);
            textToAppend = "Super important text to be appended for testing purposes";
        }

        [TestMethod]
        public void CreateDirIfNotExists_CreatesNonExistingDirectory_DirectoryCreated()
        {
            fileHandler.CreateDirIfNotExists(dir);

            Assert.IsTrue(Directory.Exists(dir));
        }

        [TestMethod]
        public void CreateFileIfNotExists_CreatesNonExistingFile_FileCreated()
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            fileHandler.CreateFileIfNotExists(path);

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void AppendTextToFile_AppendsTextToFile_AppendsSuccessfully()
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(path)) File.Create(path).Close();
            var fileText = string.Empty;

            fileHandler.AppendTextToFile(textToAppend, path);

            using (var stream = new StreamReader(path))
            {
                while (!stream.EndOfStream)
                {
                    fileText += stream.ReadLine();
                }
            }

            Assert.AreEqual(fileText, textToAppend);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(path)) File.Delete(path);
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }
    }
}
