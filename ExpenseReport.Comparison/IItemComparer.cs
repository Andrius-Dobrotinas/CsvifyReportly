using System;

namespace Andy.ExpenseReport.Comparison
{
    public interface IItemComparer<in TTransaction1, in TTransaction2>
    {
        bool AreEqual(TTransaction1 item1, TTransaction2 item2);
    }
}