using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardApplication
{
    public class FraudLookup
    {
        //must be virtual
        //virtual public bool IsFraudRisk(CreditCardApplication application)
        //{
        //    if (application.LastName == "Smith")
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        public bool IsFraudRisk(CreditCardApplication application)
        {
            return CheckApplication(application);
        }
        
        protected virtual bool CheckApplication(CreditCardApplication application)
        {
            if (application.LastName == "Smith")
            {
                return true;
            }

            return false;
        }
    }
}
