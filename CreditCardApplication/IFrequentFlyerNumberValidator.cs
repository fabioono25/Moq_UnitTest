using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardApplication
{
    public interface ILicenseData
    {
        string LicenseKey { get;  }
    }

    public interface IServiceInformation
    {
        ILicenseData License { get; }
    }

    public enum ValidationMode
    {
        Quick,
        Detailed
    }

    public interface IFrequentFlyerNumberValidator
    {
        bool IsValid(string frequentFlyerNumber);

        bool IsValid(string frequentFlyerNumber, out bool isValid);

        //string LicenceKey { get;  }
        IServiceInformation ServiceInformation { get; } //nested interfaces

        ValidationMode ValidationMode { get; set; }

        event EventHandler ValidatorLookupPerformed;
    }
}
