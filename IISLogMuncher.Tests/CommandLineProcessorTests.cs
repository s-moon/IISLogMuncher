﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISLogMuncher.Tests
{
    [TestFixture]
    public class CommandLineProcessorTests
    {
        public class ProcessArgsMethod
        {
            private CommandLineOptions clo;

            [SetUp]
            public void Init()
            {

            }

            [TearDown]
            public void Dispose()
            {

            }

            // Test empty args
            [Test]
            public void ProcessArgs_NoneSupplied_NoneReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string[] args = { };

                // act
                clo = CommandLineProcessor.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count, clo.GetNonOptions().Count);
                Assert.AreEqual(expected.GetOptionCount(), clo.GetOptionCount());
            }

            // Test one non-option argument

            // Test two non-option arguments

            // Test one option with no associated argument

            // Test one option with an associated argument

            // Test one option with an associated argument next to it e.g. -s4

            // Test two options (neither with associated arguments)

            // Test two options, one of which has an associated argument and one doesn't

            // Test two options (no associated arguments) and one non-option

            // Test a non-option, option (no assoaciated argument) and a non-option

            // Test an option, a non-option, and an option



        }
    }
}
