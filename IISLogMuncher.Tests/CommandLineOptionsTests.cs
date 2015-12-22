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
        public class GetParametersMethod
        {
            [Test]
            public void ReturnUnassignedParameterList_IfNoneSupplied_ReturnEmptyList()
            {
                int expected = 0;

                var clo = new CommandLineOptions();

                Assert.AreEqual(clo.GetParameters().Count, expected);
            }
        }

    }
}
