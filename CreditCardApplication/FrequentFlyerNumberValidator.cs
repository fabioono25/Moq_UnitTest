using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardApplication
{
    public class FrequentFlyerNumberValidator : IFrequentFlyerNumberValidator
    {
        public IServiceInformation ServiceInformation => throw new NotImplementedException();

        public ValidationMode ValidationMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler ValidatorLookupPerformed;

        //public string LicenceKey => throw new NotImplementedException();

        public bool IsValid(string frequentFlyerNumber)
        {
            throw new NotImplementedException();
        }

        public bool IsValid(string frequentFlyerNumber, out bool isValid)
        {
            throw new NotImplementedException();
        }
    }
}
