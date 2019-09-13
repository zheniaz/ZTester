using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZTester.Services;

namespace ZTesterUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        InputService inputService = new InputService();  
        [TestMethod]
        public void TestSelectOneTest1()
        {
            int expected = 1;
            var result = inputService.SelectOneTest("1");
            Assert.AreEqual(expected, result[0]);
        }
        [TestMethod]
        public void TestSelectOneTest2()
        {
            int? expected = null;
            var result = inputService.SelectOneTest("112");
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestSelectOneTest3()
        {
            int? expected = null;
            var result = inputService.SelectOneTest("]");
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void TestSelectOneTest4()
        {
            int? expected = 1;
            var result = inputService.SelectOneTest(" 1");
            Assert.AreEqual(expected, result[0]);
        }

        [TestMethod]
        public void TestSelSelectSetOfTestsTest()
        {
            int[] expected = { 1, 2, 3 };
            var result = inputService.SelectSetOfTests("1,2,3");
            Assert.IsTrue(AreEqualDatas(expected,result));
        }
        [TestMethod]
        public void TestSelSelectSetOfTestsTest2()
        {
            int[] expected = { 1, 2, 3 };
            var result = inputService.SelectSetOfTests("1,2, 3,");
            Assert.IsTrue(AreEqualDatas(expected, result));
        }
        [TestMethod]
        public void TestSelSelectSetOfTestsTest3()
        {
            var result = inputService.SelectSetOfTests("1,2,300");
            Assert.IsNull(result);
        }
        [TestMethod]
        public void TestSelSelectSetOfTestsTest4()
        {
            var result = inputService.SelectSetOfTests("1,2,3]");
            Assert.IsNull(result);
        }

        private bool AreEqualDatas(int[] arr, List<int> list)
        {
            if (arr.Length != list.Count)
            {
                return false;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != list[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
