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
        public void CorrectTriangleTest()
        {
            bool expected = Math.IsTriangleCanExist(1, 1, 1);
            Assert.IsTrue(expected);
        }

        [TestCase(200, 1, 300)]
        public void IncorrectTriangleTest(float a, float b, float c)
        {
            bool expected = Math.IsTriangleCanExist(a, b, c);
            Assert.IsTrue(!expected);
        }

        [TestCase(-1, -3, 5)]
        public void NegativeTriangleTest(float a, float b, float c)
        {
            bool expected = Math.IsTriangleCanExist(a, b, c);
            Assert.IsTrue(!expected);
        }

        [TestCase(20, 2, 30, ExpectedResult = false)]
        [TestCase(10, 200, 3, ExpectedResult = false)]
        [TestCase(11, 22, 33, ExpectedResult = false)]
        [TestCase(11, 2, 35, ExpectedResult = false)]
        [TestCase(121, 25, 73, ExpectedResult = false)]
        [TestCase(133, 2275, 32, ExpectedResult = false)]
        [TestCase(30, 3000, 32, ExpectedResult = false)]
        public bool IncorrectTrinagleTestSequence(float a, float b, float c)
        {
            bool expected = Math.IsTriangleCanExist(a, b, c);
            return expected;
        }
    }
}
