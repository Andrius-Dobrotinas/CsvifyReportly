using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison
{
    public interface IMerchantStringComparer
    {
        bool DoStatementDetailsReferToMerchant(string statementDetails, string reportMerchant, bool isViaPayPal);
    }

    public class MerchantComparer : IMerchantStringComparer
    {
        // Take this as a constructor parameter
        private const string paypalDescriptionPrefix = "PAYPAL *";
        private static int paypalDescriptionPrefixLength = paypalDescriptionPrefix.Length;

        public bool DoStatementDetailsReferToMerchant(string statementDetails, string merchant, bool isViaPayPal)
        {
            string statementMerchantDetails;

            if (isViaPayPal)
            {
                bool isStatementEntryPayPal = statementDetails != null
                    && statementDetails.StartsWith(paypalDescriptionPrefix);

                if (isStatementEntryPayPal)
                    statementMerchantDetails = statementDetails.Substring(paypalDescriptionPrefixLength);
                else
                    return false;
            }
            else
                statementMerchantDetails = statementDetails;

            return string.Equals(statementMerchantDetails, merchant, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
