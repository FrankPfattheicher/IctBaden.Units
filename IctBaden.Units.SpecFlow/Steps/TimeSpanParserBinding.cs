using System;
using FluentAssertions;
using IctBaden.Units.TimeSpans;
using TechTalk.SpecFlow;

namespace IctBaden.Units.SpecFlow.Steps
{
    [Binding]
    public class TimeSpanParserBinding
    {
        private readonly ScenarioContext _context;

        public TimeSpanParserBinding(ScenarioContext context)
        {
            _context = context;
        }
        [Given(@"'(.*)' is (.*)")]
        public void GivenTextIs(string param, string value)
        {
            _context[param] = value;
        }

        [When(@"parsed '(.*)' as '(.*)'")]
        public void WhenParsedAs(string param, string result)
        {
            _context[result] = TimeSpanParser.Parse(_context[param].ToString());
        }

        [Then(@"the '(.*)' should be (.*) days, (.*) hours, (.*) minutes, (.*) seconds")]
        public void ThenTheTimeSpanShouldBe(string result, int days, int hours, int minutes, int seconds)
        {
            var totalSeconds = days * (24 * 60 * 60);
            totalSeconds += hours * (60 * 60);
            totalSeconds += minutes * 60;
            totalSeconds += seconds;

            _context[result].As<TimeSpan>().TotalSeconds.Should().Be(totalSeconds);
        }

        [Then(@"the '(.*)' should be (.*) total seconds")]
        public void ThenTheTimeSpanShouldBeTotalSeconds(string result, int totalSeconds)
        {
            _context[result].As<TimeSpan>().TotalSeconds.Should().Be(totalSeconds);
        }

    }
}