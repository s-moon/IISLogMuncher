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
        public class ParametersMethod
        {
            CommandLineOptions clo;

            [SetUp]
            public void Init()
            {
                clo = new CommandLineOptions();
            }

            [TearDown]
            public void Dispose()
            {
                clo = null;
            }

            [Test] //   subject             scenario       result
            public void ReturnParameterList_IfNoneSupplied_ReturnEmptyList()
            {
                // arrange
                int expected = 0;

                // act
                int actual = clo.GetNonOptions().Count;

                // assert
                Assert.AreEqual(actual, expected);
            }

            [Test] //   subject             scenario            result
            public void ReturnParameterList_OneArgumentSupplied_ArgumentReturned()
            {
                // arrange
                string expected = "arg1";
                clo.AddNonOption(expected);

                // act
                string result = clo.GetNonOptions().Single();

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
                clo.AddNonOption(a1);
                clo.AddNonOption(a2);

                // act
                var result = clo.GetNonOptions();

                // assert
                CollectionAssert.AreEquivalent(expected, result);
            }
        }

        public class OptionsMethod
        {
            CommandLineOptions clo;

            [SetUp]
            public void Init()
            {
                clo = new CommandLineOptions();
            }

            [TearDown]
            public void Dispose()
            {
                clo = null;
            }

            [Test] //   subject      scenario       result
            public void ReturnOption_OptionSupplied_ValueReturnedForThatOption()
            {
                // arrange
                string expected = "3";
                char option = 's';
                clo.SetOption(option, expected);

                // act
                string result = clo.GetOption(option);

                // assert
                Assert.AreEqual(result, expected);
            }

            [Test] //   subject      scenario          result
            public void ReturnOption_OptionNotSupplied_ValueReturnedIsFalseForExistance()
            {
                // arrange
                bool expected = false;
                char option = 's';

                // act
                bool result = clo.IsOptionSet(option);

                // assert
                Assert.AreEqual(result, expected);
            }
        }
    }
}
