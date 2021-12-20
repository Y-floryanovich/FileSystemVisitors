using System;
using System.Linq;
using Moq;
using FileSystemVisitor;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemVisitorTests
{
    [TestClass]
    public class UnitTest1
    {
        Mock<IDirectory> dirInfo;
        Visitor visitor;

        public UnitTest1()
        {
            dirInfo = new Mock<IDirectory>();
            visitor = new Visitor(dirInfo.Object);

            dirInfo.Setup(d => d.GetDirectories(It.Is<string>(s => s == "Root"))).Returns(new[] { "Clothes", "Food" });
            dirInfo.Setup(d => d.GetFiles(It.IsRegex("Clothes"))).Returns(new[] { "T-shirt", "Skirt", "Pants" });
            dirInfo.Setup(d => d.GetFiles(It.IsRegex("Food"))).Returns(new[] { "Chips", "Pizza", "Icecream" });
            dirInfo.Setup(d => d.GetDirectories(It.Is<string>(s => s == "Food"))).Returns(new[] { "Drinks" });
            dirInfo.Setup(d => d.GetDirectories(It.Is<string>(s => s == "Food"))).Returns(new[] { "HealthFood" });
            dirInfo.Setup(d => d.GetFiles(It.Is<string>(s => s == "HealthFood"))).Returns(new[] { "Apple" });
        }

        void SetupMethods(Mock<IDirectory> dirInfo)
        {
            dirInfo.Setup(d => d.GetDirectories(It.Is<string>(s => s == "Root"))).Returns(new[] { "Clothes", "Food" });
            dirInfo.Setup(d => d.GetFiles(It.IsRegex("Clothes"))).Returns(new[] { "T-shirt", "Skirt", "Pants" });
            dirInfo.Setup(d => d.GetFiles(It.IsRegex("Food"))).Returns(new[] { "Chips", "Pizza", "Icecream" });
            dirInfo.Setup(d => d.GetDirectories(It.Is<string>(s => s == "Food"))).Returns(new[] { "Drinks" });
            dirInfo.Setup(d => d.GetDirectories(It.Is<string>(s => s == "Food"))).Returns(new[] { "HealthFood" });
            dirInfo.Setup(d => d.GetFiles(It.Is<string>(s => s == "HealthFood"))).Returns(new[] { "Apple" });
        }

        [TestMethod]
        public void GetFiles_toStopTrue_Verify()
        {
            // arrange
            visitor.FilteredFilesFinded += delegate (Object sender, EventArgs e)
            {
                var temp = sender as Visitor;
                temp.toStop = true;
            };

            // act
            var tmp = visitor.GetDirs("Root", x => x.Length < 6).ToList();

            // assert
            dirInfo.Verify(d => d.GetDirectories(It.IsAny<string>()),
                    Times.Once());
        }

        [TestMethod]
        public void GetDirs_toExcludeTrue_EmptyList()
        {
            // arrange
            visitor.FilesFinded += delegate (Object sender, EventArgs e)
            {
                var temp = sender as Visitor;
                temp.toExclude = true;
            };

            // act
            var tmp = visitor.GetDirs("Root", x => x.Length < 6).ToList();

            // assert
            // Методы вызываются в обычном количестве, но строки не возвращаются.
            Assert.IsTrue(dirInfo.Invocations.Count == 8);
            Assert.IsTrue(tmp.Count == 0);
        }

        [TestMethod]
        public void GetDirs_Lambda_DirsLengthLessThanSix()
        {
            // arrange

            // act
            var tmp = visitor.GetDirs("Root", x => x.Length < 6).ToList();

            // assert
            Assert.IsTrue(tmp.All<string>(d => d.Length < 6));
        }

        [TestMethod]
        public void GetDirs_GetsRoot_ReturnsDirs()
        {
            // arrange
            var expected = new List<string> { "Food", "Skirt", "Pants", "Chips", "Apple", "Pizza" };
            expected.Sort();
            // act
            var tmp = visitor.GetDirs("Root", x => x.Length < 6).ToList();
            tmp.Sort();
            // assert
            Assert.IsTrue(tmp.SequenceEqual(expected));
        }

    }
}
