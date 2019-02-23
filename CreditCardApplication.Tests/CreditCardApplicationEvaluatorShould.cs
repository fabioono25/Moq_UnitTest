using System;
using Xunit;
using Moq;

namespace CreditCardApplication.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            
            //system under test
            //var sut = new CreditCardApplicationEvaluator(null);
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

            var decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            //forcing to pass application.Age <= AutoReferralMaxAge
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //ServiceInformation musnt`t null
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey);

            mockValidator.DefaultValue = DefaultValue.Mock;
            
            //system under test
            //var sut = new CreditCardApplicationEvaluator(null);
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            //error
            var decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            //changing retur value of mock object
            //mockValidator.Setup(x => x.IsValid("x")).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            //forcing start with x -- is method
            //mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith('x')))).Returns(true);

            //forcing one letter
            //mockValidator.Setup(x => x.IsValid(It.IsIn("x", "y", "z"))).Returns(true);
            //mockValidator.Setup(x => x.IsValid(It.IsInRange("a", "z", Range.Inclusive))).Returns(true);
            mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]", System.Text.RegularExpressions.RegexOptions.None))).Returns(true);

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "x"
            };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        //testing strict and loose mocks
        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            //Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);

            //for strict mock, we need to specify setup for all correspondations
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplicationsOutDemo()
        {
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            bool isValid = true;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };

            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferWhenLicenceKeyExpired()
        {
            //var mockValidator = new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //mock property
            //mockValidator.Setup(x => x.LicenceKey).Returns("EXPIRED");
            //mockValidator.Setup(x => x.LicenceKey).Returns(GetLicenseKeyExpiryString);

            //properties hierarchies
            //var mockLicenseData = new Mock<ILicenseData>(); //level 1
            //mockLicenseData.Setup(x => x.LicenseKey).Returns("EXPIRED");

            //var mockServiceInfo = new Mock<IServiceInformation>(); //level 2
            //mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);

            //mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);

            //turning easy setup hierarchi properties
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("EXPIRED");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 42 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLicenseKeyExpiryString()
        {
            //getting return value from a function
            return "EXPIRED";
        }

        [Fact]
        public void UseDetailedLookupForOlderApplications()
        {
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            mockValidator.SetupAllProperties(); //changing track enabled
            //we want to track changes to properties
            //mockValidator.SetupProperty(x => x.ValidationMode);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 30 };

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }
    }
}
