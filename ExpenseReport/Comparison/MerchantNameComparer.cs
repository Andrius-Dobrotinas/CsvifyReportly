using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison
{
    public interface IMerchantNameComparer
    {
        bool DoStatementDetailsReferToMerchant(string statementDetails, string reportMerchant, bool isViaPayPal);
    }

    public class MerchantNameComparer : IMerchantNameComparer
    {
        // Take this as a constructor parameter
        private const string paypalDescriptionPrefix = "PAYPAL *";
        private static int paypalDescriptionPrefixLength = paypalDescriptionPrefix.Length;
        private static readonly string[] amazonPrefixes = new string[] { "AMAZON", "AMZNMKTPLACE", "AMZ*AMAZON.CO.UK" };

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

            if (string.Equals(statementMerchantDetails, merchant, StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (IsAmazon(merchant) && IsAmazon(statementMerchantDetails))
                return true;
            return false;
        }

        private static bool IsAmazon(string @string)
        {
            return amazonPrefixes
                    .Any(
                        amzPrefix => @string.StartsWith(
                            amzPrefix,
                            StringComparison.InvariantCultureIgnoreCase));
        }
    }
}