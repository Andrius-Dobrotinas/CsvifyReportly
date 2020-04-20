namespace Andy.ExpenseReport.Cmd
{
    public class SourceDataReadException : ConsoleApplicationLevelException
    {
        public SourceDataReadException(string message)
            : base(-100, "Failed to read the source data", message)
        {
        }
    }
}