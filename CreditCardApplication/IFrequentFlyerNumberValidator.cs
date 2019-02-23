using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardApplication
{
    public interface IFrequentFlyerNumberValidator
    {
        bool IsValid(string frequentFlyerNumber);

        bool IsValid(string frequentFlyerNumber, out bool isValid);
    }
}
