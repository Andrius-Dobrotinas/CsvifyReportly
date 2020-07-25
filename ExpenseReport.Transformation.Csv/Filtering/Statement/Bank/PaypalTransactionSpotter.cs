using System;

namespace Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank
{
    public interface IPaypalTransactionSpotter
    {
        bool IsPaypalTransaction(string detailsString);
    }

    public class PaypalTransactionSpotter : IPaypalTransactionSpotter
    {
        private const string paypalPrefix = "PAYPAL *";

        public bool IsPaypalTransaction(string detailsString)
        {
            if (detailsString == null) throw new ArgumentNullException(nameof(detailsString));

            return detailsString.StartsWith(paypalPrefix, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}