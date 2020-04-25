using System;

namespace Andy.ExpenseReport.Comparison
{
    public interface IItemComparer<in TItem1, in TItem2>
    {
        bool AreEqual(TItem1 item1, TItem2 item2);
    }
}