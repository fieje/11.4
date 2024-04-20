using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace YourNamespace.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private const string TestFileName = "test.txt";

        [TestMethod]
        public void TestPrintProductInfo()
        {
            string productName = "Product1";
            string[] inputLines = { "Product1,Store1,10.0", "Product2,Store2,20.0" };

            using (StreamWriter writer = new StreamWriter(TestFileName))
            {
                foreach (string line in inputLines)
                {
                    writer.WriteLine(line);
                }
            }

            string output = CaptureConsoleOutput(() => Program.PrintProductInfo(TestFileName, productName));

            Assert.IsTrue(output.Contains($"Product Name: {productName}"));
            Assert.IsTrue(output.Contains("Store: Store1"));
            Assert.IsTrue(output.Contains("Price: 10.0 UAH"));
        }

        private static string CaptureConsoleOutput(Action action)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                action.Invoke(); 
                return stringWriter.ToString();
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (File.Exists(TestFileName))
            {
                File.Delete(TestFileName);
            }
        }
    }
}
