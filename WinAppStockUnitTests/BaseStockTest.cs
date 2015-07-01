// Copyright Piotr Trojanowski 2015

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2.1 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

using WinAppStock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace WinAppStockUnitTests
{
    
    
    /// <summary>
    ///This is a test class for BaseStockTest and is intended
    ///to contain all BaseStockTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BaseStockTest
    {
        private static string baseStockTestName;
        private static string appDataPath;
        private static int initializerFuncCounter;
        static BaseStockTest()
        {
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            baseStockTestName = "BaseStockTest";
            initializerFuncCounter = 0;
        }   

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // it doesn't cause exception if directory doesn't exist so we do it in case MyTestCleanup() wasn't
            // fired last time (due to a crash or anything else)
            if (Directory.Exists(appDataPath + "\\" + baseStockTestName))
            {
                Directory.Delete(appDataPath + "\\" + baseStockTestName, true);
            }
            initializerFuncCounter = 0;
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            if (Directory.Exists(appDataPath + "\\" + baseStockTestName))
            {
                // remove stock created during the test runtime
                Directory.Delete(appDataPath + "\\" + baseStockTestName, true);
            }     
        }
        
        #endregion


        /// <summary>
        ///A test for BaseStock Constructor
        ///</summary>
        [TestMethod()]
        public void BaseStockConstructorTest()
        {
            string name = baseStockTestName;
            BaseStock target = new BaseStock(name);

            Assert.IsTrue(Directory.Exists(appDataPath + "\\" + baseStockTestName), "OOPS! Stock directory does not exist.");
        }

        /// <summary>
        ///A test for BaseStock Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WinAppStock.dll")]
        public void BaseStockConstructorTest1()
        {
            string name = baseStockTestName; 
            string absolutePath = appDataPath;
            BaseStock_Accessor target = new BaseStock_Accessor(name, absolutePath);

            Assert.IsTrue(Directory.Exists(appDataPath + "\\" + baseStockTestName), "OOPS! Stock directory does not exist.");
        }

        /// <summary>
        ///A test for DeleteChildFile
        ///</summary>
        [TestMethod()]
        public void DeleteChildFileTest()
        {
            string name = baseStockTestName;
            BaseStock target = new BaseStock(name);
            string name1 = "File";
            BaseStock.Initializer func = delegate(FileStream s) { ++initializerFuncCounter; };
            FileStream actual;
            actual = target.GetChildFile(name1);
            actual.Close();
            target.DeleteChildFile(name1);
            Assert.IsFalse(File.Exists(appDataPath + "\\" + baseStockTestName + "\\" + name1), "GOSH! Child file exist after deletion.");
        }

        /// <summary>
        ///A test for DeleteStock
        ///</summary>
        [TestMethod()]
        public void DeleteStockTest()
        {
            string name = baseStockTestName;
            BaseStock target = new BaseStock(name);
            target.DeleteStock();
            Assert.IsFalse(Directory.Exists(appDataPath + "\\" + baseStockTestName), "SHIT! Deleted stock directory still exists.");
        }

        /// <summary>
        ///A test for GetChildFile
        ///</summary>
        [TestMethod()]
        public void GetChildFileTest()
        {
            string name = baseStockTestName;
            BaseStock target = new BaseStock(name);
            string name1 = "File";
            BaseStock.Initializer func = delegate(FileStream s) { ++initializerFuncCounter; };
            FileStream actual;
            actual = target.GetChildFile(name1, func);
            Assert.AreEqual(actual.Name, appDataPath + "\\" + baseStockTestName + "\\" + name1, "DAMN! Returned FileStream has different name than it should.");
            Assert.IsTrue(initializerFuncCounter == 1, "DAMN! Initializer functions was not called.");
            actual.Close();
        }

        /// <summary>
        ///A test for GetChildFile
        ///</summary>
        [TestMethod()]
        public void GetChildFileTest1()
        {
            string name = baseStockTestName;
            BaseStock target = new BaseStock(name);
            string name1 = "File";
            FileStream actual;
            actual = target.GetChildFile(name1);
            Assert.AreEqual(appDataPath + "\\" + baseStockTestName + "\\" + name1, actual.Name, "DAMN! Returned FileStream has different name than it should.");
            actual.Close();
        }

        /// <summary>
        ///A test for GetChildStockRef
        ///</summary>
        [TestMethod()]
        public void GetChildStockRefTest()
        {
            string name = baseStockTestName;
            BaseStock target = new BaseStock(name);
            string name1 = "Child stock";
            BaseStock actual;
            actual = target.GetChildStockRef(name1);
            Assert.IsTrue(actual != null, "DOH! Child stock is null.");
            Assert.IsTrue(Directory.Exists(appDataPath + "\\" + baseStockTestName + "\\" + name1), "GOSH! Child stock directory was not created.");
        }

        /// <summary>
        ///A test for isFilenameAllowed
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WinAppStock.dll")]
        public void isFilenameAllowedTest()
        {
            string name = "Just a common name1.txt";
            bool expected = true;
            bool actual;
            actual = BaseStock_Accessor.isFilenameAllowed(name);
            Assert.AreEqual(expected, actual, "BAD! Name \"" + name + "\" is disallowed.");
        }

        /// <summary>
        ///A test for isFilenameAllowed
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WinAppStock.dll")]
        public void isFilenameAllowedTest1()
        {
            string name = "some slash name/.txt";
            bool expected = false;
            bool actual;
            actual = BaseStock_Accessor.isFilenameAllowed(name);
            Assert.AreEqual(expected, actual, "BAD! Name \"" + name + "\" is allowed.");
        }

        /// <summary>
        ///A test for isRemoved
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WinAppStock.dll")]
        public void isRemovedTest()
        {
            string name = baseStockTestName;
            BaseStock stock = new BaseStock(name);
            stock.DeleteStock();

            PrivateObject param0 = new PrivateObject(stock);
            BaseStock_Accessor target = new BaseStock_Accessor(param0);
            bool expected = true;
            bool actual;
            target.isRemoved = expected;
            actual = target.isRemoved;
            Assert.AreEqual(expected, actual, "EWW! Removed object is not marked as removed.");
        }

        /// <summary>
        ///A test for isStockReused
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WinAppStock.dll")]
        public void isStockReusedTest()
        {
            string name = baseStockTestName;
            BaseStock stock = new BaseStock(name);
            BaseStock duplicatedStock = new BaseStock(name);

            PrivateObject param0 = new PrivateObject(duplicatedStock);
            BaseStock_Accessor target = new BaseStock_Accessor(param0);
            bool expected = true;
            bool actual;
            target.isStockReused = expected;
            actual = target.isStockReused;
            Assert.AreEqual(expected, actual, "OH NO! Reused stock is not marked as reused.");
        }

        /// <summary>
        ///A test for isStockReused
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WinAppStock.dll")]
        public void isStockReusedTest1()
        {
            string name = baseStockTestName;
            BaseStock stock = new BaseStock(name);

            PrivateObject param0 = new PrivateObject(stock);
            BaseStock_Accessor target = new BaseStock_Accessor(param0);
            bool expected = false;
            bool actual;
            target.isStockReused = expected;
            actual = target.isStockReused;
            Assert.AreEqual(expected, actual, "OH NO! Not reused stock is marked as reused.");
        }

        // Doesn't really makes sense to test these properties
        ///// <summary>
        /////A test for name
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("WinAppStock.dll")]
        //public void nameTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    BaseStock_Accessor target = new BaseStock_Accessor(param0); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.name = expected;
        //    actual = target.name;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for stockPath
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("WinAppStock.dll")]
        //public void stockPathTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    BaseStock_Accessor target = new BaseStock_Accessor(param0); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.stockPath = expected;
        //    actual = target.stockPath;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
