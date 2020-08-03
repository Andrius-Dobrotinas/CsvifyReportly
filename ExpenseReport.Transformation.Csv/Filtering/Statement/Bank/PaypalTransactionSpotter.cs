using Andy.Csv.Transformation.Row.Filtering;
using System;

namespace Andy.ExpenseReport.Transformation.Csv.Filtering.Statement.Bank
{
    public interface IPaypalTransactionSpotter : IValueComparer
    {
    }

    public class PaypalTransactionSpotter : IPaypalTransactionSpotter
    {
        private const string paypalPrefix = "PAYPAL *";

        public bool IsMatch(string detailsString)
        {
            if (detailsString == null) throw new ArgumentNullException(nameof(detailsString));

            return detailsString.StartsWith(paypalPrefix, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}