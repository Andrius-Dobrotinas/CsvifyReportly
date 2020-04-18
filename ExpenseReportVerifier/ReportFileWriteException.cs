namespace Andy.ExpenseReport
{
    public class ReportFileWriteException : ConsoleApplicationLevelException
    {
        public ReportFileWriteException(string message)
            : base(-300, "Failed to write the report file", message)
        {
        }
    }
}