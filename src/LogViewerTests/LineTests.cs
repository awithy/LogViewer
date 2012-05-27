using LogViewer;
using NUnit.Framework;

namespace LogViewerTests
{
    [TestFixture]
    public abstract class LineTests
    {
        [TestFixture]
        public class When_parsing_a_valid_line : LineTests
        {
            private Line _line;

            [SetUp]
            public void BecauseOf()
            {
                _line = new Line("[0102 12:35:44][LogLevel][Source][ConfigKey][16][12] Message");
            }

            [Test]
            public void It_should_parse_the_date_time()
            {
                BecauseOf();
                Assert.That(_line.DateTime, Is.EqualTo("0102 12:35:44"));
            }

            [Test]
            public void It_should_parse_the_source()
            {
                BecauseOf();
                Assert.That(_line.Source, Is.EqualTo("Source"));
            }

            [Test]
            public void It_should_parse_the_log_leve()
            {
                BecauseOf();
                Assert.That(_line.LogLevel, Is.EqualTo("LogLevel"));
            }

            [Test]
            public void It_should_parse_the_config_key()
            {
                BecauseOf();
                Assert.That(_line.ConfigKey, Is.EqualTo("ConfigKey"));
            }

            [Test]
            public void It_should_parse_the_instance_id()
            {
                BecauseOf();
                Assert.That(_line.InstanceId, Is.EqualTo("16"));
            }

            [Test]
            public void It_should_parse_the_thread_id()
            {
                BecauseOf();
                Assert.That(_line.ThreadId, Is.EqualTo("12"));
            }

            [Test]
            public void It_should_parse_the_message()
            {
                BecauseOf();
                Assert.That(_line.Message, Is.EqualTo("Message"));
            }
        }
    }
}











