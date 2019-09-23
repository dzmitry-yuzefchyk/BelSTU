using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Lab3
{
    [TestFixture]
    public class MathTest
    {

        /// <summary>
        /// Triangle 1, 1, 1 should exist
        /// </summary>
        [Test]
        public void FirstTest()
        {
            bool expected = Math.IsTriangleCanExist(1, 1, 1);
            Assert.IsTrue(expected);
        }

        [TestCase(200, 1, 300)]
        public void SecondTest(float a, float b, float c)
        {
            bool expected = Math.IsTriangleCanExist(a, b, c);
            Assert.IsTrue(!expected);
        }

        [TestCase(100, 200, 200, ExpectedResult = true, Description = "Triangle with sides: 100, 200, 200")]
        [TestCase(20, 2, 30, ExpectedResult = false)]
        [TestCase(10, 200, 3, ExpectedResult = false)]
        [TestCase(11, 22, 33, ExpectedResult = false)]
        [TestCase(11, 2, 35, ExpectedResult = false)]
        [TestCase(121, 25, 73, ExpectedResult = false)]
        [TestCase(111, 213, 322, ExpectedResult = true)]
        [TestCase(133, 2275, 32, ExpectedResult = false)]
        public bool TestSequence(float a, float b, float c)
        {
            bool expected = Math.IsTriangleCanExist(a, b, c);
            return expected;
        }
    }
}
