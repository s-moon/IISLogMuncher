﻿using NUnit.Framework;
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
            [Test] //   subject             scenario       result
            public void ReturnParameterList_IfNoneSupplied_ReturnEmptyList()
            {
                // arrange
                int expected = 0;
                var clo = new CommandLineOptions();

                // act
                int result = clo.GetParameters().Count;

                // assert
                Assert.AreEqual(result, expected);
            }

            [Test] //   subject             scenario            result
            public void ReturnParameterList_OneArgumentSupplied_ArgumentReturned()
            {
                // arrange
                string expected = "arg1";
                var clo = new CommandLineOptions();
                clo.AddParameter(expected);

                // act
                string result = clo.GetParameters().Single();

                // assert
                Assert.AreEqual(result, expected);
            }

            [Test] //   subject             scenario             result
            public void ReturnParameterList_TwoArgumentsSupplied_ArgumentsReturned()
            {
                // arrange
                string a1 = "arg1";
                string a2 = "arg2";
                var expected = new List<string>(new[] { a1, a2 });
                var clo = new CommandLineOptions();
                clo.AddParameter(a1);
                clo.AddParameter(a2);

                // act
                var result = clo.GetParameters();

                // assert
                CollectionAssert.AreEquivalent(expected, result);
            }

            [Test] //   subject      scenario       result
            public void ReturnOption_OptionSupplied_ValueReturnedForThatOption()
            {
                // arrange
                string expected = "3";
                string option = "s";
                var clo = new CommandLineOptions();
                clo.SetOption(option, expected);

                // act
                string result = clo.GetOption(option);

                // assert
                Assert.AreEqual(result, expected);
            }

            [Test] //   subject      scenario          result
            public void ReturnOption_OptionNotSupplied_ValueReturnedIsEmptyString()
            {
                // arrange
                string expected = String.Empty;
                string option = "s";
                var clo = new CommandLineOptions();

                // act
                string result = clo.GetOption(option);

                // assert
                Assert.AreEqual(result, expected);
            }
        }

    }
}