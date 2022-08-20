using SigmaDi;
using SigmaDi.Exceptions;
using SigmaDiTests.FakeData;
using Xunit;

namespace SigmaDiTests
{
    public class RegistrationServicesTests
    {
        [Fact]
        public void Registration_Services_Without_Exception()
        {
            // Arrange
            var container = new Container();
            container.AddNew<IBusinessLayer, BusinessLayer>();
            container.AddNew<IDataProvider, DataProvider>();

            // Act
            var exception = Record.Exception(() => container.Init());
            
            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Throw_Exception_When_Two_Constructors_With_Params()
        {
            // Arrange
            var container = new Container();
            container.AddNew<IBusinessLayer, BusinessLayer>();
            container.AddNew<IDataProvider, TwoConstructor>();

            // Act Assert
            Assert.Throws<SigmaDiException>(() => container.Init());
        }
    }
}
