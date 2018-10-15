using NUnit.Framework;

namespace TicketSearch.UnitTest
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void DoHello()
        {
            Assert.AreEqual(true, true);
        }

        [TestCase('A', true)]
        [TestCase(1, false)]
        public void IsCharValid(char a, bool expected)
        {
            bool v = (a == 'A' ? true : false);
            Assert.AreEqual(expected, v);
        }
    }
}
