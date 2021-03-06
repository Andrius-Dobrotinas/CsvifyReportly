﻿using System;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class DataComparisonException : CsvStreamComparisonException
    {
        public DataComparisonException(Exception e)
            : base("An error occured while performing a comparison", e)
        {
        }
    }
}