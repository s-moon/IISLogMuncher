using NUnit.Framework;
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
            private CommandLineOptions result;

            [SetUp]
            public void Init()
            {

            }

            [TearDown]
            public void Dispose()
            {
                result = null;
            }

            // Test empty args
            [Test]
            public void ProcessArgs_NoneSupplied_NoneReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string[] args = { };

                // act
                result = CommandLineProcessor.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count, result.GetNonOptions().Count);
                Assert.AreEqual(expected.GetOptionCount(), result.GetOptionCount());
            }

            // Test one non-option argument
            [Test]
            public void ProcessArgs_OneNonOptionSupplied_OneNonOptionReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "hello.txt";
                expected.AddNonOption(nOArg1);
                string[] args = { nOArg1 };

                // act
                result = CommandLineProcessor.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Single(), result.GetNonOptions().Single());
                Assert.AreEqual(expected.GetOptionCount(), result.GetOptionCount());
            }

            // Test two non-option arguments
            [Test]
            public void ProcessArgs_TwoNonOptionsSupplied_TwoNonOptionsReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "hello.txt";
                string nOArg2 = "goodbye.txt";
                expected.AddNonOption(nOArg1);
                expected.AddNonOption(nOArg2);
                string[] args = { nOArg1, nOArg2 };

                // act
                result = CommandLineProcessor.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), result.GetNonOptions().Count());
                Assert.AreEqual(expected.GetNonOptions().First(), result.GetNonOptions().First());
                Assert.AreEqual(expected.GetOptionCount(), result.GetOptionCount());
            }

            // Test one option with no associated argument

            // Test one option with an associated argument
            [Test]
            public void ProcessArgs_OneOptionAndArgSupplied_OneOptionAndArgReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-s";
                string nOArg1Value = "3";
                expected.SetOption(nOArg1.Substring(1), nOArg1Value);
                string[] args = { nOArg1, nOArg1Value };

                // act
                result = CommandLineProcessor.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), result.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), result.GetOptionCount());
                Assert.AreSame(expected.GetOption(nOArg1.Substring(1)), result.GetOption(nOArg1.Substring(1)));
            }

            // Test one option with an associated argument next to it e.g. -s3
            [Test]
            public void ProcessArgs_OneOptionAndArgSuppliedNextToIt_OneOptionAndArgReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-s";
                string nOArg1Value = "3";
                expected.SetOption(nOArg1.Substring(1), nOArg1Value);
                string[] args = { nOArg1 + nOArg1Value };

                // act
                result = CommandLineProcessor.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), result.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), result.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.Substring(1)), result.GetOption(nOArg1.Substring(1)));
            }

            // Test two options (neither with associated arguments)

            // Test two options, one of which has an associated argument and one doesn't

            // Test two options (no associated arguments) and one non-option

            // Test a non-option, option (no assoaciated argument) and a non-option

            // Test an option, a non-option, and an option



        }
    }
}
