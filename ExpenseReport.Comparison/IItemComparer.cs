using System;

namespace Andy.ExpenseReport.Comparison
{
    public interface IItemComparer<in TTransaction1, in TTransaction2>
    {
        bool AreEqual(TTransaction1 transaction1, TTransaction2 transaction2);
    }
}