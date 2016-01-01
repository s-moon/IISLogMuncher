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
            private CommandLineOptions actual;
            private CommandLineProcessor clp;
            private const string OptionNoArgument = ""; // must match what is in CommandLineProcessor

            [SetUp]
            public void Init()
            {
                clp = new CommandLineProcessor("s:abcde");
            }

            [TearDown]
            public void Dispose()
            {
                actual = null;
                clp = null;
            }

            // Test empty args
            [Test]
            public void ProcessArgs_NoneSupplied_NoneReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string[] args = { };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count, actual.GetNonOptions().Count);
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
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
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Single(), actual.GetNonOptions().Single());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
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
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), actual.GetNonOptions().Count());
                Assert.AreEqual(expected.GetNonOptions().First(), actual.GetNonOptions().First());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
            }

            // Test one option with no associated argument
            [Test]
            public void ProcessArgs_OneOptionAndNoArgSupplied_OneOptionReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-a";
                expected.SetOption(nOArg1.ElementAt(1), OptionNoArgument.ToString());
                string[] args = { nOArg1 };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), actual.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
            }

            // Test one option with an associated argument
            [Test]
            public void ProcessArgs_OneOptionAndArgSupplied_OneOptionAndArgReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-s"; 
                string nOArg1Value = "3";
                expected.SetOption(nOArg1.ElementAt(1), nOArg1Value);
                string[] args = { nOArg1, nOArg1Value };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), actual.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreSame(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
            }

            // Test one option with an associated argument next to it e.g. -s3
            [Test]
            public void ProcessArgs_OneOptionAndArgSuppliedNextToIt_OneOptionAndArgReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-s";
                string nOArg1Value = "3";
                expected.SetOption(nOArg1.ElementAt(1), nOArg1Value);
                string[] args = { nOArg1 + nOArg1Value };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), actual.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
            }

            // Test two options (neither with associated arguments)
            [Test]
            public void ProcessArgs_TwoOptionsAndNoArgSupplied_TwoOptionsReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-a";
                string nOArg2 = "-b";
                expected.SetOption(nOArg1.ElementAt(1), OptionNoArgument.ToString());
                expected.SetOption(nOArg2.ElementAt(1), OptionNoArgument.ToString());
                string[] args = { nOArg1, nOArg2 };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), actual.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
            }

            // Test two options, one of which has an associated argument and one doesn't
            [Test]
            public void ProcessArgs_TwoOptionsAndOneArgSupplied_TwoOptionsReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-a";
                string nOArg2 = "-s";
                string nOArg2Value = "argument";
                expected.SetOption(nOArg1.ElementAt(1), OptionNoArgument.ToString());
                expected.SetOption(nOArg2.ElementAt(1), nOArg2Value);
                string[] args = { nOArg1, nOArg2, nOArg2Value };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                Assert.AreEqual(expected.GetNonOptions().Count(), actual.GetNonOptions().Count());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
                Assert.AreEqual(expected.GetOption(nOArg2.ElementAt(1)), actual.GetOption(nOArg2.ElementAt(1)));
            }

            // Test two options (no associated arguments) and one non-option
            [Test]
            public void ProcessArgs_TwoOptionsAndOneNonOptionSupplied_TwoOptionsAndOneNonOptionReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-a";
                string nOArg2 = "-b";
                string nOArg3 = "hello.txt";
                expected.SetOption(nOArg1.ElementAt(1), OptionNoArgument.ToString());
                expected.SetOption(nOArg2.ElementAt(1), OptionNoArgument.ToString());
                expected.AddNonOption(nOArg3);
                string[] args = { nOArg1, nOArg2, nOArg3 };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                CollectionAssert.AreEqual(expected.GetNonOptions(), actual.GetNonOptions());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
            }

            // Test a non-option, option (no assoaciated argument) and a non-option
            [Test]
            public void ProcessArgs_TwoOptionsAndOneNonOptionSupplied_OptionNonOptionAndOptionReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "abc.jg";
                string nOArg2 = "-b";
                string nOArg3 = "hello.txt";
                expected.AddNonOption(nOArg1);
                expected.SetOption(nOArg2.ElementAt(1), OptionNoArgument.ToString());
                expected.AddNonOption(nOArg3);
                string[] args = { nOArg1, nOArg2, nOArg3 };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                CollectionAssert.AreEqual(expected.GetNonOptions(), actual.GetNonOptions());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg2.ElementAt(1)), actual.GetOption(nOArg2.ElementAt(1)));
            }

            // Test an option, a non-option, and an option
            [Test]
            public void ProcessArgs_OneOptionAndOneNonOptionAndOneOptionSupplied_OptionNonOptionAndOptionReturned()
            {
                // arrange
                CommandLineOptions expected = new CommandLineOptions();
                string nOArg1 = "-a";
                string nOArg2 = "fooba.doc";
                string nOArg3 = "-b";
                expected.SetOption(nOArg1.ElementAt(1), OptionNoArgument.ToString());
                expected.AddNonOption(nOArg2);
                expected.SetOption(nOArg3.ElementAt(1), OptionNoArgument.ToString());
                string[] args = { nOArg1, nOArg2, nOArg3 };

                // act
                actual = clp.ProcessArgs(args);

                // assert
                CollectionAssert.AreEqual(expected.GetNonOptions(), actual.GetNonOptions());
                Assert.AreEqual(expected.GetOptionCount(), actual.GetOptionCount());
                Assert.AreEqual(expected.GetOption(nOArg1.ElementAt(1)), actual.GetOption(nOArg1.ElementAt(1)));
                Assert.AreEqual(expected.GetOption(nOArg3.ElementAt(1)), actual.GetOption(nOArg3.ElementAt(1)));
            }

            // Test an option with no args that should be present
            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ProcessArgs_OneOptionWithNoArgSupplied_ExceptionThrown()
            {
                // arrange
                string nOArg1 = "-s";
                string[] args = { nOArg1 };

                // act
                actual = clp.ProcessArgs(args);

                // assert - not needed
            }

            // Test an option with args that shouldn't be present

            // Option supplied in setup are null
            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ProcessArgs_NullOptions_ExceptionThrown()
            {
                // arrange
                clp = new CommandLineProcessor(null);

                // act - not required

                // assert - not required
            }

            // Option supplied in parameters are null
            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void ProcessArgs_NullOptionsAsParameter_ExceptionThrown()
            {
                // arrange
                clp = new CommandLineProcessor();

                // act - not required
                clp.Options = null;

                // assert - not required
            }
        }
    }
}
