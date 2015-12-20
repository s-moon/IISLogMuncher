using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher.Tests
{
    [TestFixture]
    public class CommandLineOptionsTests
    {
        [Test]
        public void LooseArgumentsEmptyOnSetup()
        {
            int expected = 0;

            var clo = new CommandLineOptions();

            Assert.AreEqual(clo.GetParameters().Count, expected);
        }
    }
}
