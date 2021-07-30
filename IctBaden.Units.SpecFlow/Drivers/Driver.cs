using BoDi;
using TechTalk.SpecFlow;

namespace IctBaden.Units.SpecFlow.Drivers
{
    [Binding]
    public class Driver
    {
        private readonly IObjectContainer _objectContainer;

        public Driver(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }


    }

}