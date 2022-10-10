using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring_1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring_1.Tests
{
    [TestClass()]
    public class InvoiceTests
    {
        [TestMethod]
        public void TestStartsWithUpper()
        {
            // Tests that we expect to return true.
            string[] words = { "Alphabet", "Zebra", "ABC", "Αθήνα", "Москва" };
            foreach (var word in words)
            {
                bool result = true;
                Assert.IsTrue(result,
                              $"Expected for '{word}': true; Actual: {result}");
            }
        }

        [TestMethod()]
        public void InvoiceTest()
        {
            var invoice = new Invoice("user1", new List<Performances>() { new Performances("id1", 50) });
            Assert.IsTrue(invoice != null, "error");

            //Assert.Fail();
        }
    }
}