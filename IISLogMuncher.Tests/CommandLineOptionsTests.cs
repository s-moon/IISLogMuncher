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
            public void ReturnParameterList_IfNoneSupplied_ReturnEmptyList()
            {
                // arrange
                int expected = 0;

                // act
                var clo = new CommandLineOptions();

                // assert
                Assert.AreEqual(clo.GetParameters().Count, expected);
            }
        }

    }
}
